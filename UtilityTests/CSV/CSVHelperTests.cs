using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Utility;
using Utility.CSV;

namespace UtilityTests.CSV
{
    [TestClass]
    public class CSVHelperTests
    {
        /// <summary>
        /// Test CSVHelper read csv file correctly
        /// </summary>
        [TestMethod]
        public void ReadCsvFastTest()
        {
            // arrange
            var mockIOHelper = new Mock<IIOHelper>();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes("id,email\r\nid1,test1.com\r\nid2,test2.com"));
            using (var streamReader = new StreamReader(stream))
            {
                // act
                var csv = new CsvHelper("");
                csv.ReadCsvFast(streamReader, ',', true);

                // assert
                var expected = new Collection<string> {"id", "email"};
                CollectionAssert.AreEquivalent(expected, csv.ColumnList);

                var recordExpected = new List<string>()
                {
                    "id1","test1.com"
                };
                CollectionAssert.AreEquivalent(recordExpected, csv.Rows[0].ItemArray.Select(a => a.ToString()).ToList());
            }
        }
    }
}