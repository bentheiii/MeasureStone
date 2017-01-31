using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace MeasureStone
{
    public class GraphicsLength : IUnit<GraphicsLength>, ScaleMeasurement<GraphicsLength>, DeltaMeasurement<GraphicsLength>, IComparable<GraphicsLength>
    {
        public GraphicsLength(BigRational val, IDeltaUnit<GraphicsLength> unit) : this(unit.ToArbitrary(val)) { }
        public GraphicsLength(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        public int CompareTo(GraphicsLength other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }
        BigRational DeltaMeasurement<GraphicsLength>.Arbitrary
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

        private static readonly Lazy<Funnel<string, GraphicsLength>> DefaultParsers;
        public static GraphicsLength Parse(string s)
        {
            return DefaultParsers.Value.Process(s);
        }

        public static readonly GraphicsLength Pixel;
        static GraphicsLength()
        {
            Pixel = new GraphicsLength(1);
            _udic = new Dictionary<string, Tuple<IUnit<GraphicsLength>, string>>(1)
            {
                ["P"] = Tuple.Create<IUnit<GraphicsLength>, string>(Pixel, "P")
            };
            DefaultParsers = new Lazy<Funnel<string, GraphicsLength>>(() => new Funnel<string, GraphicsLength>(
                new Parser<GraphicsLength>($@"^({CommonRegex.RegexDouble}) ?(p|pixels?)$", m => new GraphicsLength(double.Parse(m.Groups[1].Value), Pixel))
                ));
        }
        public static GraphicsLength operator -(GraphicsLength a)
        {
            return (-1.0 * a);
        }
        public static GraphicsLength operator *(GraphicsLength a, double b)
        {
            return new GraphicsLength(a.Arbitrary * b);
        }
        public static GraphicsLength operator /(GraphicsLength a, double b)
        {
            return a * (1 / b);
        }
        public static GraphicsLength operator *(double b, GraphicsLength a)
        {
            return a * b;
        }
        public static GraphicsLength operator +(GraphicsLength a, GraphicsLength b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new GraphicsLength(c);
        }
        public static GraphicsLength operator -(GraphicsLength a, GraphicsLength b)
        {
            return a + (-b);
        }
        public static BigRational operator /(GraphicsLength a, GraphicsLength b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static  readonly IDictionary<string, Tuple<IUnit<GraphicsLength>, string>> _udic;
        public override IDictionary<string, Tuple<IUnit<GraphicsLength>, string>> unitDictionary => _udic;
        //accepted formats (P)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "P", formatProvider, scaleDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as GraphicsLength;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
}
