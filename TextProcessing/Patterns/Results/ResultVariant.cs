using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    /// <summary>
    /// Result curresponding to a "variants" pattern.
    /// </summary>
    public class ResultVariant : ResultCollection //TODO: inherit from result, not resultcollection
    {
        public ResultVariant(string name, string target, int index, Result result) : base(name, target, index, result.Length, new Result[1] { result })
        {
        }

        public ResultVariant(string name, Result result) : this(name, result.Target, result.Index, result)
        {
        }

        public override object GetValue()
        {
            return results[0].GetValue();
        }
    }
}
