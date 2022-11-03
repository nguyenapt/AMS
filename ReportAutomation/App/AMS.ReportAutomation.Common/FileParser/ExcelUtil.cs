using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace AMS.ReportAutomation.Common.FileParser
{
    public static class ExcelUtil
    {
        public static Dictionary<int, string> GetExcelSheets(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File " + filePath + " is not exists.");
            }

            var lstSheet = new Dictionary<int, string>();

            string extension = Path.GetExtension(filePath);

            if (string.IsNullOrWhiteSpace(extension) || extension.ToLower() == ".xls")
            {
                HSSFWorkbook hssfWorkbook;

                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfWorkbook = new HSSFWorkbook(file);
                }

                for (int i = 0; i < hssfWorkbook.NumberOfSheets; i++)
                {
                    lstSheet.Add(i, hssfWorkbook.GetSheetAt(i).SheetName);
                }
            }
            else
            {
                XSSFWorkbook xssfWorkbook;

                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    xssfWorkbook = new XSSFWorkbook(file);
                }

                for (int i = 0; i < xssfWorkbook.NumberOfSheets; i++)
                {
                    lstSheet.Add(i, xssfWorkbook.GetSheetAt(i).SheetName);
                }
            }

            return lstSheet;
        }
    }
}
