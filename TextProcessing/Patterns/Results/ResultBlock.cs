using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    /// <summary>
    /// Result curresponding to a "block" pattern.
    /// </summary>
    public class ResultBlock : ResultCollection
    {
        public ResultBlock(string name, string target, int index, int length, Result[] results) : base(name, target, index, length, results)
        {
        }
    }
}
