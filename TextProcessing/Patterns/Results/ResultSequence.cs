using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Results
{
    /// <summary>
    /// Result curresponding to a "sequence" pattern.
    /// </summary>
    public class ResultSequence : ResultCollection
    {
        private Dictionary<string, List<Result>> resultsMap = null;

        public ResultSequence(string name, string target, int index, int length, Result[] results) : base(name, target, index, length, results)
        {
        }

        public override Result GetResult(string name)
        {
            return GetResult(name, 0);
        }

        public override Result GetResult(string name, int index)
        {
            InitializeResultsMap();
            List<Result> value;
            if (resultsMap.TryGetValue(name, out value) && value != null) {
                return index < value.Count ? value[index] : null;
            }
            return null;
        }

        public override Result[] GetResults(string name)
        {
            InitializeResultsMap();
            List<Result> value;
            if (resultsMap.TryGetValue(name, out value) && value != null) {
                return value.ToArray();
            }
            return new Result[0];
        }

        protected void InitializeResultsMap()
        {
            if (resultsMap == null) {
                resultsMap = new Dictionary<string, List<Result>>();
                BuildResultsMap(resultsMap, results);
            }
        }

        protected virtual void BuildResultsMap(Dictionary<string, List<Result>> map, Result[] results)
        {
            foreach (Result result in results) {
                if (!string.IsNullOrEmpty(result.Name)) {
                    BuildResultsMap(map, result.Name, result);
                }
                if (result.HasSubResults()) {
                    BuildResultsMap(map, result.GetSubResults());
                }
            }
        }

        protected void BuildResultsMap(Dictionary<string, List<Result>> map, string name, Result result)
        {
            List<Result> value;
            if (map.TryGetValue(name, out value) && value != null) {
                value.Add(result);
            } else {
                value = new List<Result>();
                value.Add(result);
                map[name] = value;
            }
        }
    }
}
