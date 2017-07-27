using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Common.Extensions
{
    public static class IListExtentions
    {
        public static DataTable ToIntDataTables(this IList<int> data, string columnName) 
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException(nameof(columnName));
            }

            var table = new DataTable();

            table.Columns.Add(columnName, typeof(int));

            foreach (var item in data)
            {
                table.Rows.Add(new object[] { item });
            }

            return table;
        }

        public static DataTable ToDataTables<T>(this IList<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prp = props[i];

                if (prp.PropertyType.IsGenericType)
                {
                    if (prp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        table.Columns.Add(prp.Name, Nullable.GetUnderlyingType(prp.PropertyType));
                    }
                    else
                    {
                        throw new NotSupportedException("ToDataTables only supports generic types of Nullable<T>");

                    }
                }
                else if (prp.PropertyType.IsClass && prp.PropertyType != typeof(string))
                {
                    throw new NotSupportedException("ToDataTables doesn't support class types");
                }
                else
                {
                    table.Columns.Add(prp.Name, prp.PropertyType);
                }
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
