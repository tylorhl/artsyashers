using Microsoft.Extensions.ObjectPool;
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
        private static readonly ObjectPool<StringBuilder> sbPool = new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());

        private static readonly Dictionary<string, Func<string, PathCommand>> DefinedPathCommands = new Dictionary<string, Func<string, PathCommand>>(StringComparer.InvariantCultureIgnoreCase)
        {
            ["M"] = s => new M(s),
            ["L"] = s => new L(s),
            ["H"] = s => new H(s),
            ["V"] = s => new V(s),
            ["C"] = s => new C(s),
            ["S"] = s => new S(s),
            ["Q"] = s => new Q(s),
            ["T"] = s => new T(s),
            ["A"] = s => new A(s),
            ["Z"] = s => new Z(s),
        };

        private float[] values;
        protected PointF startingPoint, endingPoint;

        private static readonly Regex ValueSplit = new Regex(@"[, ]+", RegexOptions.Compiled);

        private PathCommand(in string commandString)
        {
            (CommandIdentifier, values) = ParseValues(commandString);

            if (values.Length % ParameterCount != 0)
                throw new ArgumentException($"Command '{CommandIdentifier}' received {values.Length} while expecting a multiple of {ParameterCount}.");

            if(ParameterCount > 1)
            {
                var cmd = this[0];
                startingPoint = new PointF(cmd[ParameterCount - 2], cmd[ParameterCount - 1]);

                cmd = this[values.Length / ParameterCount - 1];
                endingPoint = new PointF(cmd[ParameterCount - 2], cmd[ParameterCount - 1]);
            }
        }

        public ReadOnlySpan<float> this[int i] => Values.Slice(i * ParameterCount, ParameterCount);

        public abstract int ParameterCount { get; }

        public char CommandIdentifier { get; private set; }

        public bool IsAbsolute => char.IsUpper(CommandIdentifier);

        public abstract string Format { get; }

        public virtual PointF StartingPoint => startingPoint;

        public virtual PointF EndingPoint => endingPoint;

        public ReadOnlySpan<float> Values => new ReadOnlySpan<float>(values);

        public static PathCommand Create(in string commandString)
        {
            if(string.IsNullOrWhiteSpace(commandString))
                throw new ArgumentException($"Invalid command string for {nameof(PathCommand)}");

            string cmd = commandString[0].ToString();

            if (!DefinedPathCommands.ContainsKey(cmd))
                throw new ArgumentException($"Unidentified path command '{cmd}'");

            return DefinedPathCommands[cmd](commandString);
        }

        public string ValueString => FormatJoin(Format);

        public override string ToString() => $@"{CommandIdentifier}{ValueString}";

        private static (char cmd, float[] values) ParseValues(string commandString)
            =>
            (
                commandString[0],
                ValueSplit.Split(commandString, 0, 1).Cast<float>().ToArray()
            );

        private string FormatJoin(string format)
        {
            var sb = sbPool.Get();

            try
            {
                for (int i = 0; i < Values.Length; i += ParameterCount)
                {
                    sb.Append(string.Format(format, args: Values.Slice(i, ParameterCount).ToArray()));
                }

                return sb.ToString();
            }
            finally
            {
                sbPool.Return(sb);
            }
        }

        private class M : PathCommand
        {
            public M(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 2;

            public override string Format => @"{0},{1}";
        }

        private class L : M
        {
            public L(string commandString)
                : base(commandString) { }
        }

        private class H : PathCommand
        {
            public H(string commandString)
                : base(commandString)
            {
                var vals = Values;
                startingPoint = new PointF(vals[0], 0);
                endingPoint = new PointF(vals[vals.Length - 1], 0);
            }

            public override int ParameterCount => 1;

            public override string Format => @"{0}";
        }

        private class V : H
        {
            public V(string commandString)
                : base(commandString)
            {
                var vals = Values;
                startingPoint = new PointF(0, vals[0]);
                endingPoint = new PointF(0, vals[vals.Length - 1]);
            }
        }

        private class C : PathCommand
        {
            public C(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 6;

            public override string Format => @"{0},{1} {2},{3} {4},{5}";
        }

        private class S : PathCommand
        {
            public S(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 4;

            public override string Format => @"{0},{1} {2},{3}";
        }

        private class Q : S
        {
            public Q(string commandString)
                : base(commandString) { }
        }

        private class T : M
        {
            public T(string commandString)
                : base(commandString) { }
        }

        private class A : PathCommand
        {
            public A(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 7;

            public override string Format => @"{0} {1} {2} {3} {4} {5},{6}";
        }

        private class Z : PathCommand
        {
            public Z(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 0;

            public override string Format => @"";
        }
    }
}
