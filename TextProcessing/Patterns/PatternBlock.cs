using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns
{
    public class PatternBlock : Pattern<StatusBlock, ResultBlock>
    {
        public int Min { get; private set; }
        public int Max { get; private set; }
        public Pattern Pattern { get; private set; }

        public PatternBlock(Pattern pattern, int min, int max)
        {
            Min = min;
            Max = max;
            Pattern = pattern;
        }

        public PatternBlock(string name, Pattern pattern, int min, int max) : this(pattern, min, max)
        {
            Name = name;
        }

        protected void GreedyMatching(StatusBlock status)
        {
            int count = status.MatchesCount;
            while (count < Max || Max < 0) {
                Status nextStatus;
                Result nextResult = Pattern.FirstMatch(status.Target, status.GetNextTargetIndex(), out nextStatus);
                if (nextResult != null && nextResult.Length > 0) {
                    status.Append(nextStatus, nextResult);
                    count++;
                } else {
                    break;
                }
            }
        }

        public override ResultBlock TryMatch(StatusBlock status)
        {
            if (status == null) {
                return null;
            } else if (status.NeedsMatch()) {
                Result firstResult = Pattern.TryMatch(status.CurrentStatus);
                if (firstResult == null) {
                    return null;
                } else {
                    status.Apply(firstResult);
                    GreedyMatching(status);
                }
            }
            if (Min > 0 && status.MatchesCount < Min) {
                return null;
            } else {
                return new ResultBlock(Name, status.Target, status.GetResultIndex(), status.GetResultLength(), status.GetResults());
            }
        }

        public override ResultBlock FirstMatch(string target, int startIndex, out StatusBlock status)
        {
            if (Max == 0) {
                status = null;
                return null;
            }

            status = new StatusBlock(target, startIndex, Min);
            Status firstStatus;
            Result firstResult = Pattern.FirstMatch(target, startIndex, out firstStatus);
            if (firstResult == null) {
                return Min <= 0 ? new ResultBlock(Name, status.Target, status.StartIndex, 0, new Result[0]) : null;
            } else {
                status.Append(firstStatus, firstResult);
                GreedyMatching(status);
            }
            if (Min > 0 && status.MatchesCount < Min) {
                return NextMatch(status);
            } else {
                return new ResultBlock(Name, status.Target, status.GetResultIndex(), status.GetResultLength(), status.GetResults());
            }
        }

        public override Pattern Copy()
        {
            return new PatternBlock(Pattern, Min, Max);
        }
    }        
}
