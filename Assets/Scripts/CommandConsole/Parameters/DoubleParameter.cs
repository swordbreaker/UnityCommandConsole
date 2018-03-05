namespace CommandConsole.Parameters
{
    public class DoubleParameter : NumericParameter<double>
    {
        public DoubleParameter(string name, bool optional = false) : base(name, double.Parse, optional) {}

        protected override bool CanParse(string value)
        {
            double tmp;
            return double.TryParse(value, out tmp);
        }
    }
}
