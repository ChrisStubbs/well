using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PH.Common.Storage.Constants.Enums;
using PH.Common.Storage;
using PH.Common.Storage.Config.ConfigFile;
using PH.Common.Storage.Constants.Enums;
using PH.Common.Storage.Ftp;
using PH.Common.Storage.Local;
using PH.Shared.Well.Data.EF;

namespace PH.Well.Task.UatFileCopy
{
    class Program
    {
        public static Dictionary<string, int> Depots = new Dictionary<string, int>()
        {
            {"med", 2},
            {"cov", 3},
            {"far", 5},
            {"dun", 9},
            {"lee", 14},
            {"hem", 20},
            {"bir", 22},
            {"bel", 33},
            {"bra", 42},
            {"ply", 55},
            {"bri", 59},
            {"hay", 82}
        };

        static void Main(string[] args)
        {
            Storage.RegisterStorageProviderFactory(eStorageType.Ftp, new FtpStorageProviderFactory());
            Storage.RegisterStorageProviderFactory(eStorageType.Local, new LocalStorageProviderFactory());
            Storage.RegisterStorageConfigProvider(new ConfigFileConfigProvider());
            var branches = ConfigurationManager.AppSettings["branches"].Split(new char[] { ',', ';' },
                StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
            var sources = ConfigurationManager.AppSettings["sources"];
            var archives = ConfigurationManager.AppSettings["archives"];
            var pause = int.Parse(ConfigurationManager.AppSettings["pause"] ?? "60000");
            try
            {
                ProcessFiles(sources, archives, branches, pause);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Processing failed {ex.Message}");
            }
        }

        /// <summary>
        /// Copy matching files, from a number of source locations, to the UAT processing folder
        /// </summary>
        /// <param name="sources">Storage spec listing one or more source file locations</param>
        /// <param name="archives">Storage spec listing one or more archive folder locations</param>
        /// <param name="branches">LIst if branches to filter by</param>
        /// <param name="pause">Number of milliseconds to pause after scanning folder, before checking database for processed files</param>
        private static void ProcessFiles(string sources, string archives, List<int> branches, int pause)
        {
            // Do a macro replacement for date and time
            archives = archives.Replace("{today}", $"{DateTime.Today:yyyyMMdd}");
            sources = sources.Replace("{today}", $"{DateTime.Today:yyyyMMdd}");
            for (int minus = 1; minus < 5; minus++)
            {
                archives = archives.Replace($"{{minus{minus}}}", DateTime.Today.AddDays(-minus).ToString("yyyyMMdd"));
                sources = sources.Replace($"{{minus{minus}}}", DateTime.Today.AddDays(-minus).ToString("yyyyMMdd"));
            }
            // Get all filenames in the archives (for specified dates) first
            var archiveFiles = Storage.GetFiles(archives).Select(x => x.Name.ToLower()).Distinct().ToList();
            Console.WriteLine($"Identified {archiveFiles.Count} existing files in date range");

            // Copy all changed files from the source folders to the target folders
            var files = Storage.GetFiles(sources).ToList();
            Console.WriteLine($"Identified {files.Count} files to scan");

            files = files.Where(x => !archiveFiles.Contains(x.Name.ToLower())).ToList();

            // Wait long enough for any files being processed to complete (1 minute)
            Console.WriteLine($"Waiting {pause / 1000} seconds for any file processing to complete");
            System.Threading.Thread.Sleep(pause);

            // Get a list of all files already processed from the Well database last
            WellEntities entities = new WellEntities();
            var processed = entities.Routes.Select(x => x.FileName.ToLower()).Distinct().ToList();
            Console.WriteLine($"Exclude {processed.Count} files from Well records");

            files = files.Where(x => !processed.Contains(x.Name.ToLower())).ToList();
            Console.WriteLine($"Identified {files.Count} new files to copy");

            int fileCount = 0;
            foreach (var file in files)
            {
                if (file.Size == 0)
                {
                    Console.WriteLine($"Skipping 0 length file {file.FullName}");
                }
                else
                {
                    var target = "UatWellEpod:" + file.Name;
                    var fileInfo = Storage.GetFileInfo(target);
                    if (fileInfo == null || fileInfo.Size != file.Size)
                    {
                        var match = false;
                        try
                        {
                            // Filter the file, by branch, based on filename or content
                            using (var stream = Storage.ReadFile(file.FullName))
                            {
                                if (stream != null)
                                {
                                    match = IsBranch(stream, file.Name, branches);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Failed to read {file.FullName}");
                        }
                        if (match)
                        {
                            Console.WriteLine($"Copying from {file.FullName}");
                            Storage.Copy(file.FullName, target);
                            fileCount++;
                        }
                    }
                }
            }
            Console.WriteLine($"Copied {fileCount} files to UAT Well");
        }

        /// <summary>
        /// Is this a matching branch (default to matching all branches if none are provided)
        /// </summary>
        /// <param name="stream">File stream where the file content is required</param>
        /// <param name="fileName">Filename, for files that contain the branch in the name</param>
        /// <param name="branches">List of branches we will accept to copy</param>
        /// <returns></returns>
        private static bool IsBranch(Stream stream, string fileName, List<int> branches)
        {
            fileName = fileName.ToLower();
            var matchBranch = false;
            if (!branches.Any())
            {
                return true;
            }
            if (fileName.StartsWith("route") || fileName.StartsWith("order"))
            {
                var parts = fileName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                var branch = parts[1].ToLower();
                int branchId = Depots[branch];
                if (branches.Contains(branchId))
                {
                    matchBranch = true;
                }
            }
            else if (fileName.StartsWith("epod"))
            {
                // Do a fast read to the depot element
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    int branchId;
                    bool abort = false;
                    reader.Read();
                    bool inJob = false;
                    while (!abort)
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.IsStartElement())
                                {
                                    switch (reader.Name)
                                    {
                                        case "Depot":
                                            string branch = reader.ReadElementContentAsString();
                                            branchId = Depots[branch.ToLower()];
                                            if (branches.Contains(branchId))
                                            {
                                                matchBranch = true;
                                                abort = true;
                                            }
                                            break;
                                        default:
                                            if (!reader.Read())
                                            {
                                                abort = true;
                                            }
                                            break;
                                    }
                                }
                                break;
                            default:
                                if (!reader.Read())
                                {
                                    abort = true;
                                }
                                break;
                        }
                    }
                }
            }
            // reset stream position
            return matchBranch;
        }
    }
}

