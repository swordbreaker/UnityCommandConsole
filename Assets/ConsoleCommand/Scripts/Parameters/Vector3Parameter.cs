using System;
using System.Linq;
using CommandConsole.ConsoleParser;
using UnityEngine;

namespace CommandConsole.Parameters
{
    public class Vector3Parameter : Parameter
    {
        public Vector3Parameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(IValue value)
        {
            var vObject = (VObject)value;

            var floatParameter = new FloatParameter("dummy");
            var floats = vObject.Variables.Select(value1 => (float)floatParameter.Parse(value1)).ToArray();

            return new Vector3(floats[0], floats[1], floats[2]);
        }

        public override bool CanParse(IValue value)
        {
            var vObject = value as VObject;
            if (vObject == null || vObject.Variables.Count != 3) return false;

            var floatParameter = new FloatParameter("dummy");
            return vObject.Variables.All(floatParameter.CanParse);
        }

        public override Type GetParamType()
        {
            return typeof(Vector3);
        }

        public override string GetSyntax()
        {
            return string.Format("(x y z):{0}", Name);
        }
    }
}
