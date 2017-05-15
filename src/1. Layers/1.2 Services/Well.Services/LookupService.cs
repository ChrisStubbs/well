using System;
using System.Collections.Generic;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class LookupService : ILookupService
    {
        private readonly ILookupRepository lookupRepository;

        public LookupService(ILookupRepository lookupRepository)
        {
            this.lookupRepository = lookupRepository;
        }

        public IList<KeyValuePair<string, string>> GetLookup(LookupType lookupType)
        {
            switch (lookupType)
            {
                case LookupType.ExceptionType:
                    return this.lookupRepository.ExceptionTypes();

                case LookupType.ExceptionAction:
                    throw new ArgumentException($"{lookupType}");

                default:
                    throw new ArgumentException($"{lookupType}");
            }
        }
    }
}
