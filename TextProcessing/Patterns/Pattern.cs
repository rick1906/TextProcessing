using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextProcessing.Patterns
{
    public abstract class Pattern
    {
        public string Name { get; set; } = null;

        public static implicit operator Pattern(string pattern)
        {
            return (StringPattern)pattern;
        }

        public static implicit operator Pattern(Regex pattern)
        {
            return (StringPattern)pattern;
        }

        public abstract Result TryMatch(Status status);

        public abstract Result FirstMatch(string target, int startIndex, out Status status);

        public Result NextMatch(Status status)
        {
            if (status == null) {
                return null;
            }
            while (status.Next()) {
                Result result = TryMatch(status);
                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        public Result Match(string target, int startIndex)
        {
            Status status;
            return FirstMatch(target, startIndex, out status);
        }

        public Result Match(string target)
        {
            Status status;
            return FirstMatch(target, 0, out status);
        }

        public abstract Pattern Copy();

        public Pattern Copy(string name)
        {
            Pattern copy = Copy();
            copy.Name = name;
            return copy;
        }        
    }

    public abstract class Pattern<TStatus, TResult> : Pattern where TStatus : Status where TResult : Result
    {
        public sealed override Result TryMatch(Status status)
        {
            return TryMatch((TStatus)status);
        }

        public abstract TResult TryMatch(TStatus status);

        public sealed override Result FirstMatch(string target, int startIndex, out Status status)
        {
            TStatus internalStatus = null;
            TResult result = FirstMatch(target, startIndex, out internalStatus);
            status = internalStatus;
            return result;
        }

        public abstract TResult FirstMatch(string target, int startIndex, out TStatus status);

        public TResult NextMatch(TStatus status)
        {
            return (TResult)base.NextMatch(status);
        }

        public new TResult Match(string target, int startIndex)
        {
            return (TResult)base.Match(target, startIndex);
        }

        public new TResult Match(string target)
        {
            return (TResult)base.Match(target);
        }
    }
}
