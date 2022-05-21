using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    /// <summary>
    /// Base class for parsing result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Name of the matched pattern.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Target string.
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// Substring index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Substring length.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Get subresult by index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Result this[int index] { get { return GetResult(index); } }

        /// <summary>
        /// Get subresult by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Result this[string name] { get { return GetResult(name); } }

        /// <summary>
        /// Get subresult by name and index.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Result this[string name, int index] { get { return GetResult(name, index); } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        public Result(string name, string target, int index, int length)
        {
            Name = name;
            Target = target;
            Index = index;
            Length = length;
        }

        /// <summary>
        /// Get substring corresponding to the result.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            return Target != null ? Target.Substring(Index, Length) : null;
        }

        /// <summary>
        /// Get value (string, number, etc) corresponding to the result.
        /// </summary>
        /// <returns></returns>
        public virtual object GetValue()
        {
            return GetString();
        }

        /// <summary>
        /// Get subresult.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Result GetResult(int index)
        {
            if (index == 0) {
                return this;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Get subresult.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Result GetResult(string name)
        {
            if (Name == name) {
                return this;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Get subresults.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Result[] GetResults(string name)
        {
            return new Result[0];
        }

        /// <summary>
        /// Get subresult.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual Result GetResult(string name, int index)
        {
            Result result = GetResult(name);
            if (result != null) {
                return result.GetResult(index);
            }
            return null;
        }

        /// <summary>
        /// True if this result has subresults.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasSubResults()
        {
            return false;
        }

        /// <summary>
        /// Get subresults.
        /// </summary>
        /// <returns></returns>
        public virtual Result[] GetSubResults()
        {
            return new Result[0];
        }

        /// <summary>
        /// String representation of the result.
        /// </summary>
        /// <param name="withType"></param>
        /// <returns></returns>
        public virtual string ToString(bool withType)
        {
            string npre = string.IsNullOrEmpty(Name) ? "" : (Name + ":");
            if (withType && !HasSubResults()) {
                object obj = GetValue();
                if (!(obj is string)) {
                    npre += "(" + obj.GetType().Name + ")";
                }
            }
            if (HasSubResults()) {
                Result[] results = GetSubResults();
                if (results.Length == 0) {
                    return npre + "[]";
                } else if (results.Length == 1) {
                    return npre + results[0].ToString(withType);
                } else {
                    StringBuilder sb = new StringBuilder();
                    foreach (Result result in results) {
                        sb.Append(result.ToString(withType));
                        sb.Append(",");
                    }
                    return npre + "[" + sb.ToString(0, sb.Length - 1) + "]";
                }
            }
            if (Target != null) {
                return npre + "{" + GetString() + "}";
            } else if (Length == 0) {
                return npre + "{}";
            } else if (Length == 1) {
                return npre + "{" + Index + "}";
            } else {
                return npre + "{" + Index + "-" + (Index + Length) + "}";
            }
        }

        /// <summary>
        /// String representation of the result.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(false);
        }
    }

    /// <summary>
    /// Generic variant of <c>Result</c> class.
    /// </summary>
    /// <typeparam name="TValue">type of result value</typeparam>
    public abstract class Result<TValue> : Result
    {
        public Result(string name, string target, int index, int length) : base(name, target, index, length)
        {
        }

        public new TValue GetValue()
        {
            return ParseValue(GetString());
        }

        protected abstract TValue ParseValue(string value);
    }
}
