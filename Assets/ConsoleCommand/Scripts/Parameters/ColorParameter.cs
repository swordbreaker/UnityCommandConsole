using System;
using System.Linq;
using CommandConsole.ConsoleParser;
using CommandConsole.Exceptions;
using UnityEngine;

namespace CommandConsole.Parameters
{
    public class ColorParameter : Parameter
    {
        public ColorParameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(IValue value)
        {
            var vObject = (VObject)value;
            if(vObject == null) throw new ParameterException("Cannot parse to color when it is not an Object", this);

            var floatParameter = new FloatParameter("dummy");
            var channels = vObject.Variables.Select(value1 => (float)floatParameter.Parse(value1)).ToArray();

            return new Color(channels[0], channels[1], channels[2], channels[3]);
        }

        public override bool CanParse(IValue value)
        {
            var vObject = value as VObject;
            if (vObject == null || vObject.Variables.Count != 4) return false;

            var floatParameter = new FloatParameter("dummy");
            return vObject.Variables.All(floatParameter.CanParse);
        }

        public override Type GetParamType()
        {
            return typeof(Color);
        }

        public override string GetSyntax()
        {
            return string.Format("(r g b a):{0}", Name);
        }
    }
}