using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns
{
    public class PatternSequence : PatternCollection<StatusSequence, ResultSequence>
    {
        public PatternSequence()
        {
        }

        public PatternSequence(Pattern[] patterns) : base(patterns)
        {
        }

        protected bool SequenceMatching(StatusSequence status)
        {
            int currentCount = status.MatchesCount;
            int targetCount = collection.Count;
            while (currentCount < targetCount) {
                Status nextStatus;
                Result nextResult = collection[currentCount].FirstMatch(status.Target, status.GetNextTargetIndex(), out nextStatus);
                if (nextResult != null) {
                    status.Append(nextStatus, nextResult);
                    currentCount++;
                } else {
                    break;
                }
            }
            return currentCount == targetCount;
        }

        public override ResultSequence TryMatch(StatusSequence status)
        {
            if (status != null && status.NeedsMatch()) {
                int currentIndex = status.CurrentMatchIndex;
                Result currentResult = collection[currentIndex].TryMatch(status.CurrentStatus);
                if (currentResult != null) {
                    status.Apply(currentResult);
                    if (SequenceMatching(status)) {
                        return new ResultSequence(Name, status.Target, status.GetResultIndex(), status.GetResultLength(), status.GetResults());
                    }
                }
            }
            return null;
        }

        public override ResultSequence FirstMatch(string target, int startIndex, out StatusSequence status)
        {
            status = new StatusSequence(target, startIndex);
            if (SequenceMatching(status)) {
                return new ResultSequence(Name, status.Target, status.GetResultIndex(), status.GetResultLength(), status.GetResults());
            } else {
                return NextMatch(status);
            }
        }

        public override Pattern Copy()
        {
            return new PatternSequence(collection.ToArray());
        }
    }
}
