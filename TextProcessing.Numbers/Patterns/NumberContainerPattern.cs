using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TextProcessing.Patterns.Results;
using TextProcessing.Patterns;
using NumberProcessing.Units;

namespace TextProcessing.Numbers.Patterns
{
    public class NumberContainerPattern : SimplePattern
    {
        protected Func<NumberContainer, bool> validator;

        public NumberContainerPattern(Func<NumberContainer, bool> validator)
        {
            this.validator = validator;
        }

        public NumberContainerPattern()
        {
            this.validator = null;
        }

        protected override Result SimpleMatch(string target, int startIndex)
        {
            int stopIndex = target.Length;
            string s = target.Substring(startIndex);
            do {
                NumberContainer nc;
                if (NumberContainer.TryParse(s, out nc)) {
                    if (validator != null && !validator.Invoke(nc)) {
                        return null;
                    } else {
                        return new SimpleResult<NumberContainer>(Name, target, startIndex, s.Length, nc);
                    }
                }
                Match match = Regex.Match(s, "\\s+", RegexOptions.RightToLeft);
                if (match.Success) {
                    s = s.Substring(0, match.Index).TrimEnd();
                } else {
                    break;
                }
            } while (!string.IsNullOrEmpty(s));
            return null;
        }

        public override Pattern Copy()
        {
            return new NumberContainerPattern(validator);
        }
    }
}
