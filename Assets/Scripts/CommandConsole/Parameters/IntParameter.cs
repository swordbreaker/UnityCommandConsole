namespace CommandConsole.Parameters
{
    public class IntParameter : NumericParameter<int>
    {
        public IntParameter(string name, bool optional = false) : base(name, int.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            int tmp;
            return int.TryParse(value, out tmp);
        }
    }
}
