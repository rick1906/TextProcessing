using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextProcessing.Patterns.Statuses
{
    public class StatusVariants : Status
    {
        public int VariantIndex { get; private set; }
        public int VariantCount { get; private set; }
        public Status VariantStatus { get; set; }

        public StatusVariants(string target, int startIndex, int variantCount) : base(target, startIndex)
        {
            VariantCount = variantCount;
            VariantIndex = 0;
            VariantStatus = null;
        }

        public override bool Next()
        {
            if (VariantStatus != null && VariantStatus.Next()) {
                return true;
            } else {
                VariantStatus = null;
                VariantIndex++;
                return VariantIndex < VariantCount;
            }
        }

    }
}
