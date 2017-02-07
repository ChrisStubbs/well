using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace PH.Well.BDD.Framework.Extensions
{
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


        private static Tuple<string, int> Bla()
        {
            return new Tuple<string, int>("", 1);
        }

        public static bool IsEqualToSpecFlowTable<T>(this WebElements.Grid<T> grid,
            Table table,
            Func<string, string, bool> headerMatcher = null,
            Func<string, string, string, bool> cellComparer = null)
        {
            var header = grid.Header;
            var rows = grid.ReturnAllRows().ToList();
            var returnValue = false;

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

            if (table.RowCount == rows.Count)
            {
                returnValue = true;
                //loop thought all the rows
                for (int i = 0; i < rows.Count; i++)
                {
                    returnValue = returnValue && header
                        .Where(p => finder.ContainsKey(p.Text))
                        .All(p => cellComparer(rows[i].GetTdValueByIndex(finder[p.Text].Item3), table.Rows[i][finder[p.Text].Item2], p.Text));
                }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }
    }
}
