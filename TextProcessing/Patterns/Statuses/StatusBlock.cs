using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Statuses
{
    public class StatusBlock : StatusSequence
    {
        protected int minimum;

        public StatusBlock(string target, int startIndex, int minimum) : base(target, startIndex)
        {
            this.minimum = minimum;
        }

        protected override bool Fallback(int index)
        {
            do {
                statuses.RemoveAt(index);
                results.RemoveAt(index);
                index--;
                if (minimum <= 0 || MatchesCount >= minimum) {
                    return true;
                }
                if (index < 0) {
                    return false;
                }
                if (statuses[index] != null && statuses[index].Next()) {
                    results[index] = null;
                    return true;
                }
            } while (index >= 0);
            return false;
        }
    }
}