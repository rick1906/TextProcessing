using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextProcessing.Patterns
{
    /// <summary>
    /// Base class for all patterns.
    /// </summary>
    public abstract class Pattern
    {
        /// <summary>
        /// Name to save named results.
        /// </summary>
        public string Name { get; set; } = null;

        public static implicit operator Pattern(string pattern)
        {
            return (StringPattern)pattern;
        }

        public static implicit operator Pattern(Regex pattern)
        {
            return (StringPattern)pattern;
        }

        /// <summary>
        /// Try match this pattern against the status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public abstract Result TryMatch(Status status);

        /// <summary>
        /// Find the first match of this pattern. Return parser status.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startIndex"></param>
        /// <param name="status"></param>
        /// <returns>match result or null if no result found yet</returns>
        public abstract Result FirstMatch(string target, int startIndex, out Status status);

        /// <summary>
        /// Find the next match of this pattern.
        /// </summary>
        /// <param name="status"></param>
        /// <returns>next result or null if none found</returns>
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

        /// <summary>
        /// Match this pattern against a string.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public Result Match(string target, int startIndex)
        {
            Status status;
            return FirstMatch(target, startIndex, out status);
        }

        /// <summary>
        /// Match this pattern against a string.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Result Match(string target)
        {
            Status status;
            return FirstMatch(target, 0, out status);
        }

        /// <summary>
        /// Copy a pattern.
        /// </summary>
        /// <returns></returns>
        public abstract Pattern Copy();

        /// <summary>
        /// Copy a pattern, set a new name for a copy.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Pattern Copy(string name)
        {
            Pattern copy = Copy();
            copy.Name = name;
            return copy;
        }        
    }

    /// <summary>
    /// Generic variant of <c>Pattern</c> class.
    /// </summary>
    /// <typeparam name="TStatus"></typeparam>
    /// <typeparam name="TResult"></typeparam>
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
