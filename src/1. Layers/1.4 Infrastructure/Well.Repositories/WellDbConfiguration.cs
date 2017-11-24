﻿namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using Contracts;
    using Shared.Well.Data.EF.Contracts;

    public class WellDbConfiguration : BaseDbConfiguration, IWellDbConfiguration, IWellEntitiesConnectionString, IDbMultiConfiguration
    {
        public WellDbConfiguration()
        {
            ConnectionStrings = new List<string> { DatabaseConnection };
        }
        public string DatabaseConnection => GetConnectionString("Well").ConnectionString;

        private ConnectionStringSettings GetConnectionString(string connectionStringKey)
        {
            ConnectionStringSettings conStringSettings = ConfigurationManager.ConnectionStrings[connectionStringKey];

            if (conStringSettings == null)
            {
                throw new ConstraintException($"{connectionStringKey} ConnectionString not found");
            }
            return conStringSettings;
        }

        private string entitiesNameOrConnection;
        public string NameOrConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(entitiesNameOrConnection))
                {
                    entitiesNameOrConnection = GetConnectionString("WellEntities").ConnectionString;
                }
                return entitiesNameOrConnection;
            }
            set
            {
                entitiesNameOrConnection = value;
            }
        }

        public IList<string> ConnectionStrings { get; }
    }
}
