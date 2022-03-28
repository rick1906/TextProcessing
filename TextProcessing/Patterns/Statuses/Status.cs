using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Statuses
{
    public class Status
    {
        public string Target { get; private set; }
        public int StartIndex { get; private set; }

        public Status(string target, int startIndex)
        {
            Target = target;
            StartIndex = startIndex;
        }

        public virtual bool Next()
        {
            return false;
        }
    }
}
