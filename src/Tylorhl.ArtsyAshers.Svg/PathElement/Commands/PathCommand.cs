using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tylorhl.ArtsyAshers.Svg.PathElement.Commands
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
            ["m"] = s => new M(s),
            ["l"] = s => new L(s),
            ["h"] = s => new H(s),
            ["v"] = s => new V(s),
            ["c"] = s => new C(s),
            ["s"] = s => new S(s),
            ["q"] = s => new Q(s),
            ["t"] = s => new T(s),
            ["a"] = s => new A(s),
            ["z"] = s => new Z(s),
        };

        private float[] values;

        private static readonly Regex ValueSplit = new Regex(@"[, ]+", RegexOptions.Compiled);

        protected PathCommand() {}

        private PathCommand(in string commandString)
        {
            if (commandString.Length == 1)
            {
                CommandIdentifier = commandString[0];
                return;
            }

            (CommandIdentifier, values) = ParseValues(commandString);

            if (values.Length % ParameterCount != 0)
                throw new ArgumentException($"Command '{CommandIdentifier}' received {values.Length} parameters while expecting a multiple of {ParameterCount}.");
        }

        public ReadOnlySpan<float> this[int i] => Values.Slice(i * ParameterCount, ParameterCount);

        public abstract int ParameterCount { get; }

        public char CommandIdentifier { get; private set; }

        public bool IsAbsolute => char.IsUpper(CommandIdentifier);

        public abstract string Format { get; }

        public virtual PointF StartingPoint => new PointF(this[0][ParameterCount - 2], this[0][ParameterCount - 1]);

        public virtual PointF EndingPoint => new PointF(this[values.Length / ParameterCount - 1][ParameterCount - 2], this[values.Length / ParameterCount - 1][ParameterCount - 1]);

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
                ValueSplit.Split(commandString.Substring(1), 0, 1).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => float.Parse(s)).ToArray()
            );

        // Short way of getting the final point given an absolute point as the start and a PathCommand as the destination
        public PointF PointFromPoint(PointF absolutePoint)
        {
            if(this.IsAbsolute)
            {
                return this.StartingPoint;
            }

            return new PointF(absolutePoint.X + this.StartingPoint.X, absolutePoint.Y + this.StartingPoint.Y);
        }

        public void Translate(PointF point)
        {
            if (!IsAbsolute)
                return;

            var values = new Span<float>(this.values);
            for (int i = 0; i < values.Length; i += ParameterCount)
            {
                Translate(point, values.Slice(i, ParameterCount));
            }
        }

        protected abstract void Translate(PointF point, Span<float> commandParams);

        private string FormatJoin(string format)
        {
            var sb = sbPool.Get();

            try
            {
                for (int i = 0; i < Values.Length; i += ParameterCount)
                {
                    sb.Append(string.Format($" {format}", args: Values.Slice(i, ParameterCount).ToArray().Cast<object>().ToArray()));
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

            protected override void Translate(PointF point, Span<float> commandParams)
            {
                commandParams[0] = commandParams[0] + point.X;
                commandParams[1] = commandParams[1] + point.Y;
            }
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
            }

            public override PointF StartingPoint => new PointF(Values[0], 0);

            public override PointF EndingPoint => new PointF(Values[Values.Length - 1], 0);

            public override int ParameterCount => 1;

            public override string Format => @"{0}";

            protected override void Translate(PointF point, Span<float> commandParams)
            {
                commandParams[0] = commandParams[0] + point.X;
            }
        }

        private class V : H
        {
            public V(string commandString)
                : base(commandString)
            {
            }

            public override PointF StartingPoint => new PointF(0, Values[0]);

            public override PointF EndingPoint => new PointF(0, Values[Values.Length - 1]);

            protected override void Translate(PointF point, Span<float> commandParams)
            {
                commandParams[0] = commandParams[0] + point.Y;
            }
        }

        private class C : PathCommand
        {
            public C(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 6;

            public override string Format => @"{0},{1} {2},{3} {4},{5}";

            protected override void Translate(PointF point, Span<float> commandParams)
            {
                for(int i = 0; i < commandParams.Length; i += 2)
                {
                    commandParams[i] = commandParams[i] + point.X;
                    commandParams[i+1] = commandParams[i+1] + point.Y;
                }
            }
        }

        private class S : PathCommand
        {
            public S(string commandString)
                : base(commandString) { }

            public override int ParameterCount => 4;

            public override string Format => @"{0},{1} {2},{3}";

            protected override void Translate(PointF point, Span<float> commandParams)
            {
                for (int i = 0; i < commandParams.Length; i += 2)
                {
                    commandParams[i] = commandParams[i] + point.X;
                    commandParams[i + 1] = commandParams[i + 1] + point.Y;
                }
            }
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

            protected override void Translate(PointF point, Span<float> commandParams)
            {
                commandParams[5] = commandParams[5] + point.X;
                commandParams[6] = commandParams[6] + point.Y;
            }
        }

        private class Z : PathCommand
        {
            public Z(string commandString)
                : base(commandString) { }

            public override PointF StartingPoint => default;

            public override PointF EndingPoint => default;

            public override int ParameterCount => 0;

            public override string Format => @"";

            protected override void Translate(PointF point, Span<float> commandParams)
            { }
        }
    }
}
