using System.Collections.Generic;
using System.IO;


namespace AMS.ReportAutomation.Common.FileParser
{
    public interface IParser
    {
        List<List<CellModel>> Parse(string filePath, string culture, int sheetAt = 0, string delimiter = "");

        List<List<string>> GetPreview(string filePath, int totalRow = 8, int sheetAt = 0, string delimiter = "");

        List<CellHeader> GetHeader(string filePath, int sheetAt = 0, string delimiter = "");

        int GetTotalColumn(string filePath, int sheetAt = 0, string delimiter = "");

        int GetTotalRow(string filePath, int sheetAt = 0, string delimiter = "");

        void WriteDemo(string[] properties, Stream outputStream, int type = 0, string delimiter = "");
    }
}
