﻿namespace PH.Well.Repositories
{
    using System.Configuration;

    public struct Configuration
    {
        public static int TransactionTimeout = int.Parse(ConfigurationManager.AppSettings["transactionTimeout"]);
    }
}