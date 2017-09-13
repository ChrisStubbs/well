using System;
using System.Data.SqlClient;
using System.Reflection;

namespace PH.Well.UnitTests.Factories
{
    using System.Linq;

    public class SqlExceptionCreator
    {
        private static T Construct<T>(params object[] p)
        {
            var ctor = (from ctors in typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                where ctors.GetParameters().Count() == p.Count()
                select ctors).Single();
            return (T)ctor.Invoke(p);
        }

        public static SqlException CreateSqlException(int errorNumber)
        {

            var collection = Construct<SqlErrorCollection>();
            var error = Construct<SqlError>(errorNumber, (byte)2, (byte)3, "server name", "This is a Mock-SqlException", "proc", 100);

            typeof(SqlErrorCollection)
                .GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(collection, new object[] { error });


            var e = typeof(SqlException)
                .GetMethod("CreateException", BindingFlags.NonPublic | BindingFlags.Static, null, CallingConventions.ExplicitThis, new[] { typeof(SqlErrorCollection), typeof(string) }, new ParameterModifier[] { })
                .Invoke(null, new object[] { collection, "7.0.0" }) as SqlException;

            return e;
        }
    }
}