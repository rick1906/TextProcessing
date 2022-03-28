using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    public abstract class ResultCollection : Result
    {
        protected Result[] results;

        public ResultCollection(string name, string target, int index, int length, Result[] results) : base(name, target, index, length)
        {
            this.results = results;
        }

        public override object GetValue()
        {
            if (results.Length == 1) {
                return results[0].GetValue();
            } else {
                return base.GetValue();
            }
        }

        public override Result GetResult(int index)
        {
            if (index >= 0 && index < results.Length) {
                return results[index];
            } else {
                return null;
            }
        }

        public override bool HasSubResults()
        {
            return results.Length > 0;
        }

        public override Result[] GetSubResults()
        {
            return results;
        }
    }
}
