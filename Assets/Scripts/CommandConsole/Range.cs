namespace CommandConsole
{
    /// <summary>
    /// Imutable struct to store a Range
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    public struct Range<T>
    {
        public delegate bool InRangeCheckerFunction(T value, Range<T> range);

        public readonly T Form;
        public readonly T Till;
        private InRangeCheckerFunction _inRangeChecker;

        /// <summary>
        /// Create a new Range
        /// </summary>
        /// <param name="form">Inclusive</param>
        /// <param name="till">Inclusive</param>
        /// <param name="inRangeChecker">A function which takes a value and a range and outputs if it is in range</param>
        public Range(T form, T till, InRangeCheckerFunction inRangeChecker)
        {
            Form = form;
            Till = till;
            _inRangeChecker = inRangeChecker;
        }

        public bool IsInRange(T value)
        {
            return _inRangeChecker(value, this);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Form, Till);
        }
    }

    public struct Range
    {
        private Range<object> _inner;

        public Range(object from, object till, Range<object>.InRangeCheckerFunction inRangeChecker)
        {
            _inner = new Range<object>(from, till, inRangeChecker);
        }

        public bool IsInRange(object value)
        {
            return _inner.IsInRange(value);
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
    }

    public static class RangeBuilder
    {
        public static Range<float>.InRangeCheckerFunction FloatCheckerFunction
        {
            get { return (value, range) => value >= range.Form && value <= range.Till; }
        }

        public static Range<double>.InRangeCheckerFunction DoubleCheckerFunction
        {
            get { return (value, range) => value >= range.Form && value <= range.Till; }
        }

        public static Range<int>.InRangeCheckerFunction IntCheckerFunction
        {
            get { return (value, range) => value >= range.Form && value <= range.Till; }
        }

        public static Range<float> CreateFloatRange(float from, float till)
        {
            return new Range<float>(from, till, FloatCheckerFunction);
        }

        public static Range<float> CreatePercentageRange()
        {
            return new Range<float>(0f, 1f, FloatCheckerFunction);
        }

        public static Range<double> CreateDoubleRange(double from, double till)
        {
            return new Range<double>(from, till, DoubleCheckerFunction);
        }

        public static Range<int> CreateIntRange(int from, int till)
        {
            return new Range<int>(from, till, IntCheckerFunction);
        }

        public static Range<int> CreateBinaryIntRange()
        {
            return new Range<int>(0, 1, IntCheckerFunction);
        }
    }
}
