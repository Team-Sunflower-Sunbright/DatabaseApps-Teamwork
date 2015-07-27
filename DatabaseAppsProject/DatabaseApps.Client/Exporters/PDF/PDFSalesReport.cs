using DatabaseApps.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseApps.Client.Exporters.PDF
{
    public class PDFSalesReport
    {
        private static PdfPTable GetHeader(string title)
        {
            var header = new PdfPTable(1);
            header.WidthPercentage = 100f;
            Font titleFont = FontFactory.GetFont("Arial", 15, Font.BOLD);
            Paragraph Title = new Paragraph(title, titleFont);
            var titleCell = new PdfPCell(Title);
            titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
            titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            titleCell.PaddingTop = 5;
            titleCell.PaddingBottom = 7;
            header.AddCell(titleCell);

            return header;
        }

        private static PdfPTable CreateReportTable(IGrouping<GroupingByDate, Income> sales)
        {
            var dataTable = new PdfPTable(new float[] { 2, 1, 1.5f, 3.5f, 1 });
            dataTable.WidthPercentage = 100f;
            Font dateFont = FontFactory.GetFont("Arial", 11);
            var date = sales.Key.Year + "-" + sales.Key.Month + "-" + sales.Key.Day;
            var dateParagraph = new Paragraph("Date: " + date, dateFont);
            var dateCell = new PdfPCell(dateParagraph);
            dateCell.Padding = 5;
            dateCell.Colspan = 5;
            dateCell.BackgroundColor = new BaseColor(243, 243, 243);
            dataTable.AddCell(dateCell);

            var headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var productCell = new PdfPCell(new Paragraph("Product", headerFont));
            var quantityCell = new PdfPCell(new Paragraph("Quantity", headerFont));
            var unitPriceCell = new PdfPCell(new Paragraph("Unit Price", headerFont));
            var locationCell = new PdfPCell(new Paragraph("Location", headerFont));
            var sumCell = new PdfPCell(new Paragraph("Sum", headerFont));
            var headerCells = new PdfPCell[] { productCell, quantityCell, unitPriceCell, locationCell, sumCell };
            foreach (var cell in headerCells)
            {
                cell.BackgroundColor = new BaseColor(200, 200, 200);
                cell.Padding = 5;
            }
            var headerRow = new PdfPRow(headerCells);
            dataTable.Rows.Add(headerRow);

            var dataFont = FontFactory.GetFont("Arial", 10);
            decimal total = 0;
            foreach (var sale in sales)
            {
                var productDataCell = new PdfPCell(new Paragraph(sale.Product.Name, dataFont));
                var quantityDataCell = new PdfPCell(new Paragraph(sale.Quantity.ToString() + " " + sale.Product.Measure.Name, dataFont));
                var unitPriceDataCell = new PdfPCell(new Paragraph(sale.Product.BuyingPrice.ToString(), dataFont));
                var locationDataCell = new PdfPCell(new Paragraph(sale.Product.Vendor.Name, dataFont));
                var sum = ((decimal)sale.Quantity * sale.Product.BuyingPrice);
                var sumDataCell = new PdfPCell(new Paragraph(sum.ToString(), dataFont));
                var dataCells = new PdfPCell[] { productDataCell, quantityDataCell, unitPriceDataCell, locationDataCell, sumDataCell };

                foreach (var cell in dataCells)
                {
                    cell.Padding = 5;
                }
                var dataRow = new PdfPRow(dataCells);
                dataTable.Rows.Add(dataRow);
                total += sum;
            }

            Font totalSumFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            var footerTotal = new PdfPCell(new Paragraph("Total sum for " + date + ":", dataFont));
            footerTotal.Colspan = 4;
            footerTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
            footerTotal.Padding = 5;
            dataTable.AddCell(footerTotal);
            var footerSum = new PdfPCell(new Paragraph(total.ToString(), totalSumFont));
            footerSum.Padding = 5;
            dataTable.AddCell(footerSum);

            return dataTable;
        }

        public static void CreateReport(string fileName, IQueryable<IGrouping<GroupingByDate, Income>> rowsByDate)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            var header = PDFSalesReport.GetHeader("Aggregated Sales Report");
            doc.Add(header);
            decimal grandTotal = 0;

            foreach (var row in rowsByDate)
            {
                var reportTable = PDFSalesReport.CreateReportTable(row);
                grandTotal += row.Sum(a => a.Product.BuyingPrice * (decimal)a.Quantity);
                doc.Add(reportTable);
            }

            doc.Add(PDFSalesReport.CreateGrandTotalTable(grandTotal));

            doc.Close();
        }


        private static PdfPTable CreateGrandTotalTable(decimal sum)
        {
            var grandTotalTable = new PdfPTable(new float[] { 2, 1, 1.5f, 3.5f, 1 });
            grandTotalTable.WidthPercentage = 100f;
            Font grandTotalTextFont = FontFactory.GetFont("Arial", 11);
            var grandTotalParagraph = new Paragraph("Grand total: ", grandTotalTextFont);
            var grandTotalTextCell = new PdfPCell(grandTotalParagraph);
            grandTotalTextCell.Padding = 5;
            grandTotalTextCell.Colspan = 4;
            var backgroundColor = new BaseColor(88, 186, 209);
            grandTotalTextCell.BackgroundColor = backgroundColor;
            grandTotalTextCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Font grandSumFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            var grandSumParagraph = new Paragraph(sum.ToString(), grandSumFont);
            var grandSumCell = new PdfPCell(grandSumParagraph);
            grandSumCell.BackgroundColor = backgroundColor;
            grandSumCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            grandTotalTable.AddCell(grandTotalTextCell);
            grandTotalTable.AddCell(grandSumCell);

            return grandTotalTable;
        }
    }
}
