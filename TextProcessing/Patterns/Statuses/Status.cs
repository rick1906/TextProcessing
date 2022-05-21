using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Statuses
{
    /// <summary>
    /// The base class for parser status (final automation state container with some metadata).
    /// Contains current parsing data corresponding to a current pattern.
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Target string (string being parsed).
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// Start index in string.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="startIndex"></param>
        public Status(string target, int startIndex)
        {
            Target = target;
            StartIndex = startIndex;
        }

        /// <summary>
        /// Try next possible state.
        /// </summary>
        /// <returns>success or not</returns>
        public virtual bool Next()
        {
            return false;
        }
    }
}
