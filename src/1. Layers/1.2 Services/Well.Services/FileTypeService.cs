namespace PH.Well.Services
{
    using System;

    using PH.Well.Common.Extensions;
    using PH.Well.Domain.Enums;
    using PH.Well.Services.Contracts;

    public class FileTypeService : IFileTypeService
    {
        public EpodFileType DetermineFileType(string filename)
        {
            try
            {
                var position = filename.IndexOf("_", StringComparison.Ordinal);

                return StringExtensions.GetValueFromDescription<EpodFileType>(filename.Substring(0, position + 1));
            }
            catch
            {
                return EpodFileType.Unknown;
            }
        }
    }
}