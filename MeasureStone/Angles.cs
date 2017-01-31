using System;
using System.Collections.Generic;
using NumberStone;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units.RotationalSpeeds;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace WhetStone.Units.Angles
{
    /// <summary>
    /// Represents a 2D, cartesian angle.
    /// </summary>
    /// <remarks>
    /// <para>Arbitrary unit is radians.</para>
    /// <para>In most conventions, 0 means aligned with the x axis.</para>
    /// <para>This class is immutable.</para>
    /// </remarks>
    public class Angle : IUnit<Angle>, ScaleMeasurement<Angle>, DeltaMeasurement<Angle>, IComparable<Angle>
    {
        /// <summary>
        /// Constructor for <see cref="Angle"/>
        /// </summary>
        /// <param name="val">The value of the angle, in <paramref name="unit"/>s.</param>
        /// <param name="unit">The unit in which <paramref name="val"/> is expressed.</param>
        /// <param name="normalize">If set to <see langword="true"/>, the value of the angle is set to be between 0 and 360 degrees (outer exclusive), using  modulo arithmatic.</param>
        public Angle(BigRational val, IUnit<Angle> unit, bool normalize = false) : this(unit.ToArbitrary(val), normalize) { }
        /// <summary>
        /// Constructor for <see cref="Angle"/>
        /// </summary>
        /// <param name="arbitrary">The value of the angle, in radianss.</param>
        /// <param name="normalize">If set to <see langword="true"/>, the value of the angle is set to be between 0 and 360 degrees (outer exclusive), using  modulo arithmatic.</param>
        public Angle(BigRational arbitrary, bool normalize = false)
        {
            if (normalize)
                arbitrary = arbitrary.TrueMod(2 * Math.PI);
            this.Arbitrary = arbitrary;
        }
        /// <summary>
        /// The arbitrary value of the <see cref="Angle"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Angle"/>'s value in radians.
        /// </value>
        public BigRational Arbitrary { get; }
        BigRational ScaleMeasurement<Angle>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        BigRational DeltaMeasurement<Angle>.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        /// <summary>
        /// <see cref="IUnit{T}.FromArbitrary"/>
        /// </summary>
        public override BigRational FromArbitrary(BigRational arb)
        {
            return arb / Arbitrary;
        }
        /// <summary>
        /// <see cref="IUnit{T}.ToArbitrary"/>
        /// </summary>
        public override BigRational ToArbitrary(BigRational val)
        {
            return val * Arbitrary;
        }
        /// <summary>
        /// Compares the <see cref="Angle"/> to another <see cref="Angle"/> by their <see cref="Arbitrary"/> value.
        /// </summary>
        /// <param name="other">The other <see cref="Angle"/> to compare to.</param>
        /// <returns>a negative value if this is lower, positive if this is higher, or 0 if they are equal.</returns>
        public int CompareTo(Angle other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }
        /// <summary>
        /// Normalizes an <see cref="Angle"/>, setting it between 0 and 1 full turns.
        /// </summary>
        /// <returns>A normalized <see cref="Angle"/>.</returns>
        public Angle Normalize()
        {
            return Normalized ? this : new Angle(Arbitrary, true);
        }
        /// <summary>
        /// Whether the <see cref="Angle"/> is normalized (is between 0 and 1 full turns).
        /// </summary>
        public bool Normalized => Arbitrary.iswithinPartialExclusive(0, 2 * Math.PI);
        /// <summary>
        /// Creates a new <see cref="Angle"/> through the arcsine function.
        /// </summary>
        /// <param name="x">The input for the arcsine function.</param>
        /// <returns>A new <see cref="Angle"/>, whose value is the result of asin(<paramref name="x"/>).</returns>
        public static Angle ASin(double x) => new Angle(Math.Asin(x));
        /// <summary>
        /// Creates a new <see cref="Angle"/> through the arccosine function.
        /// </summary>
        /// <param name="x">The input for the arccosine function.</param>
        /// <returns>A new <see cref="Angle"/>, whose value is the result of acos(<paramref name="x"/>).</returns>
        public static Angle ACos(double x) => new Angle(Math.Acos(x));
        /// <summary>
        /// Creates a new <see cref="Angle"/> through the arctangent function.
        /// </summary>
        /// <param name="x">The input for the arctangent function.</param>
        /// <returns>A new <see cref="Angle"/>, whose value is the result of atan(<paramref name="x"/>).</returns>
        public static Angle ATan(double x) => new Angle(Math.Atan(x));
        /// <summary>
        /// Creates a new <see cref="Angle"/> through the arctangent2 function.
        /// </summary>
        /// <param name="y">The y input for the arctangent function.</param>
        /// <param name="x">The x input for the arctangent function.</param>
        /// <returns>A new <see cref="Angle"/>, whose value is the result of atan2(<paramref name="x"/>).</returns>
        public static Angle ATan(double y, double x)
        {
            var r = Math.Atan2(y, x);
            return new Angle(r, true);
        }
        private static readonly Funnel<string, Angle> DefaultParsers =new Funnel<string, Angle>(
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(turns?|t)$", m => new Angle(double.Parse(m.Groups[1].Value), Turn)),
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(°|degrees?|d)$", m => new Angle(double.Parse(m.Groups[1].Value), Degree)),
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(rad|㎭|radians?|c|r)$", m => new Angle(double.Parse(m.Groups[1].Value), Radian)),
                new Parser<Angle>($@"^({CommonRegex.RegexDouble}) ?(grad|g|gradians?|gon)$", m => new Angle(double.Parse(m.Groups[1].Value), Gradian))
                );
        /// <summary>
        /// Parses a string to an <see cref="Angle"/>.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to parse.</param>
        /// <returns>The parsed <see cref="Angle"/>.</returns>
        /// <exception cref="NoValidProcessorException">If </exception>
        public static Angle Parse(string s)
        {
            return DefaultParsers.Process(s);
        }
        /// <summary>
        /// The Radian unit, 1/2PI of a full turn.
        /// </summary>
        public static readonly Angle Radian = new Angle(1);
        /// <summary>
        /// The Degree unit, 1/360 of a full turn.
        /// </summary>
        public static readonly Angle Degree = new Angle(Math.PI / 180);
        /// <summary>
        /// The Turn unit, 1 full turn.
        /// </summary>
        /// <remarks>This is the only standard <see cref="Angle"/> unit that is not <see cref="Normalized"/>.</remarks>
        public static readonly Angle Turn = new Angle(2 * Math.PI);
        /// <summary>
        /// The Gradian unit, 1/400 of a full turn.
        /// </summary>
        public static readonly Angle Gradian = new Angle(Math.PI / 200);
        /// <summary>
        /// The Right Angle unit, 1/4 of a full turn.
        /// </summary>
        public static readonly Angle RightAngle = new Angle(Math.PI / 2);
        public static RotationalSpeed operator /(Angle a, TimeSpan b)
        {
            return new RotationalSpeed(a.Arbitrary / b.TotalSeconds);
        }
        /// <summary>
        /// Negates the <see cref="Angle"/>, returning an angle of the same magnitude, but in the other direction.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Angle operator -(Angle a)
        {
            return (-1.0 * a);
        }
        public static Angle operator *(Angle a, double b)
        {
            return new Angle(a.Arbitrary * b);
        }
        public static Angle operator /(Angle a, double b)
        {
            return a * (1 / b);
        }
        public static Angle operator *(double b, Angle a)
        {
            return a * b;
        }
        public static Angle operator +(Angle a, Angle b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new Angle(c);
        }
        public static Angle operator -(Angle a, Angle b)
        {
            return a + (-b);
        }
        public static BigRational operator /(Angle a, Angle b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        private static readonly IDictionary<string, Tuple<IUnit<Angle>, string>> _udic = new Dictionary<string, Tuple<IUnit<Angle>, string>>(4)
        {
            ["R"] = Tuple.Create<IUnit<Angle>, string>(Radian, "rad"),
            ["D"] = Tuple.Create<IUnit<Angle>, string>(Degree, "\u00b0"),
            ["G"] = Tuple.Create<IUnit<Angle>, string>(Gradian, "gon"),
            ["T"] = Tuple.Create<IUnit<Angle>, string>(Turn, "\u03c4")
        };
        public override IDictionary<string, Tuple<IUnit<Angle>, string>> unitDictionary => _udic;
        //accepted formats (R|D|G|T)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.StringFromUnitDictionary(format, "R", formatProvider, scaleDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as Angle;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
}
