using System;

namespace Uncas.Sandbox.Fraud
{
    public class Feature<T>
    {
        public Feature(string name, Func<T, double> value, double initialGuess = 0d)
        {
            Name = name;
            Value = value;
            InitialGuess = initialGuess;
        }

        public string Name { get; private set; }
        public Func<T, double> Value { get; private set; }
        public double InitialGuess { get; private set; }
    }
}