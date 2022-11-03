using System.Text.RegularExpressions;

namespace AMS.ReportAutomation.Common.FileParser
{
    public static class Utils
    {
        private static readonly Regex RegxHtmlTag = new Regex(@"</?([a-z]+) *[^/]*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Filter html tag like script, a, ...
        /// </summary>
        /// <param name="input">Cell string</param>
        /// <returns>Filtered html tag</returns>
        public static string FilterHtmlTag(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            // Concept: Replace all html tag to readable and unrisk tag like: <script> => «script»
            return RegxHtmlTag.Replace(input, "«$1»");
        }
    }
}
