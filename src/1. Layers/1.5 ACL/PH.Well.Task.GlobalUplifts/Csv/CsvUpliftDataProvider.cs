using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Common.FileNode.CSV;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Csv
{
    public class CsvUpliftDataProvider : IUpliftDataProvider
    {
        private readonly string _filePath;

        public CsvUpliftDataProvider(string filePath)
        {
            _filePath = filePath;
        }

        private List<string> _headers;
        private int _branchNumberIndex;
        private int _accountNumberIndex;
        private int _creditReasonIndex;
        private int _productCodeIndex;
        private int _quantityIndex;
        private int _startDateIndex;
        private int _endDateIndex;

        private List<ValidationResult> _validationResults;


        public IEnumerable<IUpliftData> GetUpliftData()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IUpliftData> ParseFile()
        {
            var csvFile = new CsvFileNode(1);
            var lines = csvFile.Parse(_filePath, true);

            _validationResults = new List<ValidationResult>();

            //Get header
            _headers = lines.First();

            //Set indexes
            _branchNumberIndex = FindHeaderIndex("BRANCH");
            _accountNumberIndex = FindHeaderIndex("ACC NO");
            _creditReasonIndex = FindHeaderIndex("CREDIT REASON CODE");
            _productCodeIndex = FindHeaderIndex("PRODUCT CODE");
            _quantityIndex = FindHeaderIndex("QTY");
            _startDateIndex = FindHeaderIndex("Start Date");
            _endDateIndex = FindHeaderIndex("End Date");

            foreach (var line in lines.Skip(1))
            {
                List<string> memberErrors = new List<string>();

                var branchNumberString = line[_branchNumberIndex]?.Trim();
                var accountNumberString = line[_accountNumberIndex]?.Trim();
                var creditReasonString = line[_creditReasonIndex]?.Trim();
                var productCodeString = line[_productCodeIndex]?.Trim();
                var quantityString = line[_quantityIndex]?.Trim();
                var startDateString = line[_quantityIndex]?.Trim();
                var endDateString = line[_quantityIndex]?.Trim();

                int branchNumber = 0;
                if (!int.TryParse(branchNumberString,out branchNumber))
                {
                    memberErrors.Add($"Invalid branch number {branchNumberString}");
                }

                if (string.IsNullOrWhiteSpace(accountNumberString))
                {
                    memberErrors.Add($"Invalid account number {accountNumberString}");
                }

                if (string.IsNullOrWhiteSpace(creditReasonString))
                {
                    memberErrors.Add($"Invalid credit reason code {creditReasonString}");
                }

                int productCode = 0;
                if (!int.TryParse(productCodeString, out productCode))
                {
                    memberErrors.Add($"Invalid product code {productCodeString}");
                }

                int quantity = 0;
                if (!int.TryParse(quantityString, out quantity) || quantity <= 0)
                {
                    memberErrors.Add($"Invalid quantity {quantityString}");
                }

                DateTime startDate;
                if (!DateTime.TryParse(startDateString, out startDate))
                {
                    memberErrors.Add($"Invalid start date {quantityString}");
                }
            }
        }

        /// <summary>
        /// Finds index of given header value using case insensitive search
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private int FindHeaderIndex(string header)
        {
            return _headers.Select(x => x.ToLower().Trim()).ToList().IndexOf(header.ToLower().Trim());
        }
    }
}
