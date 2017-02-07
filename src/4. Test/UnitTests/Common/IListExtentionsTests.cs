using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PH.Well.Common.Extensions;

namespace PH.Well.UnitTests.Common
{
    [TestFixture]
    public class IListExtentionsTests
    {
        [Test]
        public void Should_Convert_List_ToIntDataTables()
        {
            var list = new List<int>
            {
                3, 99, 132
            };

            var dt = list.ToIntDataTables("Value");

            Assert.That(dt.Columns.Count, Is.EqualTo(1));
            Assert.That(dt.Rows.Count, Is.EqualTo(3));
            Assert.That(dt.Rows[0][0], Is.TypeOf<int>());

            for (int i = 0; i < list.Count; i++)
            {
                Assert.That((int)dt.Rows[i][0], Is.EqualTo(list[i]));
            }
        }

        [Test]
        public void Should_Throw_Error_ToIntDataTables()
        {
            List<int> list = null;

            Assert.Throws<ArgumentNullException>(() => list.ToIntDataTables(""));
            list = new List<int>();
            Assert.Throws<ArgumentException>(() => list.ToIntDataTables(""));
            Assert.Throws<ArgumentException>(() => list.ToIntDataTables(" "));
            Assert.Throws<ArgumentException>(() => list.ToIntDataTables(null));
        }

        [Test]
        public void Should_Convert_SimpleClass_ToDataTable()
        {
            var list = new List<TestClassSingleProperty>
            {
                new TestClassSingleProperty { NumericValue = 1},
                new TestClassSingleProperty { NumericValue = 2},
                new TestClassSingleProperty { NumericValue = 3}
            };

            var dt = list.ToDataTables();

            Assert.That(dt.Columns.Count, Is.EqualTo(1));
            Assert.That(dt.Rows.Count, Is.EqualTo(3));
            Assert.That(dt.Rows[0][0], Is.TypeOf<int>());

            for (int i = 0; i < list.Count; i++)
            {
                Assert.That((int)dt.Rows[i][0], Is.EqualTo(list[i].NumericValue));
            }
        }

        [Test]
        public void Should_Throw_Error_With_Props_Complex_Types()
        {
            var list = new List<TestClass2Properties>
            {
                new TestClass2Properties { NumericValue = null, StringValue = null},
                new TestClass2Properties { NumericValue = 2, StringValue = "2"},
            };
        }

        [Test]
        public void Should_Convert_2PropertiesClass_ToDataTable()
        {
            var list = new List<ClassWithComplexTypeProperty>
            {
                new ClassWithComplexTypeProperty { Complex = new TestClassSingleProperty {  NumericValue = 0 } },
            };

            Assert.Throws<NotSupportedException>(() => list.ToDataTables());
        }

        private class ClassWithComplexTypeProperty
        {
            public TestClassSingleProperty Complex { get; set; }
        }

        private class TestClass2Properties
        {
            public int? NumericValue { get; set; }
            public string StringValue { get; set; }
        }

        private class TestClassSingleProperty
        {
            public int NumericValue { get; set; }
        }
    }
}
