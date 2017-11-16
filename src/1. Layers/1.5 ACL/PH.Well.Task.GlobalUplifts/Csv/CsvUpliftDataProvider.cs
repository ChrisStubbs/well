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
        #region Constants
        private const int TWO_WEEKS = 14;
        #endregion Constants

        #region Private fields
        /// <summary>
        /// Id that gets assigned as result set id
        /// </summary>
        private string _id { get; }
        private readonly string _filePath;
        private readonly string _archivePath;
        private TextReader _textReader;
        private bool _createReader;
        private bool _archiveFile;

        //if most of this fields are used in a single method why are they declared as class fields?
        private List<string> _headers;
        private int _branchNumberIndex;
        private int _accountNumberIndex;
        private int _creditReasonIndex;
        private int _productCodeIndex;
        private int _quantityIndex;
        private int _startDateIndex;
        private int _endDateIndex;
        private int _referenceIndex;
        #endregion Private fields

        #region Properties
        public DateTime MaxUpliftStartDate { get; set; }

        /// <summary>
        /// Max value of how many days uplift end date can be greater than start date (default = 14)
        /// </summary>
        public int MaxUpliftEndDateDays { get; set; }

        public string CreditReasonCode { get; set; }
        #endregion Properties

        #region Constructors
        public CsvUpliftDataProvider(string filePath,string archivePath) : this()
        {
            _filePath = filePath;
            _archivePath = archivePath;
            _id = Path.GetFileName(_filePath);
            _createReader = true;
            _archiveFile = true;
        }

        public CsvUpliftDataProvider(string id,TextReader textReader) : this()
        {
            _textReader = textReader;
            _id = id;
        }

        protected CsvUpliftDataProvider()
        {
            // Set defaults
            MaxUpliftStartDate = DateTime.Now;
            CreditReasonCode = "GLOBAL UPLIFT";
            MaxUpliftEndDateDays = TWO_WEEKS;
        }
        #endregion Constructors

        #region Public methods
        public IEnumerable<UpliftDataSet> GetUpliftData()
        {
            var validationResults = new List<ValidationResult>();
            var records = new List<IUpliftData>();

            var csvFile = new CsvFileNode();
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
            _referenceIndex = FindHeaderIndex("Ref");

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
                var referenceString = line[_referenceIndex]?.Trim();

                // Validation logic could be broken down into parsing errors and data errors
                int branchNumber = 0;
                if (!int.TryParse(branchNumberString, out branchNumber))
                {
                    memberErrors.Add($"Invalid branch number {branchNumberString}");
                }

                if (string.IsNullOrWhiteSpace(accountNumberString))
                {
                    memberErrors.Add($"Invalid account number {accountNumberString}");
                }

                if (string.IsNullOrWhiteSpace(creditReasonString) || !creditReasonString.Equals(CreditReasonCode, StringComparison.CurrentCultureIgnoreCase))
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
                    memberErrors.Add($"Invalid start date {startDateString}");
                }
                else if (startDate < MaxUpliftStartDate)
                {
                    memberErrors.Add($"Start date < max start date {startDateString}");
                }


                DateTime endDate;
                if (!DateTime.TryParse(endDateString, out endDate))
                {
                    memberErrors.Add($"Invalid end date {endDateString}");
                }
                else if (endDate < startDate || endDate > startDate.AddDays(MaxUpliftEndDateDays))
                {
                    memberErrors.Add($"Invalid end date {endDateString}");
                }

                if (memberErrors.Any())
                {
                    validationResults.Add(new ValidationResult($"Invalid record. Data line : {recordCount}", memberErrors));
                }
                else
                {
                    records.Add(new UpliftDataBase(recordCount, branchNumber, accountNumberString, creditReasonString,
                        productCode,
                        quantity, startDate, endDate, referenceString));
                }

                recordCount++;
            }

            if (_archiveFile)
            {
                var target = Path.Combine(_archivePath, _id);
                if (File.Exists(target) && target != _filePath)
                {
                    File.Delete(target);
                }
                File.Move(_filePath, target);
            }

            if (validationResults.Any())
            {
                // Return set with errors
                return new[] {new UpliftDataSet(_id, Enumerable.Empty<IUpliftData>(), validationResults)};
            }
            else
            {
                // Return set without errors
                return new[] { new UpliftDataSet(_id, records) };
            }
        }
        #endregion Public methods

        #region Private helper methods
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
        #endregion Private helper methods
    }
}
