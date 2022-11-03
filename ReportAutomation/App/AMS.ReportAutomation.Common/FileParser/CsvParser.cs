//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using CsvHelper;
//using CsvHelper.Configuration;

//namespace AMS.ReportAutomation.Common.FileParser
//{
//    public class CsvParser : IParser
//    {
//        private CultureInfo _cultureInfo;
//        private Regex _regxInt;
//        private Regex _regxDouble;
//        private Regex _regxBool;

//        List<List<CellModel>> IParser.Parse(string filePath, string culture, int sheetAt, string delimiter)
//        {
//            culture = string.IsNullOrWhiteSpace(culture) ? "en" : culture;

//            DetectFileFormat(culture);

//            var lines = OpenFile(filePath, delimiter);

//            // get values line by line
//            return lines.Select(FetchValues).ToList();
//        }

//        List<List<string>> IParser.GetPreview(string filePath, int totalRow, int sheetAt, string delimiter)
//        {
//            return OpenFile(filePath, delimiter).Take(totalRow).ToList();
//        }

//        List<CellHeader> IParser.GetHeader(string filePath, int sheetAt, string delimiter)
//        {
//            var lines = OpenFile(filePath, delimiter);

//            var headers = FetchHeader(lines[0]);

//            return headers;
//        }

//        int IParser.GetTotalColumn(string filePath, int sheetAt, string delimiter)
//        {
//            var lines = OpenFile(filePath, delimiter);

//            return lines.Select(line => line.Count()).Concat(new[] { 0 }).Max();
//        }

//        int IParser.GetTotalRow(string filePath, int sheetAt, string delimiter)
//        {
//            var lines = OpenFile(filePath, delimiter);

//            return lines.Count;
//        }

//        void IParser.WriteDemo(string[] properties, Stream outputStream, int type, string delimiter)
//        {
//            string output = string.Join(",", properties);
//            byte[] bytes = Encoding.UTF8.GetBytes(output);
//            outputStream.Write(bytes, 0, bytes.Length);
//        }

//        private List<List<string>> OpenFile(string filePath, string delimiter)
//        {
//            if (!File.Exists(filePath))
//            {
//                throw new FileNotFoundException("File " + filePath + " is not exists.");
//            }

//            var lines = new List<List<string>>();
//            try
//            {
//                //var csvConfig = new CsvConfiguration
//                //{
//                //    ShouldSkipRecord = record => record.All(string.IsNullOrEmpty),
//                //    Delimiter = string.IsNullOrEmpty(delimiter) ? "," : delimiter,
//                //    HasHeaderRecord = false
//                //};

//                //using (var sr = new StreamReader(filePath, Encoding.Default, true))
//                //{
//                //    var csv = new CsvReader(sr, csvConfig);

//                //    while (csv.Read())
//                //    {
//                //        lines.Add(new List<string>(csv.CurrentRecord.Select(Utils.FilterHtmlTag)));
//                //    }
//                //}

//                using (var reader = new StreamReader(filePath, Encoding.Default, true))
//                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
//                {
//                    csv.Configuration.HasHeaderRecord = false;
//                    csv.Configuration.Delimiter = string.IsNullOrEmpty(delimiter) ? "," : delimiter;
//                    csv.Configuration.ShouldSkipRecord = record => true;
//                    while (csv.Read())
//                    {
//                        lines.Add(new List<string>(csv.Context.Record.Select(Utils.FilterHtmlTag)));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Read file failed. Error: " + ex.Message + " " + ex.StackTrace);
//            }

//            if (lines.Count < 1)
//            {
//                throw new ArgumentException("File format error.");
//            }

//            return lines;
//        }

//        private void DetectFileFormat(string culture)
//        {
//            try
//            {
//                _cultureInfo = new CultureInfo(culture);
//            }
//            catch (Exception)
//            {
//                _cultureInfo = CultureInfo.InvariantCulture;
//            }

