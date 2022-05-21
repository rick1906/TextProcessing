using TextProcessing.Patterns.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextProcessing.Patterns
{
    /// <summary>
    /// Pattern for a string.
    /// </summary>
    public class StringPattern : SimplePattern
    {
        public Regex Regex { get; private set; }

        public StringPattern(string pattern, RegexOptions options, bool anchored)
        {
            if (anchored) {
                if ((options & RegexOptions.RightToLeft) == 0) {
                    if (pattern.Length < 3 || pattern.Substring(0, 3) != @"\G(") {
                        pattern = @"\G(?:" + pattern + ")";
                    }
                } else {
                    if (pattern.Length < 3 || pattern.Substring(pattern.Length - 3, 3) != @")\G") {
                        pattern = "(?:" + pattern + @")\G";
                    }
                }
            }
            Regex = new Regex(pattern, options);
        }

        public StringPattern(string pattern, RegexOptions options) : this(pattern, options, true)
        {
        }

        public StringPattern(string pattern) : this(pattern, RegexOptions.None)
        {
        }

        public StringPattern(string pattern, bool anchored) : this(pattern, RegexOptions.None, anchored)
        {
        }

        public StringPattern(Regex pattern) : this(pattern.ToString(), pattern.Options)
        {
        }

        public StringPattern(Regex pattern, bool anchored) : this(pattern.ToString(), pattern.Options, anchored)
        {
        }

        public static implicit operator StringPattern(string pattern)
        {
            return new StringPattern(pattern);
        }

        public static implicit operator StringPattern(Regex pattern)
        {
            return new StringPattern(pattern);
        }

        protected override Result SimpleMatch(string target, int startIndex)
        {
            Match match = Regex.Match(target, startIndex);
            if (match.Success) {
                return new Result(Name, target, match.Index, match.Length);
            }
            return null;
        }

        public override Pattern Copy()
        {
            return new StringPattern(Regex);
        }
    }
}
