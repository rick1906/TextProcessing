using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns
{
    public class PatternUnion : PatternCollection<StatusUnion, ResultVariant>
    {
        public PatternUnion()
        {
        }

        public PatternUnion(Pattern[] patterns) : base(patterns)
        {
        }

        protected Result UnionMatching(Result original, int matchedPatternIndex)
        {
            // NOTICE: Does not work with STRING END symbol ($)
            int count = collection.Count;
            string resultString = original.GetString();
            for (int i = 0; i < count; ++i) {
                if (i != matchedPatternIndex) {
                    Status subStatus;
                    Result subResult = collection[i].FirstMatch(resultString, 0, out subStatus);
                    if (subResult == null) {
                        return null;
                    }

                    int maxLength = subResult.Length;
                    Result maxResult = subResult;
                    if (subStatus != null) {
                        while (maxLength < resultString.Length) {
                            subResult = collection[i].NextMatch(subStatus);
                            if (subResult != null) {
                                if (subResult.Length > maxLength) {
                                    maxLength = subResult.Length;
                                    maxResult = subResult;
                                }
                            } else {
                                break;
                            }
                        }
                    }
                    if (maxLength < resultString.Length) {
                        return UnionMatching(maxResult, i);
                    }
                }
            }
            return original;
        }

        public override ResultVariant TryMatch(StatusUnion status)
        {
            if (status != null && collection.Count != 0) {
                Result nextResult = collection[0].TryMatch(status.BaseStatus);
                if (nextResult != null) {
                    Result result = UnionMatching(nextResult, 0);
                    if (result != null) {
                        return new ResultVariant(Name, status.Target, status.StartIndex, result);
                    }
                }
            }
            return null;
        }

        public override ResultVariant FirstMatch(string target, int startIndex, out StatusUnion status)
        {
            if (collection.Count != 0) {
                Status firstStatus;
                Result firstResult = collection[0].FirstMatch(target, startIndex, out firstStatus);
                if (firstResult != null) {
                    Result result = UnionMatching(firstResult, 0);
                    if (result != null) {
                        status = new StatusUnion(target, startIndex, firstStatus);
                        return new ResultVariant(Name, target, startIndex, result);
                    }
                }
            }

            status = null;
            return null;
        }

        public override Pattern Copy()
        {
            return new PatternUnion(collection.ToArray());
        }
    }
}