//            // Support for integer number: group digit, floating point like: 1,000.00
//            // This Regular expression is: ^[-+]?(\d?\d?\d?,?)?(\d{3},?)*(\.0{1,2})?$ with delimeters are replaced by culture format
//            _regxInt = new Regex(@"^[-+]?(\d?\d?\d?" + _cultureInfo.NumberFormat.NumberGroupSeparator.Replace(" ", "\\s") + @"?)?(\d{3}" + _cultureInfo.NumberFormat.NumberGroupSeparator.Replace(" ", "\\s") + @"?)*(" + _cultureInfo.NumberFormat.NumberDecimalSeparator.Replace(".", "\\.") + @"0{1,2})?$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
//            // Support for float number: group digit, floating point, E number like: 1,000.0E-1
//            // This Regular expression is: ^[-+]?(\d?\d?\d?,?)?(\d{3},?)*(\.[0-9]*([eE][-+]?[0-9]+)?)?$ with delimeters are replaced by culture format
//            _regxDouble = new Regex(@"^[-+]?(\d?\d?\d?" + _cultureInfo.NumberFormat.NumberGroupSeparator.Replace(" ", "\\s") + @"?)?(\d{3}" + _cultureInfo.NumberFormat.NumberGroupSeparator.Replace(" ", "\\s") + @"?)*(" + _cultureInfo.NumberFormat.NumberDecimalSeparator.Replace(".", "\\.") + @"[0-9]*([eE][-+]?[0-9]+)?)?$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
//            // Support for boolean like: true, false
//            _regxBool = new Regex(@"^(true|false)$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
//        }

//        private List<CellHeader> FetchHeader(IEnumerable<string> input)
//        {
//            return input.Select((item, index) => new CellHeader
//            {
//                Name = item,
//                ColIndex = index
//            }).ToList();
//        }

//        private List<CellModel> FetchValues(IEnumerable<string> input)
//        {
//            var ret = new List<CellModel>();
//            int i = 0;

//            foreach (var splitedValue in input)
//            {
//                CellType type;
//                object value = ParseValue(splitedValue, out type);

//                ret.Add(new CellModel
//                {
//                    Value = value,
//                    Type = type,
//                    ColIndex = i,
//                    PropName = string.Empty
//                });

//                i++;
//            }

//            return ret;
//        }

//        private object ParseValue(string input, out CellType type)
//        {
//            if (!string.IsNullOrWhiteSpace(input))
//            {
//                if (_regxInt.IsMatch(input))
//                {
//                    int retInt;

//                    if (int.TryParse(input, NumberStyles.Currency, _cultureInfo, out retInt))
//                    {
//                        type = CellType.Int;
//                        return retInt;
//                    }

//                    long retLong;

//                    if (long.TryParse(input, NumberStyles.Currency, _cultureInfo, out retLong))
//                    {
//                        type = CellType.Long;

//                        return retLong;
//                    }

//                    type = CellType.Int;

//                    return null;
//                }

//                if (_regxDouble.IsMatch(input))
//                {
//                    double retDouble;

//                    type = CellType.Double;

//                    if (double.TryParse(input, NumberStyles.Any, _cultureInfo, out retDouble))
//                    {
//                        return retDouble;
//                    }

//                    return null;
//                }

//                if (_regxBool.IsMatch(input))
//                {
//                    bool retBool;

//                    type = CellType.Bool;

//                    if (bool.TryParse(input, out retBool))
//                    {
//                        return retBool;
//                    }

//                    return null;
//                }

//                DateTime retDateTime;

//                if (DateTime.TryParse(input, _cultureInfo, DateTimeStyles.None, out retDateTime))
//                {
//                    type = CellType.DateTime;
//                    return retDateTime;
//                }
//            }

//            type = CellType.String;

//            return Utils.FilterHtmlTag(input);
//        }

//        public void WriteDemo(string[] properties, Stream outputStream, int type = 0, string delimiter = "")
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
