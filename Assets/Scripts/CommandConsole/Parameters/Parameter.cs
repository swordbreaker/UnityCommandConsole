using System;
using CommandConsole.ConsoleParser;
using CommandConsole.Exceptions;

namespace CommandConsole.Parameters
{
    public abstract class Parameter : IParameter
    {
        public string Name { get; private set; }
        public bool Optional { get; private set; }

       
        public Parameter(string name, bool optional = false)
        {
            Name = name;
            Optional = optional;
        }

        protected abstract object ParseValue(IValue value);

        public object Parse(IValue value, bool validate = true)
        {
            if (validate) Validate(value);
            if (CanParse(value))
            {
                return ParseValue(value);
            }
            throw new ParameterException(string.Format("Cannot parse {0} to type {1}", value, GetParamType().Name), this);
        }

        public abstract bool CanParse(IValue value);

        public virtual bool IsValid(IValue value)
        {
            return true;
        }

        public virtual void Validate(IValue value)
        {
            if (!IsValid(value))
            {
                throw new ParameterException(string.Format("Validation error on parameter {0}", Name), this);
            }
        }

        public abstract Type GetParamType();

        public virtual string GetSyntax()
        {
            return
                string.Format("{0}:{1}", GetParamType().Name, Name);
        }
    }
}