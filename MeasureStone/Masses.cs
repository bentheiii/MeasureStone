using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace MeasureStone
{
    //arbitrary is kilogram
    public class Mass : IUnit<Mass>, ScaleMeasurement<Mass>, DeltaMeasurement<Mass>, IComparable<Mass>
    {
        public Mass(BigRational val, IUnit<Mass> unit) : this(unit.ToArbitrary(val)) { }
        public Mass(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        BigRational ScaleMeasurement<Mass>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        BigRational DeltaMeasurement<Mass>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        public override BigRational FromArbitrary(BigRational arb)
        {
            return arb / Arbitrary;
        }
        public override BigRational ToArbitrary(BigRational val)
        {
            return val * Arbitrary;
        }
        public int CompareTo(Mass other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }

        private static readonly Lazy<Funnel<string, Mass>> DefaultParsers;
        public static Mass Parse(string s)
        {
            return DefaultParsers.Value.Process(s);
        }

        public static readonly Mass Milligram, Gram, KiloGram, Tonne, Ounce, Pound;
        static Mass()
        {
            KiloGram = new Mass(1);
            Gram = new Mass(0.001);
            Tonne = new Mass(1000);
            Milligram = new Mass(1E-06);
            Pound = new Mass(0.45359237);
            Ounce = new Mass(28.349523125, Gram);
            _udic = new Dictionary<string, Tuple<IUnit<Mass>, string>>(11)
            {
                ["K"] = Tuple.Create<IUnit<Mass>, string>(KiloGram, "kg"),
                ["M"] = Tuple.Create<IUnit<Mass>, string>(Milligram, "mg"),
                ["G"] = Tuple.Create<IUnit<Mass>, string>(Gram, "g"),
                ["T"] = Tuple.Create<IUnit<Mass>, string>(Tonne, "t"),
                ["O"] = Tuple.Create<IUnit<Mass>, string>(Ounce, "oz"),
                ["L"] = Tuple.Create<IUnit<Mass>, string>(Pound, "lb")
            };
            DefaultParsers = new Lazy<Funnel<string, Mass>>(() => new Funnel<string, Mass>(
                new Parser<Mass>($@"^({CommonRegex.RegexDouble}) ?(kg?|kilograms?)$", m => new Mass(double.Parse(m.Groups[1].Value), KiloGram)),
                new Parser<Mass>($@"^({CommonRegex.RegexDouble}) ?(g|grams?)$", m => new Mass(double.Parse(m.Groups[1].Value), Gram)),
                new Parser<Mass>($@"^({CommonRegex.RegexDouble}) ?(mg|milligrams?)$", m => new Mass(double.Parse(m.Groups[1].Value), Milligram)),
                new Parser<Mass>($@"^({CommonRegex.RegexDouble}) ?(t|tons?)$", m => new Mass(double.Parse(m.Groups[1].Value), Tonne)),
                new Parser<Mass>($@"^({CommonRegex.RegexDouble}) ?(oz|ounces?)$", m => new Mass(double.Parse(m.Groups[1].Value), Ounce)),
                new Parser<Mass>($@"^({CommonRegex.RegexDouble}) ?(lb|pounds?)$", m => new Mass(double.Parse(m.Groups[1].Value), Pound))
                ));
        }
        public static Mass operator -(Mass a)
        {
            return (-1.0 * a);
        }
        public static Mass operator *(Mass a, double b)
        {
            return new Mass(a.Arbitrary * b);
        }
        public static Mass operator /(Mass a, double b)
        {
            return a * (1 / b);
        }
        public static Mass operator *(double b, Mass a)
        {
            return a * b;
        }
        public static Mass operator +(Mass a, Mass b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new Mass(c);
        }
        public static Mass operator -(Mass a, Mass b)
        {
            return a + (-b);
        }
        public static BigRational operator /(Mass a, Mass b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static readonly IDictionary<string, Tuple<IUnit<Mass>, string>> _udic;
        public override IDictionary<string, Tuple<IUnit<Mass>, string>> unitDictionary => _udic;
        //accepted formats (K|M|G|T|O|L)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "K", formatProvider, scaleDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as Mass;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
}
