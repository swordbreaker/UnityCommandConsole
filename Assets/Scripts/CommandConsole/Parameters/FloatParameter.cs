namespace CommandConsole.Parameters
{
    public class FloatParameter : NumericParameter<float>
    {
        public FloatParameter(string name, bool optional = false) : base(name, float.Parse, optional) {}

        public override string GetSyntax()
        {
            return string.Format("float:{0}", Name);
        }

        protected override bool CanParse(string value)
        {
            float tmp;
            return float.TryParse(value, out tmp);
        }
    }
}
