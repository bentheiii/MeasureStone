using System;
using System.Collections.Generic;
using Numerics;
using WhetStone.Funnels;
using WhetStone.Units;
using WhetStone.WordPlay;
using WhetStone.WordPlay.Parsing;

namespace MeasureStone
{
    //arbitrary is megabyte
    public class DataSize : IUnit<DataSize>, ScaleMeasurement, DeltaMeasurement, IComparable<DataSize>
    {
        public DataSize(BigRational val, IUnit<DataSize> unit) : this(unit.ToArbitrary(val)) { }
        public DataSize(BigRational arbitrary)
        {
            this.Arbitrary = arbitrary;
        }
        public BigRational Arbitrary { get; }
        BigRational ScaleMeasurement.Arbitrary
        {
            get
            {
                return this.Arbitrary;
            }
        }
        BigRational DeltaMeasurement.Arbitrary
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
        public int CompareTo(DataSize other)
        {
            return Arbitrary.CompareTo(other.Arbitrary);
        }

        private static readonly Lazy<Funnel<string, DataSize>> DefaultParsers;
        public static DataSize Parse(string s)
        {
            return DefaultParsers.Value.Process(s);
        }

        public static readonly DataSize Bit, Byte, 
            Kibibyte, Mebibyte, Gibibyte, Tebibyte, Pebibyte, Exbibyte, Zebibyte, Yobibyte,
            Kilobyte, Megabyte, Gigabyte, Terrabyte, Pettabyte, Exabyte, Zettabyte, Yottabyte,
            Kibibit, Mebibit, Gibibit, Tebibit, Pebibit, Exbibit, Zebibit, Yobibit,
            Kilobit, Megabit, Gigabit, Terrabit, Pettabit, Exabit, Zettabit, Yottabit;
        static DataSize()
        {
            Mebibyte = new DataSize(1);
            Gibibyte = Mebibyte * 1024;
            Tebibyte = Gibibyte * 1024;
            Pebibyte = Tebibyte * 1024;
            Exbibyte = Pebibyte * 1024;
            Zebibyte = Exbibyte * 1024;
            Yobibyte = Zebibyte * 1024;
            
            Kibibyte = Mebibyte / 1024;
            Byte = Kibibyte / 1024;
            Bit = Byte / 8;

            Kibibit = Kilobyte/8;
            Mebibit = Mebibyte/8;
            Gibibit = Gibibyte/8;
            Tebibit = Tebibyte/8;
            Pebibit = Pebibyte/8;
            Exbibit = Exbibyte/8;
            Zebibit = Zebibyte/8;
            Yobibit = Yobibyte/8;

            Kilobyte = Byte*1000;
            Megabyte = Kilobyte*1000;
            Gigabyte = Megabyte*1000;
            Terrabyte = Gigabyte*1000;
            Pettabyte = Terrabyte*1000;
            Exabyte = Pettabyte*1000;
            Zettabyte = Exabyte*1000;
            Yottabyte = Zettabyte*1000;

            Kilobit = Kilobyte/8;
            Megabit = Megabyte/8;
            Gigabit = Gigabyte/8;
            Terrabit = Terrabyte/8;
            Pettabit = Pettabyte/8;
            Exabit = Exabyte/8;
            Zettabit = Zettabyte/8;
            Yottabit = Yottabyte/8;

            DefaultParsers = new Lazy<Funnel<string, DataSize>>(() => new Funnel<string, DataSize>(
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(b|bits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Bit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(B|bytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Byte)),
                //binary, bytes
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(KiB|kibibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Kibibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(MiB|mebibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Mebibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(GiB|gibibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Gibibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(TiB|tebibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Tebibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(PiB|pebibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Pebibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(EiB|exbibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Exbibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(ZiB|zebibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Zebibyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(YiB|yobibytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Yobibyte)),
                //binary, bits
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Kib|kibibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Kibibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Mib|mebibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Mebibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Gib|gibibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Gibibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Tib|tebibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Tebibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Pib|pebibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Pebibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Eib|exbibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Exbibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Zib|zebibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Zebibit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Yib|yobibits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Yobibit)),
                //decimal, bytes
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(KB|kilobytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Kilobyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(MB|megabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Megabyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(GB|gigabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Gigabyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(TB|terrabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Terrabyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(PB|pettabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Pettabyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(EB|exabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Exabyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(ZB|zettabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Zettabyte)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(YB|yottabytes?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Yottabyte)),
                //decimal, bits
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Kb|kilobits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Kilobit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Mb|megabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Megabit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Gb|gigabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Gigabit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Tb|terrabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Terrabit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Pb|pettabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Pettabit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Eb|exabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Exabit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Zb|zettabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Zettabit)),
                new Parser<DataSize>($@"^({CommonRegex.RegexDouble}) ?(Yb|yottabits?)$", m => new DataSize(double.Parse(m.Groups[1].Value), Yottabit))
                ));
        }
        public static DataSize operator -(DataSize a)
        {
            return (-1.0 * a);
        }
        public static DataSize operator *(DataSize a, double b)
        {
            return new DataSize(a.Arbitrary * b);
        }
        public static DataSize operator /(DataSize a, double b)
        {
            return a * (1 / b);
        }
        public static DataSize operator *(double b, DataSize a)
        {
            return a * b;
        }
        public static DataSize operator +(DataSize a, DataSize b)
        {
            var c = a.Arbitrary + b.Arbitrary;
            return new DataSize(c);
        }
        public static DataSize operator -(DataSize a, DataSize b)
        {
            return a + (-b);
        }
        public static BigRational operator /(DataSize a, DataSize b)
        {
            return a.Arbitrary / b.Arbitrary;
        }
        public override string ToString()
        {
            return this.ToString("");
        }
        //accepted formats (b|B|K|M|G|T|P|E|Z|Y)_{double format}_{symbol}
        public string ToString(string format, IFormatProvider formatProvider)
        {
            IDictionary<string, Tuple<IScaleUnit<DataSize>, string>> unitDictionary = new Dictionary<string, Tuple<IScaleUnit<DataSize>, string>>(11);
            unitDictionary["b"] = Tuple.Create<IScaleUnit<DataSize>, string>(Bit, "b");
            unitDictionary["B"] = Tuple.Create<IScaleUnit<DataSize>, string>(Byte, "B");
            unitDictionary["K"] = Tuple.Create<IScaleUnit<DataSize>, string>(Kibibyte, "KB");
            unitDictionary["M"] = Tuple.Create<IScaleUnit<DataSize>, string>(Mebibyte, "MB");
            unitDictionary["G"] = Tuple.Create<IScaleUnit<DataSize>, string>(Gibibyte, "GB");
            unitDictionary["T"] = Tuple.Create<IScaleUnit<DataSize>, string>(Tebibyte, "TB");
            unitDictionary["P"] = Tuple.Create<IScaleUnit<DataSize>, string>(Pebibyte, "PB");
            unitDictionary["E"] = Tuple.Create<IScaleUnit<DataSize>, string>(Exbibyte, "EB");
            unitDictionary["Z"] = Tuple.Create<IScaleUnit<DataSize>, string>(Zebibyte, "ZB");
            unitDictionary["Y"] = Tuple.Create<IScaleUnit<DataSize>, string>(Yobibyte, "YB");
            return this.StringFromUnitDictionary(format, "M", formatProvider, unitDictionary);
        }
        public override int GetHashCode()
        {
            return Arbitrary.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var an = obj as DataSize;
            return an != null && an.Arbitrary == this.Arbitrary;
        }
    }
}
