using System;
using System.IO;
using OfficeOpenXml;

namespace RelationshipMatrixGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize Excel package
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Add a worksheet to the workbook
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Relationship Matrix");

                // Define the entities and their counts
                int fanCount = 200;
                int brandCount = 10;
                int selfBrandCount = 3;
                int governmentCount = 1;
                int mcnCount = 2;
                int platformCount = 1;
                int anchorCount = 5;

                // Add entity names to the first row and column
                worksheet.Cells[1, 1].Value = "FAN";
                worksheet.Cells[1, 2].Value = "BRAND";
                worksheet.Cells[1, 3].Value = "SELFBRAND";
                worksheet.Cells[1, 4].Value = "GOVERNMENT";
                worksheet.Cells[1, 5].Value = "MCN";
                worksheet.Cells[1, 6].Value = "PLATFORM";
                worksheet.Cells[1, 7].Value = "ANCHOR";

                // Add FAN names
                for (int i = 1; i <= fanCount; i++)
                {
                    worksheet.Cells[i + 1, 1].Value = "FAN" + i;
                }

                // Add BRAND names
                for (int i = 1; i <= brandCount; i++)
                {
                    worksheet.Cells[i + 1, 2].Value = "BRAND" + i;
                }

                // Add SELFBRAND names
                for (int i = 1; i <= selfBrandCount; i++)
                {
                    worksheet.Cells[i + 1, 3].Value = "SELFBRAND" + i;
                }

                // Add GOVERNMENT name
                worksheet.Cells[2, 4].Value = "GOVERNMENT";

                // Add MCN names
                for (int i = 1; i <= mcnCount; i++)
                {
                    worksheet.Cells[i + 1, 5].Value = "MCN" + i;
                }

                // Add PLATFORM name
                worksheet.Cells[2, 6].Value = "PLATFORM";

                // Add ANCHOR names
                for (int i = 1; i <= anchorCount; i++)
                {
                    worksheet.Cells[i + 1, 7].Value = "ANCHOR" + i;
                }

                // Establish relationships according to the given rules

                // FAN to ANCHOR and SELFBRAND
                Random rand = new Random();
                for (int i = 2; i <= fanCount + 1; i++)
                {
                    int anchorIndex = rand.Next(1, anchorCount + 1);
                    int selfBrandIndex = rand.Next(1, selfBrandCount + 1);
                    worksheet.Cells[i, 7 + anchorIndex].Value = 1; // Relationship with ANCHOR
                    worksheet.Cells[i, 3 + selfBrandIndex].Value = 1; // Relationship with SELFBRAND
                }

                // Assign ANCHOR to MCN
                int[] anchorMCN = new int[anchorCount];
                for (int i = 0; i < anchorCount; i++)
                {
                    anchorMCN[i] = (i < mcnCount) ? i + 1 : 0;
                }
                for (int i = 2; i <= anchorCount + 1; i++)
                {
                    worksheet.Cells[i, 5].Value = anchorMCN[i - 2];
                }

                // BRAND to ANCHOR
                for (int i = 2; i <= brandCount + 1; i++)
                {
                    int anchorIndex = rand.Next(1, anchorCount + 1);
                    worksheet.Cells[i, 7 + anchorIndex].Value = 1; // Relationship with ANCHOR
                }

                // Establish relationships with PLATFORM
                for (int i = 2; i <= fanCount + 1; i++)
                {
                    worksheet.Cells[i, 6].Value = 1; // Relationship with PLATFORM
                }
                for (int i = 2; i <= brandCount + 1; i++)
                {
                    worksheet.Cells[i, 6].Value = 1; // Relationship with PLATFORM
                }
                for (int i = 2; i <= selfBrandCount + 1; i++)
                {
                    worksheet.Cells[i, 6].Value = 1; // Relationship with PLATFORM
                }
                worksheet.Cells[2, 6].Value = 1; // Relationship with PLATFORM (GOVERNMENT)

                // Establish relationships with GOVERNMENT
                for (int i = 2; i <= platformCount + 1; i++)
                {
                    worksheet.Cells[i, 4].Value = 1; // Relationship with GOVERNMENT
                }
                for (int i = 2; i <= brandCount + 1; i++)
                {
                    worksheet.Cells[i, 4].Value = 1; // Relationship with GOVERNMENT
                }
                for (int i = 2; i <= selfBrandCount + 1; i++)
                {
                    worksheet.Cells[i, 4].Value = 1; // Relationship with GOVERNMENT
                }

                // Fill in the empty cells with 0
                for (int i = 2; i <= fanCount + 1; i++)
                {
                    for (int j = 2; j <= 7; j++)
                    {
                        if (worksheet.Cells[i, j].Value == null)
                            worksheet.Cells[i, j].Value = 0;
                    }
                }

                // Save the Excel file
                FileInfo excelFile = new FileInfo("E:3.xlsx");
                excelPackage.SaveAs(excelFile);
            }

            Console.WriteLine("Excel file generated successfully.");
        }
    }
}
