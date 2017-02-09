using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace PH.Well.BDD.Framework.Extensions
{
    public class ContainsSpecFlowTableResult
    {
        public ContainsSpecFlowTableResult()
        {
            this.Errors = new List<CellError>();
        }

        public IList<CellError> Errors { get; set; }

        public bool RowCountMatches { get; set; }

        public bool HasError
        {
            get
            {
                return !(this.RowCountMatches && this.Errors.Count == 0);
            }
        }

        public class CellError
        {
            public string GridCellText { get; set; }
            public string TableCellText { get; set; }
            public string Column { get; set; }
            public int RowNumber { get; set; }
        }
    }

    public static class GridExtensions
    {
        public static Func<string, string, bool> defaultHeaderMatcher = (left, right) =>
        {
            return left.Replace(" ", string.Empty).Equals(right, StringComparison.InvariantCultureIgnoreCase);
        };

        public static Func<string, string, string, bool> defaultCellComparer = (left, right, column) =>
        {
            return left == right;
        };
        
        public static ContainsSpecFlowTableResult ContainsSpecFlowTable<T>(this WebElements.Grid<T> grid,
            Table table,
            Func<string, string, bool> headerMatcher = null,
            Func<string, string, string, bool> cellComparer = null)
        {
            var header = grid.Header;
            var rows = grid.ReturnAllRows().ToList();
            var returnValue = new ContainsSpecFlowTableResult();

            //if null set it to the default matcher
            headerMatcher = headerMatcher ?? defaultHeaderMatcher;
            //if null set it to the default comparer
            cellComparer = cellComparer ?? defaultCellComparer;

            //here i am going to find the position of the header inside the array
            Func<string, Tuple<string, string, int>> indexFinder = (headerName) =>
            {
                var result = header
                    .Select((p, index) => new Tuple<string, string, int>(p.Text, headerName, index))
                    .FirstOrDefault(p => headerMatcher(p.Item1, headerName));

                if (result == null)
                {
                    throw new MissingFieldException($"Missing field {headerName}");
                }

                return result;
            };

            //store the position and the header name inside a dictionary to be easy to find
            var finder = table.Header
                .Select(p => indexFinder(p))
                .ToDictionary(k => k.Item1, v => v);

            //loop thought all the rows
            for (int i = 0; i < rows.Count; i++)
            {
                var cells = header
                    .Where(p => finder.ContainsKey(p.Text));

                foreach (var cell in cells)
                {
                    var tableCellText = table.Rows[i][finder[cell.Text].Item2];
                    var gridCellText = rows[i].GetTdValueByIndex(finder[cell.Text].Item3);

                    if (!cellComparer(gridCellText, tableCellText, cell.Text))
                    {
                        //it is not equal so lets add it to the error list
                        returnValue.Errors.Add(new ContainsSpecFlowTableResult.CellError
                        {
                            TableCellText = tableCellText,
                            GridCellText = gridCellText,
                            Column = cell.Text,
                            RowNumber = i
                        });
                    }
                }
            }

            returnValue.RowCountMatches = table.RowCount == rows.Count;

            return returnValue;
        }
    }
}
