using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TextProcessing.Patterns.Results;

namespace TextProcessing.Patterns
{
    /// <summary>
    /// Pattern for a number.
    /// </summary>
    public class NumberPattern : SimplePattern
    {
        protected static Regex numberRegex = new Regex(@"\G-?\d+(?:[,\.]\d+)?(?:E-?\d+)?", RegexOptions.IgnoreCase);
        protected static Regex intRegex = new Regex(@"\G-?\d+", RegexOptions.IgnoreCase);

        public NumberStyles NumberStyles { get; private set; }
        public bool IsInteger { get { return (NumberStyles & (NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)) == 0; } }

        public NumberPattern(NumberStyles numberStyles)
        {
            NumberStyles = numberStyles;
        }

        public NumberPattern() : this(NumberStyles.Float)
        {
        }

        protected override Result SimpleMatch(string target, int startIndex)
        {
            bool isInteger = IsInteger;
            Regex regex = isInteger ? intRegex : numberRegex;
            Match match = regex.Match(target, startIndex);
            if (match.Success) {
                string s = match.Value.Replace(',', '.');
                if (isInteger) {
                    int value1;
                    if (int.TryParse(s, NumberStyles, CultureInfo.InvariantCulture, out value1)) {
                        return new SimpleResult<int>(Name, target, match.Index, match.Length, value1);
                    }
                } else {
                    double value2;
                    if (double.TryParse(s, NumberStyles, CultureInfo.InvariantCulture, out value2)) {
                        return new SimpleResult<double>(Name, target, match.Index, match.Length, value2);
                    }
                }
            }
            return null;
        }

        public override Pattern Copy()
        {
            return new NumberPattern(NumberStyles);
        }
    }
}
