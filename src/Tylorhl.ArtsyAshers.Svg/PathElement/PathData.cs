using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tylorhl.ArtsyAshers.Svg.PathElement.Commands;

namespace Tylorhl.ArtsyAshers.Svg.PathElement
{
    public class PathData
    {
        private static readonly ObjectPool<StringBuilder> sbPool = new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());

        private static readonly Regex CommandSplit = new Regex(@"(?=[MLHVCSQTAZ])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private List<PathCommand> commands = new List<PathCommand>();

        private string formattedString = null;

        public PathData(string data)
        {
            var cmds = CommandSplit.Split(data);

            // i = 1 to avoid the empty first value
            for (int i = 1; i < cmds.Length; i++)
                this.commands.Add(PathCommand.Create(cmds[i]));

            // Since he commands collection is immutable from the outside
            // create the formatted string ahead of time with command shorthand
            char lastCmd = default;

            var sb = sbPool.Get();

            try
            {
                bool calculateEnd = commands.LastOrDefault().ParameterCount != 0;

                if (!calculateEnd)
                    EndingPoint = StartingPoint;

                for (int i = 0; i < commands.Count; i++)
                {
                    if(lastCmd != commands[i].CommandIdentifier)
                    {
                        lastCmd = commands[i].CommandIdentifier;
                        sb.Append(lastCmd);
                        sb.AppendFormat("{0}", commands[i].ValueString);
                    }
                    else
                        sb.AppendFormat(" {0}", commands[i].ValueString);

                    if(calculateEnd)
                    {
                        EndingPoint = commands[i].PointFromPoint(EndingPoint);
                    }
                }

                formattedString = sb.ToString();
            }
            finally
            {
                sbPool.Return(sb);
            }
        }

        public IReadOnlyList<PathCommand> Commands => commands.AsReadOnly();

        public PointF StartingPoint => Commands[0].StartingPoint;

        public PointF EndingPoint { get; set; } = new PointF(0, 0);

        public override string ToString() => formattedString;

        public static PathData operator -(PathData cmd1, PathData cmd2)
        {
            return new PathData($"M{cmd2.EndingPoint.X},{cmd2.EndingPoint.Y}L{cmd1.StartingPoint.X},{cmd1.StartingPoint.Y}");
        }
    }
}
