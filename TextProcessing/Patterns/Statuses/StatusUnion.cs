using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Statuses
{
    public class StatusUnion : Status
    {
        public Status BaseStatus { get; private set; }

        public StatusUnion(string target, int startIndex, Status baseStatus) : base(target, startIndex)
        {
            BaseStatus = baseStatus;
        }

        public override bool Next()
        {
            return BaseStatus != null && BaseStatus.Next();
        }
    }
}
