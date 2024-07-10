using System;
using System.IO;
using OfficeOpenXml;

class Program
{
    static void Main()
    {
        // 定义节点数量
        int numFan = 100;
        int numBrand = 9;
        int numAnchor = 3;
        int numGovernment = 1;
        int numPlatform = 1;

        // 创建节点名称数组
        string[] nodeNames = new string[numFan + numBrand + numGovernment + numPlatform + numAnchor];
        for (int i = 0; i < numFan; i++)
        {
            nodeNames[i] = "FAN" + (i + 1);
        }
        for (int i = numFan; i < numFan + numBrand; i++)
        {
            nodeNames[i] = "BRAND" + (i - numFan + 1);
        }
        nodeNames[numFan + numBrand] = "GOVERNMENT";
        nodeNames[numFan + numBrand + numGovernment] = "PLATFORM";
        for (int i = numFan + numBrand + numGovernment; i < numFan + numBrand + numGovernment + numAnchor; i++)
        {
            nodeNames[i] = "ANCHOR" + (i - numFan - numBrand - numGovernment + 1);
        }

        // 创建矩阵
        int[,] matrix = new int[numFan + numBrand + numGovernment + numPlatform + numAnchor, numFan + numBrand + numGovernment + numPlatform + numAnchor];

        // 设置 FAN 与 ANCHOR 的关系
        for (int i = 0; i < numFan; i++)
        {
            // FAN 与 PLATFORM 的关系
            matrix[i, numFan + numBrand + numGovernment] = 1;
            // 随机选择一个 ANCHOR 与之建立关系
            Random rnd = new Random();
            int anchorIndex = rnd.Next(numFan + numBrand + numGovernment + numPlatform, numFan + numBrand + numGovernment + numPlatform + numAnchor);
            matrix[i, anchorIndex] = 1; // FAN 与 ANCHOR 的关系
        }

        // 设置 BRAND 与 ANCHOR 的关系
        for (int i = numFan; i < numFan + numBrand; i++)
        {
            Random rnd = new Random();
            int anchorIndex = rnd.Next(numFan + numBrand + numGovernment + numPlatform, numFan + numBrand + numGovernment + numPlatform + numAnchor);
            matrix[i, anchorIndex] = 1; // BRAND 与 ANCHOR 的关系
            matrix[i, numFan + numBrand + numGovernment] = 1; // BRAND 与 PLATFORM 的关系
        }

        // 设置 PLATFORM 与 GOVERNMENT 的关系
        matrix[numFan + numBrand, numFan + numBrand] = 1; // PLATFORM 与 GOVERNMENT 的关系
        // 设置 PLATFORM 与 BRAND 的关系
        for (int i = numFan; i < numFan + numBrand; i++)
        {
            matrix[numFan + numBrand, i] = 1;
        }
        // 设置 ANCHOR 与 PLATFORM 的关系
        for (int i = numFan + numBrand; i < numFan + numBrand + numAnchor; i++)
        {
            matrix[i, numFan + numBrand + numGovernment] = 1;
        }

        // 设置 PLATFORM 与其他节点的关系
        for (int i = 0; i < numFan + numBrand + numGovernment + numPlatform + numAnchor; i++)
        {
            if (i != numFan + numBrand + numGovernment)
            {
                matrix[numFan + numBrand + numGovernment, i] = 1;
                matrix[i, numFan + numBrand + numGovernment] = 1;
            }
        }

        // 将矩阵保存为 Excel 表格
        using (ExcelPackage excel = new ExcelPackage())
        {
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("RelationshipMatrix");

            // 设置节点名称行
            for (int i = 0; i < nodeNames.Length; i++)
            {
                worksheet.Cells[1, i + 2].Value = nodeNames[i];
            }
            // 设置节点名称列
            for (int i = 0; i < nodeNames.Length; i++)
            {
                worksheet.Cells[i + 2, 1].Value = nodeNames[i];
            }

            // 填充关系矩阵
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    worksheet.Cells[i + 2, j + 2].Value = matrix[i, j];
                }
            }

            // 保存 Excel 表格
            FileInfo excelFile = new FileInfo(@"C:\Users\HP\Desktop\炼丹炉\ModifiedRelationshipMatrix4.xlsx");
            excel.SaveAs(excelFile);
        }

        Console.WriteLine("Excel 表格已生成并保存为 ModifiedRelationshipMatrix4.xlsx");
    }
}
