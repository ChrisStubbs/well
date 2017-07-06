using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using PH.Common.FileNode.CSV;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Csv
{
    public class CsvUpliftDataProvider : IUpliftDataProvider
    {
        private readonly string _filePath;
        private TextReader _textReader;
        private bool _createReader;

        private List<string> _headers;
        private int _branchNumberIndex;
        private int _accountNumberIndex;
        private int _creditReasonIndex;
        private int _productCodeIndex;
        private int _quantityIndex;
        private int _startDateIndex;
        private int _endDateIndex;
      


        public CsvUpliftDataProvider(string filePath) : this()
        {
            _filePath = filePath;
            _createReader = true;
        }

        public CsvUpliftDataProvider(TextReader textReader) : this()
        {
            _textReader = textReader;
        }

        protected CsvUpliftDataProvider()
        {
            MaxUpliftStartDate = DateTime.Now;
        }

        public DateTime MaxUpliftStartDate { get; set; }

        /// <summary>
        /// Max value of how many days uplift end date can be greater than start date
        /// </summary>
        public int MaxUpliftEndDateDays { get; set; }


        public UpliftDataSet GetUpliftData()
        {
            var validationResults = new List<ValidationResult>();
            var records = new List<IUpliftData>();

            var csvFile = new CsvFileNode(1);
            var lines = Parse(csvFile);

            // Get header
            _headers = csvFile.Headers;

            // Set indexes
            _branchNumberIndex = FindHeaderIndex("BRANCH");
            _accountNumberIndex = FindHeaderIndex("ACC NO");
            _creditReasonIndex = FindHeaderIndex("CREDIT REASON CODE");
            _productCodeIndex = FindHeaderIndex("PRODUCT CODE");
            _quantityIndex = FindHeaderIndex("QTY");
            _startDateIndex = FindHeaderIndex("Start Date");
            _endDateIndex = FindHeaderIndex("End Date");

            var recordCount = 1;
            foreach (var line in lines)
            {
                List<string> memberErrors = new List<string>();

                var branchNumberString = line[_branchNumberIndex]?.Trim();
                var accountNumberString = line[_accountNumberIndex]?.Trim();
                var creditReasonString = line[_creditReasonIndex]?.Trim();
                var productCodeString = line[_productCodeIndex]?.Trim();
                var quantityString = line[_quantityIndex]?.Trim();
                var startDateString = line[_startDateIndex]?.Trim();
                var endDateString = line[_endDateIndex]?.Trim();

                int branchNumber = 0;
                if (!int.TryParse(branchNumberString, out branchNumber))
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
                if (!DateTime.TryParse(startDateString, out startDate) || startDate > MaxUpliftStartDate)
                {
                    memberErrors.Add($"Invalid start date {startDateString}");
                }


                DateTime endDate;
                if (!DateTime.TryParse(endDateString, out endDate) || endDate < startDate || endDate > startDate.AddDays(MaxUpliftEndDateDays))
                {
                    memberErrors.Add($"Invalid end date {endDateString}");
                }

                if (memberErrors.Any())
                {
                    validationResults.Add(new ValidationResult($"Invalid record. Index : {recordCount}", memberErrors));
                }
                else
                {
                    records.Add(new UpliftDataBase(branchNumber, accountNumberString, creditReasonString, productCode,
                        quantity, startDate, endDate));
                }
            }


            if (validationResults.Any())
            {
                // Return set with errors
                return new UpliftDataSet(records, validationResults);
            }
            else
            {
                // Return set without errors
                return new UpliftDataSet(records);
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


        private List<List<string>> Parse(CsvFileNode csvNode)
        {
            // Call to list is important here to populate Headers property

            if (_createReader)
            {
                return csvNode.Parse(_filePath, true).ToList();
            }
            else
            {
                return csvNode.Parse(_textReader, true).ToList();
            }
        }
    }
}
