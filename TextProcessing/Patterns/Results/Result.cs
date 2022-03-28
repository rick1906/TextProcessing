using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    public class Result
    {
        public string Name { get; private set; }
        public string Target { get; private set; }
        public int Index { get; private set; }
        public int Length { get; private set; }

        public Result this[int index] { get { return GetResult(index); } }
        public Result this[string name] { get { return GetResult(name); } }
        public Result this[string name, int index] { get { return GetResult(name, index); } }

        public Result(string name, string target, int index, int length)
        {
            Name = name;
            Target = target;
            Index = index;
            Length = length;
        }

        public string GetString()
        {
            return Target != null ? Target.Substring(Index, Length) : null;
        }

        public virtual object GetValue()
        {
            return GetString();
        }

        public virtual Result GetResult(int index)
        {
            if (index == 0) {
                return this;
            } else {
                return null;
            }
        }

        public virtual Result GetResult(string name)
        {
            if (Name == name) {
                return this;
            } else {
                return null;
            }
        }

        public virtual Result[] GetResults(string name)
        {
            return new Result[0];
        }

        public virtual Result GetResult(string name, int index)
        {
            Result result = GetResult(name);
            if (result != null) {
                return result.GetResult(index);
            }
            return null;
        }

        public virtual bool HasSubResults()
        {
            return false;
        }

        public virtual Result[] GetSubResults()
        {
            return new Result[0];
        }

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

        public override string ToString()
        {
            return ToString(false);
        }
    }

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
