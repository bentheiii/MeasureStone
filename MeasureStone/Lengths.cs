using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace MeasureStone
{
    //arbitrary is meters
    public class Length : IUnit<Length>, ScaleMeasurement<Length>, DeltaMeasurement<Length>, IComparable<Length>
    {
        public Length(BigRational val, IUnit<Length> unit) : this(unit.ToArbitrary(val)) { }
        public Length(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        BigRational ScaleMeasurement<Length>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        BigRational DeltaMeasurement<Length>.Arbitrary
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
        public int CompareTo(Length other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }

        private static readonly Lazy<Funnel<string, Length>> DefaultParsers;
        public static Length Parse(string s)
        {
            return DefaultParsers.Value.Process(s);
        }

        public static readonly Length Meter, CentiMeter, MilliMeter, KiloMeter, Foot, Yard, Mile, LightSecond, LightYear, Parsec, AstronomicalUnit;
        static Length()
        {
            Meter = new Length(1);
            CentiMeter = new Length(0.01);
            MilliMeter = new Length(0.001);
            KiloMeter = new Length(1000);
            Foot = new Length(0.3048);
            Yard = new Length(3, Foot);
            Mile = new Length(1760, Yard);
            LightSecond = new Length(299792458);
            LightYear = new Length(9460730472580800);
            AstronomicalUnit = new Length(149597870700);
            Parsec = new Length(648000 / Math.PI, AstronomicalUnit);
            _udic = new Dictionary<string, Tuple<IUnit<Length>, string>>(11)
            {
                ["M"] = Tuple.Create<IUnit<Length>, string>(Meter, "m"),
                ["CM"] = Tuple.Create<IUnit<Length>, string>(CentiMeter, "cm"),
                ["MM"] = Tuple.Create<IUnit<Length>, string>(MilliMeter, "mm"),
                ["KM"] = Tuple.Create<IUnit<Length>, string>(KiloMeter, "km"),
                ["F"] = Tuple.Create<IUnit<Length>, string>(Foot, "f"),
                ["Y"] = Tuple.Create<IUnit<Length>, string>(Yard, "yd"),
                ["MI"] = Tuple.Create<IUnit<Length>, string>(Mile, "mi"),
                ["LS"] = Tuple.Create<IUnit<Length>, string>(LightSecond, "ls"),
                ["LY"] = Tuple.Create<IUnit<Length>, string>(LightYear, "ly"),
                ["P"] = Tuple.Create<IUnit<Length>, string>(Parsec, "p"),
                ["AU"] = Tuple.Create<IUnit<Length>, string>(AstronomicalUnit, "au")
            };
            DefaultParsers = new Lazy<Funnel<string, Length>>(() => new Funnel<string, Length>(
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(m|meters?)$", m => new Length(double.Parse(m.Groups[1].Value), Meter)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(cm|centimeters?)$", m => new Length(double.Parse(m.Groups[1].Value), CentiMeter)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(mm|millimeters?)$", m => new Length(double.Parse(m.Groups[1].Value), MilliMeter)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(km|kilometers?)$", m => new Length(double.Parse(m.Groups[1].Value), KiloMeter)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(ft|foot|feet)$", m => new Length(double.Parse(m.Groups[1].Value), Foot)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(yd|yards?)$", m => new Length(double.Parse(m.Groups[1].Value), Yard)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(mi|miles?)$", m => new Length(double.Parse(m.Groups[1].Value), Mile)),
                new Parser<Length>($@"^({CommonRegex.RegexDouble}) ?(au|astronomical units?)$",
                    m => new Length(double.Parse(m.Groups[1].Value), AstronomicalUnit))
                ));
        }
        public static Length operator -(Length a)
        {
            return (-1.0 * a);
        }
        public static Length operator *(Length a, double b)
        {
            return new Length(a.Arbitrary * b);
        }
        public static Length operator /(Length a, double b)
        {
            return a * (1 / b);
        }
        public static Length operator *(double b, Length a)
        {
            return a * b;
        }
        public static Length operator +(Length a, Length b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new Length(c);
        }
        public static Length operator -(Length a, Length b)
        {
            return a + (-b);
        }
        public static BigRational operator /(Length a, Length b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static readonly IDictionary<string, Tuple<IUnit<Length>, string>> _udic;
        public override IDictionary<string, Tuple<IUnit<Length>, string>> unitDictionary => _udic;
        //accepted formats (M|CM|MM|KM|F|Y|MI|LS|LY|P|AU)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "M", formatProvider, scaleDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as Length;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
}