using TextProcessing.Patterns.Results;
using TextProcessing.Patterns.Statuses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns
{
    /// <summary>
    /// Interface for a collection of patterns.
    /// </summary>
    public interface PatternCollection : IEnumerable<Pattern>
    {
        PatternCollection Add(string name, Pattern pattern);

        PatternCollection Add(Pattern pattern);

        PatternCollection Add(string name, Pattern pattern, int min);

        PatternCollection Add(Pattern pattern, int min);

        PatternCollection Add(string name, Pattern pattern, int min, int max);

        PatternCollection Add(Pattern pattern, int min, int max);

        PatternCollection Add(string name, Pattern pattern, char symbol);

        PatternCollection Add(Pattern pattern, char symbol);
    }

    /// <summary>
    /// Base implementation for a collection of patterns.
    /// </summary>
    /// <typeparam name="TStatus"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class PatternCollection<TStatus, TResult> : Pattern<TStatus, TResult>, PatternCollection where TStatus : Status where TResult : Result
    {
        protected List<Pattern> collection;

        public PatternCollection()
        {
            collection = new List<Pattern>();
        }

        public PatternCollection(Pattern[] patterns)
        {
            collection = new List<Pattern>(patterns);
        }

        public PatternCollection Add(string name, Pattern pattern)
        {
            collection.Add(pattern.Copy(name));
            return this;
        }

        public PatternCollection Add(Pattern pattern)
        {
            collection.Add(pattern);
            return this;
        }

        public PatternCollection Add(string name, Pattern pattern, int min)
        {
            collection.Add(new PatternBlock(name, pattern, min, -1));
            return this;
        }

        public PatternCollection Add(Pattern pattern, int min)
        {
            collection.Add(new PatternBlock(pattern, min, -1));
            return this;
        }

        public PatternCollection Add(string name, Pattern pattern, int min, int max)
        {
            collection.Add(new PatternBlock(name, pattern, min, max));
            return this;
        }

        public PatternCollection Add(Pattern pattern, int min, int max)
        {
            collection.Add(new PatternBlock(pattern, min, max));
            return this;
        }

        public PatternCollection Add(string name, Pattern pattern, char symbol)
        {
            if (symbol == '*') {
                return Add(name, pattern, 0, -1);
            } else if (symbol == '+') {
                return Add(name, pattern, 1, -1);
            } else if (symbol == '?') {
                return Add(name, pattern, 0, 1);
            } else {
                throw new ArgumentException();
            }
        }

        public PatternCollection Add(Pattern pattern, char symbol)
        {
            return Add(null, pattern, symbol);
        }

        public PatternCollection Clear()
        {
            collection.Clear();
            return this;
        }

        public IEnumerator<Pattern> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
