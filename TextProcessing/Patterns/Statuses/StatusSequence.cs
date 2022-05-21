using TextProcessing.Patterns.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Statuses
{
    /// <summary>
    /// Status corresponding to a "sequence" pattern.
    /// </summary>
    public class StatusSequence : Status
    {
        protected List<Status> statuses = new List<Status>();
        protected List<Result> results = new List<Result>();

        public int MatchesCount { get { return statuses.Count; } }
        public int CurrentMatchIndex { get { return statuses.Count - 1; } }
        public Status CurrentStatus { get { return statuses.Count > 0 ? statuses[CurrentMatchIndex] : null; } }
        public Result CurrentResult { get { return results.Count > 0 ? results[CurrentMatchIndex] : null; } }

        public StatusSequence(string target, int startIndex) : base(target, startIndex)
        {
        }

        public void Append(Status status, Result result)
        {
            statuses.Add(status);
            results.Add(result);
        }

        public void Apply(Result result)
        {
            results[CurrentMatchIndex] = result;
        }

        public override bool Next()
        {
            int index = CurrentMatchIndex;
            if (index >= 0) {
                if (statuses[index] != null && statuses[index].Next()) {
                    results[index] = null;
                    return true;
                } else {
                    return Fallback(index);
                }
            }
            return false;
        }

        protected virtual bool Fallback(int index)
        {
            do {
                statuses.RemoveAt(index);
                results.RemoveAt(index);
                index--;
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

        public bool NeedsMatch()
        {
            return results.Count > 0 && results[CurrentMatchIndex] == null;
        }

        public int GetNextTargetIndex()
        {
            Result current = CurrentResult;
            if (current != null) {
                return current.Index + current.Length;
            } else {
                return StartIndex;
            }
        }

        public int GetResultIndex()
        {
            if (results.Count > 0) {
                return results[0].Index;
            } else {
                return StartIndex;
            }
        }

        public int GetResultLength()
        {
            if (results.Count == 1) {
                return results[0].Length;
            } else {
                return GetNextTargetIndex() - GetResultIndex();
            }
        }

        public Result[] GetResults()
        {
            return results.ToArray();
        }
    }
}
