using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    /// <summary>
    /// Result for a single-value pattern.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SimpleResult<TValue> : Result
    {
        public TValue Value { get; private set; }

        public SimpleResult(string name, string target, int index, int length, TValue value) : base(name, target, index, length)
        {
            Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }
    }
}
