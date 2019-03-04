using System;
using System.Collections.Generic;
using MyPhotoBL.Enums;

namespace MyPhotoBL
{
    public class IntegerFilter
    {
        public CompareRealtion Relation { get; set; } = CompareRealtion.Equal;

        public int FilterValue { get; set; } = 0;

        public bool Filter(int value)
        {
            switch (Relation)
            {
                case CompareRealtion.Equal:        return value == FilterValue;
                case CompareRealtion.NorEqual:     return value != FilterValue;
                case CompareRealtion.Less:         return value <  FilterValue;
                case CompareRealtion.Greater:      return value >  FilterValue;
                case CompareRealtion.LessEqual:    return value <= FilterValue;
                case CompareRealtion.GreaterEqual: return value >= FilterValue;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}