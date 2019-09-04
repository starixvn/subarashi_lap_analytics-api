using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Data;
using System.Text;

namespace Demo_SSAS
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string connectionString = "provider=msolap; Data Source=CP002-HOWARD\\MSSQLSERVER2017;Catalog=AdventureWorkSales;";
            AdomdConnection conn = new AdomdConnection(connectionString);
            conn.Open();
            string commandText = @"Select	[Measures].[InternetTotalSales] on Columns,
		        Non Empty{[DimDate].[Calendar].[CalendarYear]} on Rows
                From [AdventureWorkSalesModel]";

            AdomdCommand cmd = new AdomdCommand(commandText, conn);
            AdomdDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var result = new StringBuilder();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    
                    result.Append(Convert.ToString(dr[i]) + "  ");
                }
                Console.WriteLine(result.ToString());
            }

            //use datatable
            //conn.Close();
            //conn.Open();

            //AdomdDataAdapter currentDataAdapter = new AdomdDataAdapter(commandText, conn);
            //DataTable tabularResults = new DataTable();
            //currentDataAdapter.Fill(tabularResults);
            //foreach (DataRow row in tabularResults.Rows)
            //{
            //    string description = row["[DimProduct].[Category].[Category].[MEMBER_CAPTION]"].ToString();
            //    string icoFileName = row["[Measures].[InternetTotalMargin]"].ToString();
            //    string installScript = row["[Measures].[InternetTotalSales]"].ToString();
            //}


            dr.Close();
            conn.Close();
        }

        private static DataTable tabularQueryExecute(string qry, AdomdConnection cnx)
        {
            AdomdDataAdapter currentDataAdapter = new AdomdDataAdapter(qry, cnx);
            DataTable tabularResults = new DataTable();
            currentDataAdapter.Fill(tabularResults);
            return tabularResults;
        }
    }
}