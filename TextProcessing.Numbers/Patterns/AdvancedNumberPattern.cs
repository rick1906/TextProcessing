using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TextProcessing.Patterns.Results;
using TextProcessing.Patterns;
using NumberProcessing;

namespace TextProcessing.Numbers.Patterns
{
    public class AdvancedNumberPattern : SimplePattern
    {
        protected Func<AdvancedNumber, bool> validator;

        public AdvancedNumberPattern(Func<AdvancedNumber, bool> validator)
        {
            this.validator = validator;
        }

        public AdvancedNumberPattern()
        {
            this.validator = null;
        }

        protected override Result SimpleMatch(string target, int startIndex)
        {
            Regex regex = new Regex(@"\G" + AdvancedNumberParser.GetRegexString(), RegexOptions.IgnoreCase);
            Match match = regex.Match(target, startIndex);
            if (match.Success) {
                string s = match.Value;
                AdvancedNumber value;
                if (AdvancedNumber.TryParse(s, out value) && (validator == null || validator.Invoke(value))) {
                    return new SimpleResult<AdvancedNumber>(Name, target, match.Index, match.Length, value);
                }
            }
            return null;
        }

        public override Pattern Copy()
        {
            return new AdvancedNumberPattern(validator);
        }
    }
}
