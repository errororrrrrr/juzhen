using System;
using OfficeOpenXml;

class Program
{
    static void Main(string[] args)
    {
        // 创建关系矩阵
        int[,] relationsMatrix = GenerateRelationsMatrix();

        // 创建一个新的Excel包
        using (var package = new ExcelPackage())
        {
            // 添加一个工作表
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Relations");

            // 添加行列名
            for (int i = 0; i < relationsMatrix.GetLength(0); i++)
            {
                worksheet.Cells[i + 2, 1].Value = GetRoleName(i);
                worksheet.Cells[1, i + 2].Value = GetRoleName(i);
            }

            // 将关系矩阵写入Excel工作表
            for (int i = 0; i < relationsMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < relationsMatrix.GetLength(1); j++)
                {
                    worksheet.Cells[i + 2, j + 2].Value = relationsMatrix[i, j];
                }
            }

            // 保存Excel文件
            package.SaveAs(new System.IO.FileInfo(@"E:\output1.xlsx"));
        }

        Console.WriteLine("Excel文件已生成！");
    }

    static int[,] GenerateRelationsMatrix()
    {
        // 角色数量
        int numAnchors = 5;
        int numFans = 200;
        int numMCNs = 2;
        int numSelfBrands = 3;
        int numBrands = 10;
        int numGovernment = 1;
        int numPlatforms = 1;

        // 创建关系矩阵
        int[,] relationsMatrix = new int[numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment+numPlatforms, numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment+numPlatforms];

        // 初始化关系矩阵，根据您的描述，我们可以做出一些假设
        // 在这里，我随机生成关系，1表示有关系，0表示无关系
        Random random = new Random();
        for (int i = 0; i < relationsMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < relationsMatrix.GetLength(1); j++)
            {
                // 主播与粉丝之间的关系
                if (i < numAnchors && j >= numAnchors && j < numAnchors + numFans)
                {
                    relationsMatrix[i, j] = random.Next(2);
                }
                // 主播与直播平台之间的关系
                else if (i < numAnchors && j == numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment)
                {
                    relationsMatrix[i, j] = 1; // 所有的主播都要在直播平台上直播
                }
                // 品牌商与主播之间的关系
                else if (i >= numAnchors + numFans + numMCNs + numSelfBrands && i < numAnchors + numFans + numMCNs + numSelfBrands + numBrands && j < numAnchors)
                {
                    // 确保品牌商与最多两个主播有关系
                    int brandIndex = i - (numAnchors + numFans + numMCNs + numSelfBrands); // 获取品牌商索引
                    int numRelations = 0; // 记录品牌商已经与多少个主播产生了关系
                    for (int k = 0; k < numAnchors; k++)
                    {
                        // 检查当前主播是否已经有两个品牌商与之有关系
                        if (relationsMatrix[i, k] == 1)
                        {
                            numRelations++;
                        }
                    }
                    if (numRelations < 2)
                    {
                        relationsMatrix[i, j] = 1; // 设置品牌商与主播之间的关系为1
                    }
                    else
                    {
                        relationsMatrix[i, j] = 0; // 已经有两个品牌商与该主播有关系，设置为0
                    }
                }
                // 品牌商与政府之间的关系
                else if (i >= numAnchors + numFans + numMCNs + numSelfBrands + numBrands && i < numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment && j >= numAnchors + numFans + numMCNs + numSelfBrands && j < numAnchors + numFans + numMCNs + numSelfBrands + numBrands)
                {
                    relationsMatrix[i, j] = 1; // 品牌商与政府之间有关系
                }
                // 独立品牌主播与平台之间的关系
                else if (i >= numAnchors && i < numAnchors + numMCNs && j == numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment)
                {
                    relationsMatrix[i, j] = 1; // 独立品牌主播与平台有关系
                }
                // 独立品牌主播与政府之间的关系
                else if (i >= numAnchors && i < numAnchors + numMCNs && j >= numAnchors + numFans + numMCNs + numSelfBrands + numBrands && j < numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment)
                {
                    relationsMatrix[i, j] = 1; // 独立品牌主播与政府有关系
                }
                // 平台与政府之间的关系
                else if (i == numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment && j == numAnchors + numFans + numMCNs + numSelfBrands + numBrands + numGovernment)
                {
                    relationsMatrix[i, j] = 1; // 平台与政府之间有关系
                }
                // MCN1与Anchor之间的关系
                else if (i == numAnchors + numFans && j >= numAnchors && j < numAnchors + numFans && relationsMatrix[i, j] == 0)
                {
                    int anchorIndex = j; // 获取Anchor索引
                    int numRelations = 0; // 记录该MCN已经与多少个Anchor产生了关系
                    for (int k = 0; k < numAnchors; k++)
                    {
                        // 检查当前主播是否已经有两个品牌商与之有关系
                        if (relationsMatrix[i, k] == 1)
                        {
                            numRelations++;
                        }
                    }
                    if (numRelations < 3) // 确保每个MCN与5-6个Anchor有关系
                    {
                        relationsMatrix[i, j] = 1; // 设置MCN与Anchor之间的关系为1
                    }
                }
                // MCN2与Anchor之间的关系
                else if (i == numAnchors + numFans + 1 && j >= numAnchors && j < numAnchors + numFans && relationsMatrix[i, j] == 0)
                {
                    int anchorIndex = j; // 获取Anchor索引
                    int numRelations = 0; // 记录该MCN已经与多少个Anchor产生了关系
                    for (int k = 0; k < numAnchors; k++)
                    {
                        // 检查当前主播是否已经有两个品牌商与之有关系
                        if (relationsMatrix[i, k] == 1)
                        {
                            numRelations++;
                        }
                    }
                    if (numRelations < 3) // 确保每个MCN与5-6个Anchor有关系
                    {
                        relationsMatrix[i, j] = 1; // 设置MCN与Anchor之间的关系为1
                    }
                }
                // Fan与SelfBrand之间的关系
                else if (i >= numAnchors + numFans + numMCNs + numSelfBrands && i < numAnchors + numFans + numMCNs + numSelfBrands + numBrands && j >= numAnchors && j < numAnchors + numFans)
                {
                    relationsMatrix[i, j] = random.Next(2); // 随机生成Fan与SelfBrand之间的关系
                }
            }
        }

        return relationsMatrix;
    }

    static string GetRoleName(int index)
    {
        // 根据角色索引返回角色名
        string roleName = "";
        if (index < 5)
        {
            roleName = "Anchor" + (index + 1);
        }
        else if (index < 205)
        {
            roleName = "Fan" + (index - 4);
        }
        else if (index < 207)
        {
            roleName = "MCN" + (index - 204);
        }
        else if (index < 210)
        {
            roleName = "SELFBRAND" + (index - 206);
        }
        else if (index < 220)
        {
            roleName = "BRAND" + (index - 209);
        }
        else if (index == 220)
        {
            roleName = "Platform";
        }
        else
        {
            roleName = "Government";
        }
        return roleName;
    }
}
