namespace DatabaseApps.Client.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Models;
    using OfficeOpenXml;

    public static class ExcelExporter
    {
        public static void ExportToExcel(IEnumerable<MySqlExportData> mySqlData, IDictionary<string, double?> sqliteData)
        {
            var data = GatherReportData(mySqlData, sqliteData);
            ExportDataToExcel(data);
        }

        private static IEnumerable<ExcelReportData> GatherReportData(IEnumerable<MySqlExportData> mySqlData,
            IDictionary<string, double?> sqliteData)
        {
            var excelReportData = new List<ExcelReportData>();

            foreach (var vendor in mySqlData)
            {
                var totalTaxes = 0m;
                var totalIncome = 0m;
                vendor.Products.ToList().ForEach(p =>
                {
                    var tax = 0m;
                    if (sqliteData.ContainsKey(p.Key))
                    {
                        tax = (decimal)sqliteData[p.Key];
                    }

                    totalTaxes += ( p.Value == null ? 0 : (decimal)p.Value ) * tax / 100;
                    totalIncome += p.Value == null ? 0 : (decimal)p.Value;
                });

                var financialResult = totalIncome - totalTaxes - vendor.Expenses;

                var data = new ExcelReportData()
                {
                    FinancialResult = financialResult,
                    TotalExpense = vendor.Expenses,
                    TotalIncome = totalIncome,
                    TotalTaxes = totalTaxes,
                    VendorName = vendor.VendorName
                };

                excelReportData.Add(data);
            }

            Console.WriteLine("Excel report issued.");

            return excelReportData;
        }

        /// <summary>
        /// Exports to Excel aggregated data as ExcelReportData.
        /// </summary>
        /// <param name="data">Selected data as ExcelReportData.</param>
        private static void ExportDataToExcel(IEnumerable<ExcelReportData> data)
        {
            using (var excel = new ExcelPackage())
            {
                const string FileLocation = @"../../Output-Files/Report.xlsx";

                // Meta info
                excel.Workbook.Properties.Author = "Shady Computer";
                excel.Workbook.Properties.Title = "Shady Report";
                excel.Workbook.Properties.Company = "Shady Systems";

                excel.Workbook.Worksheets.Add("Data Report");

                var worksheet = excel.Workbook.Worksheets[1];
                worksheet.Name = "Data Report";

                // Table Indexes
                const int VendorIndex = 1;
                const int IncomeIndex = 2;
                const int ExpensesIndex = 3;
                const int TasexIndex = 4;
                const int ResultIndex = 5;

                // Headers
                var vendor = worksheet.Cells[1, VendorIndex];
                var income = worksheet.Cells[1, IncomeIndex];
                var expenses = worksheet.Cells[1, ExpensesIndex];
                var taxes = worksheet.Cells[1, TasexIndex];
                var result = worksheet.Cells[1, ResultIndex];

                vendor.Value = "Vendor";
                income.Value = "Incomes";
                expenses.Value = "Expenses";
                taxes.Value = "Total Taxes";
                result.Value = "Financial Result";

                // Rest of the rows
                var currentRowIndex = 2;

                foreach (var reportData in data)
                {
                    var vendorCell = worksheet.Cells[currentRowIndex, VendorIndex];
                    vendorCell.Value = reportData.VendorName;
                    var incomeCell = worksheet.Cells[currentRowIndex, IncomeIndex];
                    incomeCell.Value = reportData.TotalIncome;
                    var expensesCell = worksheet.Cells[currentRowIndex, ExpensesIndex];
                    expensesCell.Value = reportData.TotalExpense;
                    var taxesCell = worksheet.Cells[currentRowIndex, TasexIndex];
                    taxesCell.Value = reportData.TotalTaxes;
                    var resultCell = worksheet.Cells[currentRowIndex, ResultIndex];
                    resultCell.Value = reportData.FinancialResult;

                    currentRowIndex++;
                }

                // Save the Excel file
                Byte[] bin = excel.GetAsByteArray();
                File.WriteAllBytes(FileLocation, bin);

                Console.WriteLine("Excel report done in \"{0}\"!", FileLocation);
            }
        }
    }
}
