using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns
{
    public abstract class SimplePattern : Pattern
    {
        protected abstract Result SimpleMatch(string target, int startIndex);

        public sealed override Result TryMatch(Status status)
        {
            if (status != null) {
                return SimpleMatch(status.Target, status.StartIndex);
            } else {
                return null;
            }
        }

        public sealed override Result FirstMatch(string target, int startIndex, out Status status)
        {
            status = null;
            return SimpleMatch(target, startIndex);
        }
    }
}
