using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace MeasureStone
{
    //arbitrary is kelvin
    public class Temperature : ScaleMeasurement<Temperature>, IComparable<Temperature>
    {
        public Temperature(BigRational val, IScaleUnit<Temperature> unit) : this(unit.ToArbitrary(val)) { }
        public Temperature(BigRational arbitrary)
        {
            if (arbitrary < 0)
                throw new Exception("temperature scale cannot be negative");
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        public int CompareTo(Temperature other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }

        private static readonly Lazy<Funnel<string, Temperature>> DefaultParsers;
        public static Temperature Parse(string s)
        {
            return DefaultParsers.Value.Process(s);
        }

        public static readonly IScaleUnit<Temperature> Kelvin, Fahrenheit, Celsius;
        static Temperature()
        {
            Kelvin = new ScaleUnit<Temperature>(_udic,1);
            Celsius = new ScaleUnit<Temperature>(_udic, 1, -273.15);
            Fahrenheit = new ScaleUnit<Temperature>(_udic, 9 / 5.0, -459.67);
            DefaultParsers = new Lazy<Funnel<string, Temperature>>(() => new Funnel<string, Temperature>(
                new Parser<Temperature>($@"^({CommonRegex.RegexDouble}) ?(k|kelvin)$", m => new Temperature(double.Parse(m.Groups[1].Value), Kelvin)),
                new Parser<Temperature>($@"^({CommonRegex.RegexDouble}) ?(f|fahrenheit)$", m => new Temperature(double.Parse(m.Groups[1].Value), Fahrenheit)),
                new Parser<Temperature>($@"^({CommonRegex.RegexDouble}) ?(c|celsius)$", m => new Temperature(double.Parse(m.Groups[1].Value), Celsius))
                ));
        }
        public static Temperature operator +(Temperature t, TemperatureDelta d)
        {
            return new Temperature(t.Arbitrary + d.Arbitrary);
        }
        public static Temperature operator +(TemperatureDelta d, Temperature t)
        {
            return new Temperature(t.Arbitrary + d.Arbitrary);
        }
        public static Temperature operator -(Temperature t, TemperatureDelta d)
        {
            return new Temperature(t.Arbitrary - d.Arbitrary);
        }
        public static TemperatureDelta operator -(Temperature t, Temperature d)
        {
            return new TemperatureDelta(t.Arbitrary - d.Arbitrary);
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static  readonly IDictionary<string, Tuple<IScaleUnit<Temperature>, string>> _udic =
        new Dictionary<string, Tuple<IScaleUnit<Temperature>, string>>(3)
        {
            ["K"] = Tuple.Create(Kelvin, "K"),
            ["F"] = Tuple.Create(Fahrenheit, "F"),
            ["C"] = Tuple.Create(Celsius, "C")
        };
        public IDictionary<string, Tuple<IScaleUnit<Temperature>, string>> scaleDictionary => _udic;
        //accepted formats (K|F|C)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "C", formatProvider, scaleDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as Temperature;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
    public class TemperatureDelta : DeltaMeasurement<TemperatureDelta>, IDeltaUnit<TemperatureDelta>, IComparable<TemperatureDelta>
    {
        public TemperatureDelta(BigRational val, IDeltaUnit<TemperatureDelta> unit) : this(unit.ToArbitrary(val)) { }
        public TemperatureDelta(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        public int CompareTo(TemperatureDelta other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }
        BigRational DeltaMeasurement<TemperatureDelta>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        public BigRational FromArbitrary(BigRational arb)
        {
            return arb * Arbitrary;
        }
        public BigRational ToArbitrary(BigRational val)
        {
            return val / Arbitrary;
        }

        private static readonly Lazy<Funnel<string, TemperatureDelta>> DefaultParsers;
        public static TemperatureDelta Parse(string s)
        {
            return DefaultParsers.Value.Process(s);
        }

        public static readonly TemperatureDelta Kelvin, Fahrenheit, Celsius;
        static TemperatureDelta()
        {
            Kelvin = new TemperatureDelta(1);
            Celsius = Kelvin;
            Fahrenheit = new TemperatureDelta(9 / 5.0);
            _udic = new Dictionary<string, Tuple<IDeltaUnit<TemperatureDelta>, string>>(3)
            {
                ["K"] = Tuple.Create<IDeltaUnit<TemperatureDelta>, string>(Kelvin, "K"),
                ["F"] = Tuple.Create<IDeltaUnit<TemperatureDelta>, string>(Fahrenheit, "F"),
                ["C"] = Tuple.Create<IDeltaUnit<TemperatureDelta>, string>(Celsius, "C")
            };
            DefaultParsers = new Lazy<Funnel<string, TemperatureDelta>>(() => new Funnel<string, TemperatureDelta>(
                new Parser<TemperatureDelta>($@"^({CommonRegex.RegexDouble}) ?(k|kelvin)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Kelvin)),
                new Parser<TemperatureDelta>($@"^({CommonRegex.RegexDouble}) ?(f|fahrenheit)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Fahrenheit)),
                new Parser<TemperatureDelta>($@"^({CommonRegex.RegexDouble}) ?(c|celsius)$", m => new TemperatureDelta(double.Parse(m.Groups[1].Value), Celsius))
                ));
        }
        public static TemperatureDelta operator -(TemperatureDelta a)
        {
            return (-1.0 * a);
        }
        public static TemperatureDelta operator *(TemperatureDelta a, double b)
        {
            return new TemperatureDelta(a.Arbitrary * b);
        }
        public static TemperatureDelta operator /(TemperatureDelta a, double b)
        {
            return a * (1 / b);
        }
        public static TemperatureDelta operator *(double b, TemperatureDelta a)
        {
            return a * b;
        }
        public static TemperatureDelta operator +(TemperatureDelta a, TemperatureDelta b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new TemperatureDelta(c);
        }
        public static TemperatureDelta operator -(TemperatureDelta a, TemperatureDelta b)
        {
            return a + (-b);
        }
        public static BigRational operator /(TemperatureDelta a, TemperatureDelta b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static readonly IDictionary<string, Tuple<IDeltaUnit<TemperatureDelta>, string>> _udic;
        public IDictionary<string, Tuple<IDeltaUnit<TemperatureDelta>, string>> deltaDictionary => _udic;
        //accepted formats (K|F|C)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromDeltaDictionary(format, "C", formatProvider, deltaDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as TemperatureDelta;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
}
