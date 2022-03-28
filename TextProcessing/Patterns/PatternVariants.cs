using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns
{
    public class PatternVariants : PatternCollection<StatusVariants, ResultVariant>
    {
        public PatternVariants()
        {
        }

        public PatternVariants(Pattern[] patterns) : base(patterns)
        {
        }

        public override ResultVariant TryMatch(StatusVariants status)
        {
            if (status.VariantIndex < collection.Count) {
                int index = status.VariantIndex;
                if (status.VariantStatus != null) {
                    Result nextResult = collection[index].TryMatch(status.VariantStatus);
                    if (nextResult != null) {
                        return new ResultVariant(Name, nextResult);
                    }
                } else {
                    Status nextStatus;
                    Result nextResult = collection[index].FirstMatch(status.Target, status.StartIndex, out nextStatus);
                    if (nextResult != null) {
                        status.VariantStatus = nextStatus;
                        return new ResultVariant(Name, nextResult);
                    }
                }
            }
            return null;
        }

        public override ResultVariant FirstMatch(string target, int startIndex, out StatusVariants status)
        {
            status = new StatusVariants(target, startIndex, collection.Count);
            if (status.VariantCount == 0) {
                return null;
            }

            Status firstStatus;
            Result firstResult = collection[0].FirstMatch(target, startIndex, out firstStatus);
            if (firstResult != null) {
                status.VariantStatus = firstStatus;
                return new ResultVariant(Name, firstResult);
            } else {
                return NextMatch(status);
            }
        }

        public override Pattern Copy()
        {
            return new PatternVariants(collection.ToArray());
        }
    }
}
