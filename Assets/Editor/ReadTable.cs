using UnityEngine;
using UnityEditor;
using OfficeOpenXml;
using System.IO;
using System;
using System.Reflection;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        string path = Application.dataPath + "/Editor/关卡管理.xlsx";
        string assetName = "Level";
        FileInfo fileInfo = new FileInfo(path);
        //创建序列化类
        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));
        //打开Excel文件，using会在使用完毕后自动关闭读取的文件
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            //表格内的具体表单：此处为僵尸
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["僵尸"];
            //遍历每一行
            for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
            {
                LevelItem levelItem = new LevelItem();
                //获取LevelItem的type
                Type type = typeof(LevelItem);
                //遍历每一列
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    //用反射的方式对levelItem进行赋值，work.GetValue(2,j)对应表格2行j列的内容
                    FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
                }
                //当前行赋值结束后，添加到列表中
                levelData.LevelDataList.Add(levelItem);
            }
        }
            AssetDatabase.CreateAsset(levelData, "Assets/Resources/" + assetName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        
    }
    // static int getInt(string input)
    // {
    //     string value = input.Trim();
    //     int num = -1;
    //     int.TryParse(value, out num);
    //     return num;
    // }

}
