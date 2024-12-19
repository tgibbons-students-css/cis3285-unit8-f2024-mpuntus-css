using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {




        private int CountDbRecords()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\nikmi\\Documents\\Software Class\\Unit 8\\Learning Activity\\cis3285-unit8-f2024-mpuntus-css\\Unit8_SRP_F24\\DataFiles\\tradedatabase.mdf\";Integrated Security=True;Connect Timeout=30";

            //string azureConnectString = @"Server=tcp:cis3285-sql-server.database.windows.net,1433; Initial Catalog = Unit8_TradesDatabase; Persist Security Info=False; User ID=cis3285;Password=Saints4SQL; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 60;";
            // Change the connection string used to match the one you want
            //using (var connection = new SqlConnection(azureConnectString))
            using (var connection = new SqlConnection(connectionString))

            {
                if (connection.State == ConnectionState.Closed)
                {
                    //connection.Open();
                }
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                myCommand.Connection.Open();
                
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }
        
        //negative lot size
        [TestMethod()]
        public void TestNegativeNormalFiles()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.negativetrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 0, countAfter);
        }

        //10 trades
        [TestMethod()]
        public void TestTenNormalFiles()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.goodtrades_ten.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 10, countAfter);
        }

        // no trades, empty file
        [TestMethod()]
        public void TestNullNormalFiles()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.badtrades_null.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 0, countAfter);
        }


        /*
        [TestMethod()]
        public void TestExceptionNormalFiles()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.Non_Exsisting_File.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore, countAfter);
        }
        */


        // from Learning Activity, only 1 trade
        [TestMethod()]
         public void TestNormalFile()
         {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.goodtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 1, countAfter);
         }

        
        //negative price
        [TestMethod()]
        public void TestOneBadNormalFiles()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24Tests.badtrades_one_negative.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countBefore = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);
            //Assert
            int countAfter = CountDbRecords();
            Assert.AreEqual(countBefore + 0, countAfter);
        }


        // As a developer,
        //I want the ReadTradeData_ValidInput_ReturnsLines() method to
        //determine trade data and handle unexpected formats properly,
        //so that the system can provide meaningful
        //feedback and avoid processing invalid or unsupported trades.

        [TestMethod()]
        public void ReadTradeData_ValidInput_ReturnsLines()
        {
            // Arrange
            string input = "PSBEYD,1000,1.51\nEURUSD,500,1.12";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;

            var tradeProcessor = new TradeProcessor();

            // Act
            var result = tradeProcessor.ReadTradeData(stream);

            // Assert
            Assert.AreEqual(0, result.Count());
        }


        //I want the ReadTradeData_EmptyFile_ReturnsEmptyList method
        //to handle empty files properly,
        //so that the system does not
        //throw errors and can return an empty
        //list when no trade data is present in the file.
        
        [TestMethod()]
        public void ReadTradeData_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            string input = "";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;

            var tradeProcessor = new TradeProcessor();

            // Act
            var result = tradeProcessor.ReadTradeData(stream);

            // Assert
            Assert.AreEqual(-123, result.Count());
        }

        


        [TestMethod()]
        public void ProcessTradesTest()
        {
            //Assert.Fail();
        }
    }
}