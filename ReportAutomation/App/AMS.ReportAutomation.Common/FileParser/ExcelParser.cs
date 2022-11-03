using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AMS.ReportAutomation.Common.FileParser
{
    public class ExcelParser : IParser
    {
        private CultureInfo _cultureInfo;
        private readonly Regex _regxNumber = new Regex(@"[\*#\?E]|^\d*.?\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        List<List<CellModel>> IParser.Parse(string filePath, string culture, int sheetAt, string delimiter)
        {
            culture = string.IsNullOrWhiteSpace(culture) ? "en" : culture;

            DetectFileFormat(culture);

            ISheet sheet = OpenFile(filePath, sheetAt);

            if (sheet == null || sheet.LastRowNum < 0)
            {
                throw new ArgumentException("File format error.");
            }

            // read values
            var values = new List<List<CellModel>>();
            for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                var cells = new List<CellModel>();

                if (sheet.GetRow(rowIndex) == null)
                {
                    continue;
                }

                for (int cellIndex = 0; cellIndex < sheet.GetRow(rowIndex).LastCellNum; cellIndex++)
                {
                    var cell = sheet.GetRow(rowIndex).GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);

                    var cellModel = new CellModel();

                    if (cell != null)
                    {
                        CellType type;

                        cellModel.ColIndex = cell.ColumnIndex;
                        cellModel.Value = ParseValue(cell, out type);
                        cellModel.Type = type;
                        cellModel.PropName = string.Empty;
                    }
                    else
                    {
                        cellModel.ColIndex = cellIndex;
                        cellModel.Value = null;
                        cellModel.Type = CellType.String;
                        cellModel.PropName = string.Empty;
                    }

                    cells.Add(cellModel);
                }

                values.Add(cells);
            }

            return values;
        }

        List<List<string>> IParser.GetPreview(string filePath, int totalRow, int sheetAt, string delimiter)
        {
            ISheet sheet = OpenFile(filePath, sheetAt);

            if (sheet == null)
            {
                return new List<List<string>>();
            }

            var ret = new List<List<string>>();
            int lastRowNum = sheet.LastRowNum >= totalRow ? totalRow - 1 : sheet.LastRowNum;

            for (int i = 0; i <= lastRowNum; i++)
            {
                var lstCell = new List<string>();

                if (sheet.GetRow(i) == null)
                {
                    continue;
                }

                for (int j = 0; j < sheet.GetRow(i).LastCellNum; j++)
                {
                    var cell = sheet.GetRow(i).GetCell(j, MissingCellPolicy.RETURN_NULL_AND_BLANK);

                    if (cell != null && !cell.IsMergedCell && !cell.IsPartOfArrayFormulaGroup)
                    {
                        lstCell.Add(CellToString(cell));
                    }
                    else
                    {
                        lstCell.Add(string.Empty);
                    }
                }

                ret.Add(lstCell);
            }

            return ret;
        }

        List<CellHeader> IParser.GetHeader(string filePath, int sheetAt, string delimiter)
        {
            ISheet sheet = OpenFile(filePath, sheetAt);

            if (sheet == null)
            {
                return new List<CellHeader>();
            }

            var headers = new List<CellHeader>();

            if (sheet.GetRow(0) == null)
            {
                return new List<CellHeader>();
            }

            // read header
            for (int cellIndex = 0; cellIndex < sheet.GetRow(0).LastCellNum; cellIndex++)
            {
                var cell = sheet.GetRow(0).GetCell(cellIndex, MissingCellPolicy.RETURN_BLANK_AS_NULL);

                if (cell != null && !cell.IsMergedCell && !cell.IsPartOfArrayFormulaGroup)
                {
                    headers.Add(new CellHeader { Name = CellToString(cell), ColIndex = cell.ColumnIndex });
                }
            }

            return headers;
        }

        int IParser.GetTotalColumn(string filePath, int sheetAt, string delimiter)
        {
            ISheet sheet = OpenFile(filePath, sheetAt);

            if (sheet == null)
            {
                return 0;
            }

            // TODO: this maybe slow, but only the way to define how many columns in the excel file???
            int totalColumn = 0;

            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                if (sheet.GetRow(i) != null && sheet.GetRow(i).LastCellNum > totalColumn)
                {
                    totalColumn = sheet.GetRow(i).LastCellNum;
                }
            }

            return totalColumn;
        }

        int IParser.GetTotalRow(string filePath, int sheetAt, string delimiter)
        {
            ISheet sheet = OpenFile(filePath, sheetAt);

            if (sheet == null)
            {
                return 0;
            }

            return sheet.LastRowNum + 1;
        }

        void IParser.WriteDemo(string[] properties, Stream outputStream, int type, string delimiter)
        {
            IWorkbook workbook;

            if (type == 1)
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }

            ISheet sheet = workbook.CreateSheet();
            IRow row = sheet.CreateRow(0);

            for (int i = 0; i < properties.Length; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                cell.SetCellValue(properties[i]);
            }

            workbook.Write(outputStream);
        }

        private ISheet OpenFile(string filePath, int sheetAt)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File " + filePath + " is not exists.");
            }

            ISheet sheet;

            string extension = Path.GetExtension(filePath);
            sheetAt = sheetAt < 0 ? 0 : sheetAt;

            if (string.IsNullOrWhiteSpace(extension) || extension.ToLower() == ".xls")
            {
                HSSFWorkbook hssfWorkbook;

                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfWorkbook = new HSSFWorkbook(file);
                }

                sheet = hssfWorkbook.GetSheetAt(sheetAt);
            }
            else
            {
                XSSFWorkbook xssfWorkbook;

                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    xssfWorkbook = new XSSFWorkbook(file);
                }

                sheet = xssfWorkbook.GetSheetAt(sheetAt);
            }

            return sheet;
        }

        private void DetectFileFormat(string culture)
        {
            try
            {
                _cultureInfo = new CultureInfo(culture);
            }
            catch (Exception)
            {
                _cultureInfo = CultureInfo.InvariantCulture;
            }
        }

        private string CellToString(ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }

            string cellValue;

            switch (cell.CellType)
            {
                case NPOI.SS.UserModel.CellType.Numeric:
                    int format = cell.CellStyle.DataFormat;

                    if (format < 14 || (format >= 37 && format <= 44) || (format == 48 || format == 49)) // builtin dataformat Number detect
                    {
                        cellValue = cell.NumericCellValue.ToString(_cultureInfo);
                    }
                    else if ((format >= 14 && format <= 22) || (format >= 45 && format <= 47)) // builtin dataformat Datetime detect
                    {
                        cellValue = cell.DateCellValue.ToString(_cultureInfo);
                    }
                    else
                    {
                        string formatStr = cell.CellStyle.GetDataFormatString();

                        cellValue = _regxNumber.IsMatch(formatStr)
                            ? cell.NumericCellValue.ToString(_cultureInfo) : cell.DateCellValue.ToString(_cultureInfo);
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Boolean:
                    cellValue = cell.BooleanCellValue.ToString(_cultureInfo);
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    cellValue = Utils.FilterHtmlTag(cell.StringCellValue);
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                    try
                    {
                        cell.SetCellType(cell.CachedFormulaResultType);
                        cellValue = CellToString(cell);
                    }
                    catch (Exception)
                    {
                        cellValue = string.Empty;
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Error:
                case NPOI.SS.UserModel.CellType.Blank:
                case NPOI.SS.UserModel.CellType.Unknown:
                default:
                    cellValue = string.Empty;
                    break;
            }

            return cellValue;
        }

        private object ParseValue(ICell cell, out CellType type)
        {
            var cellType = cell.CellType;

            if (cellType == NPOI.SS.UserModel.CellType.Blank || cellType == NPOI.SS.UserModel.CellType.Unknown || cellType == NPOI.SS.UserModel.CellType.Error)
            {
                type = CellType.String;
                return string.Empty;
            }

            string input;

            if (cellType == NPOI.SS.UserModel.CellType.Numeric)
            {
                int format = cell.CellStyle.DataFormat;

                // All of these data format is number data format like: 1,000 ; $1,000; 100%, ...
                // Data format combine of Builtin data format (from extension core), and Fixed data format from: Excel 2010
                if (format < 14 || (format >= 37 && format <= 44) || (format == 48 || format == 49))
                {
                    return GetInt(cell.NumericCellValue, out type);
                }

                // All of these data format is date time data format like: m/d/y ; m/d/yyyy, ....
                // Data format combine of Builtin data format (from extension core), and Fixed data format from: Excel 2010
                if ((format >= 14 && format <= 22) || (format >= 45 && format <= 47))
                {
                    DateTime retDateTime = cell.DateCellValue;

                    type = CellType.DateTime;
                    return retDateTime;
                }

                string formatStr = cell.CellStyle.GetDataFormatString();

                if (_regxNumber.IsMatch(formatStr))
                {
                    return GetInt(cell.NumericCellValue, out type);
                }

                type = CellType.DateTime;
                return cell.DateCellValue;
            }

            if (cellType == NPOI.SS.UserModel.CellType.Boolean)
            {
                type = CellType.Bool;

                return cell.BooleanCellValue;
            }

            if (cellType == NPOI.SS.UserModel.CellType.Formula)
            {
                // Try to parse formula
                try
                {
                    cell.SetCellType(cell.CachedFormulaResultType);
                    return ParseValue(cell, out type);
                }
                catch (Exception)
                {
                    type = CellType.String;
                    return string.Empty;
                }
            }

            input = Utils.FilterHtmlTag(cell.StringCellValue);

            type = CellType.String;

            return input;
        }

        private object GetInt(double input, out CellType type)
        {
            // Detect double number trick: remainder asssignment for 1 and see if it have floating point
            if (((decimal)input%1) != 0)
            {
                type = CellType.Double;

                return input;
            }

            if (input > Int32.MaxValue || input < Int32.MinValue)
            {
                type = CellType.Long;
                return (long) input;
            }

            type = CellType.Int;

            return (int) input;
        }
    }
}
