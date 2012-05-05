using System;

namespace Uncas.Sandbox.Fraud
{
    public class Feature<T>
    {
        public Feature(string name, Func<T, double> value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public Func<T, double> Value { get; private set; }
    }
}