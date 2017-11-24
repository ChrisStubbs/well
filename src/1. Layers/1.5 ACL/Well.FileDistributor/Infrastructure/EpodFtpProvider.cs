﻿namespace PH.Well.FileDistributor.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Contracts;
    using Common.Contracts;
    using Common;
    using Domain.ValueObjects;
    using System.Xml;
    using System.Xml.Linq;
    using System.Configuration;
    using Newtonsoft.Json;

    public class EpodFtpProvider : IEpodProvider
    {
        private readonly IFtpClient ftpClient;
        private readonly IWebClient webClient;
        private readonly IEventLogger eventLogger;
        private readonly ILogger logger;
        private readonly Lazy<BranchGroups> branchGroupsSetup;

        private Dictionary<string, int> depots = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase)
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

        public EpodFtpProvider(
            ILogger logger,
            IFtpClient ftpClient,
            IWebClient webClient,
            IEventLogger eventLogger)
        {
            this.logger = logger;
            this.ftpClient = ftpClient;
            this.webClient = webClient;
            this.eventLogger = eventLogger;

            this.ftpClient.FtpLocation = Configuration.FtpLocation;
            this.ftpClient.FtpUserName = Configuration.FtpUsername;
            this.ftpClient.FtpPassword = Configuration.FtpPassword;
            this.webClient.Credentials = new NetworkCredential(Configuration.FtpUsername, Configuration.FtpPassword);

            branchGroupsSetup = new Lazy<BranchGroups>(() =>
            {
                //var collection = ConfigurationManager.AppSettings;
                //const string AppSettingFinder = "BranchGroups";

                //return JsonConvert.DeserializeObject<BranchGroups>(collection.Cast<string>()
                //.Select(key => new KeyValuePair<string, string>(key, collection[key]))
                //.First(p => p.Key == AppSettingFinder).Value);

                return JsonConvert.DeserializeObject<BranchGroups>(Configuration.BranchGroups);
            });

        }

        public void Import()
        {
            var listings = new List<DirectoryListing>();

            using (var response = this.ftpClient.GetResponseStream())
            {
                using (var reader = new StreamReader(response))
                {
                    var routeFile = string.Empty;

                    while ((routeFile = reader.ReadLine()) != null)
                    {
                        listings.Add(new DirectoryListing(routeFile));
                    }
                }
            }

            foreach (var listing in listings.OrderBy(x => x.Datetime))
            {
                var tempFilename = GetTemporaryFilename(listing.Filename, Guid.NewGuid());
                var downloadedFile = this.webClient.CopyFile(Configuration.FtpLocation + "/" + listing.Filename,
                                        Path.Combine(Configuration.DownloadFilePath, tempFilename));

                if (string.IsNullOrWhiteSpace(downloadedFile))
                {
                    this.logger.LogDebug($"FileDistributor file not copied from FTP {listing.Filename}!");
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"FileDistributor file not copied from FTP {listing.Filename}!", EventId.FtpFileDistributorFileNotCopied);

                    continue;
                }

                int branchId;
                string folderName;
                try
                {
                    branchId = this.GetBranchNumber(downloadedFile);
                    folderName = this.GetGroupFolder(branchId);
                }
                catch (ConfigurationErrorsException ex)
                {
                    this.logger.LogError(ex.Message);
                    continue;
                }
                
                var folder = Path.Combine(Configuration.DownloadFilePath,
                    folderName);
                var targetFileName = Path.Combine(folder, listing.Filename);

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                if (!File.Exists(targetFileName))
                {
                    
                    if (!File.Exists(targetFileName))
                    {
                        File.Move(downloadedFile, targetFileName);
                        this.logger.LogDebug($"Success! File {listing.Filename} copied from ftp!");
                    }

                    if (Configuration.DeleteFtpFileAfterImport)
                    {
                        this.ftpClient.DeleteFile(Path.GetFileName(targetFileName));
                    }
                    // Abort if a file called stop.txt exists in exe folder
                    if (File.Exists("stop.txt"))
                    {
                        File.Delete("stop.txt");
                        this.logger.LogDebug("Process stopped due Stop.txt file");
                        return;
                    }
                }
                else
                {
                    this.logger.LogDebug($"File { listing.Filename } already exists in target folder. File not copied from FTP.");
                }
            }
        }

        private string GetGroupFolder(int branchId)
        {
            try
            {
                return branchGroupsSetup.Value.GetGroupNameForBranch(branchId);
            }
            catch
            {
                throw new ConfigurationErrorsException($"No configuration found for branch {branchId}. Please check your app.config settings and make sure it's correctly configure on ConnectionStringGroups");
            }
        }

        private int GetBranchNumber(string fileName)
        {
            if (fileName.Contains("ORDER_") || fileName.Contains("ROUTE_"))
            {
                var parts = fileName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                var branch = parts[1].ToLower();

                if (depots.ContainsKey(branch))
                {
                    return depots[branch];
                }

                throw new ConfigurationErrorsException($"No branch found on file {fileName}");
            }

            using (var file = File.OpenRead(fileName))
            {
                using (var reader = XmlReader.Create(file))
                {
                    while (reader.Read())
                    {
                        reader.MoveToContent();

                        if (reader.NodeType == XmlNodeType.Element
                            && string.Equals(reader.Name, "EntityAttributeValue", StringComparison.CurrentCultureIgnoreCase))
                        {
                            var el = XNode.ReadFrom(reader) as XElement;

                            if (el != null)
                            {
                                var xElements = el.DescendantNodes()
                                    .Where(p => p is XElement)
                                    .Select(p => (XElement)p)
                                    .ToList();

                                var hasRouteOwner = xElements
                                    .Any(p => string.Equals(p.Name.LocalName, "Code", StringComparison.CurrentCultureIgnoreCase)
                                           && string.Equals(p.Value, "ROUTEOWNER", StringComparison.CurrentCultureIgnoreCase));

                                if (hasRouteOwner)
                                {
                                    var branch = xElements.First(p => p.Name == "Value1").Value;

                                    if (depots.ContainsKey(branch))
                                    {
                                        return depots[branch];
                                    }
                                    throw new ConfigurationErrorsException($"No branch found on file {fileName}");
                                }
                            }
                        }
                    }
                }
            }

            throw new ConfigurationErrorsException($"No branch found on file {fileName}");
        }

        private string GetTemporaryFilename(string filename, Guid guid)
        {
            return $"{guid}{filename}";
        }
    }
}
