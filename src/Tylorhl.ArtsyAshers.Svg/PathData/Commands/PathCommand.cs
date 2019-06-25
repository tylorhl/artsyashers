using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tylorhl.ArtsyAshers.Svg.PathData.Commands
{
    public abstract class PathCommand
    {
        private static readonly Dictionary<char, int> DefinedPathCommands = new Dictionary<char, int>()
        {
            ['M'] = 2,
            ['L'] = 2,
            ['H'] = 1,
            ['V'] = 1,
            ['C'] = 6,
            ['S'] = 4,
            ['Q'] = 4,
            ['T'] = 2,
            ['A'] = 7,
            ['Z'] = 0,
        };

        public abstract int ParameterCount { get; }

        private static readonly Regex ValueSplit = new Regex(@"[, ]+", RegexOptions.Compiled);

        public PathCommand(string commandString)
        {
            if (IsCommand(commandString))
            {
                Values = ValueSplit.Split(commandString, int.MaxValue, 1).Cast<float>().ToArray();
            }
        }

        public char CommandIdentifier => char.ToUpperInvariant(GetType().Name[0]);

        public bool IsAbsolute => char.IsUpper(CommandIdentifier);

        public static bool IsCommand(in string command) 
            => command != null && command.Length > 0 ? DefinedPathCommands.ContainsKey(char.ToUpperInvariant(command[0])) : false;

        public abstract string Format { get; }

        public virtual PointF StartingPoint { get; protected set; }

        public virtual PointF EndingPoint { get; protected set; }

        public virtual IList<float> Values { get; protected set; }

        public override string ToString()
                => $@"{(IsAbsolute ? CommandIdentifier : char.ToLowerInvariant(CommandIdentifier))}{FormatJoin(Format)}";

        protected string FormatJoin(string format)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Values.Count; i += ParameterCount)
            {
                Values.Skip(i).Take(ParameterCount);
                sb.Append(string.Format(format, args: Values));
            }

            return sb.ToString();
        }

        private class M : PathCommand
        {
            public M(string commandString)
                : base(commandString)
                => StartingPoint = EndingPoint = new PointF(Values[0], Values[1]);

            public override int ParameterCount => 2;

            public override string Format => @"{0},{1}";
        }

        private class L : M
        {
            public L(string commandString)
                : base(commandString)
                => StartingPoint = EndingPoint = new PointF(Values[0], Values[1]);
        }

        private class H : PathCommand
        {
            public H(string commandString)
                : base(commandString)
                => StartingPoint = EndingPoint = new PointF(Values[0], 0);

            public override int ParameterCount => 1;

            public override string Format => @"{0}";
        }

        private class V : H
        {
            public V(string commandString)
                : base(commandString)
                => StartingPoint = EndingPoint = new PointF(0, Values[0]);
        }

        // TODO: Resume work from here
        private class C : PathCommand
        {
            public C(string commandString)
                : base(commandString)
            {

            }

            public override int ParameterCount => 6;

            public override string Format => @"{0},{1} {2},{3} {4},{5}";
        }

        private class S : PathCommand
        {
            public S(string commandString)
                : base(commandString)
            {

            }

            public override int ParameterCount => 4;

            public override string Format => @"{0},{1} {2},{3}";
        }

        private class Q : S
        {
            public Q(string commandString)
                : base(commandString)
            {

            }
        }

        private class T : M
        {
            public T(string commandString)
                : base(commandString)
            {

            }
        }

        private class A : PathCommand
        {
            public A(string commandString)
                : base(commandString)
            {

            }

            public override int ParameterCount => 7;

            public override string Format => @"{0} {1} {2} {3} {4} {5},{6}";
        }

        private class Z : PathCommand
        {
            public Z(string commandString)
                : base(commandString)
            { }

            public override int ParameterCount => 0;

            public override string Format => @"";
        }
    }
}
