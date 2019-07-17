using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Tylorhl.ArtsyAshers.Svg.PathElement;

namespace Tylorhl.ArtsyAshers.Svg
{
    public class Path
    {
        public PathData Data { get; set; }

        public string Style { get; set; } = "";

        public double PathLength { get; set; }

        public string Id { get; set; }

        public bool Transitonary { get; set; } = false;

        public double Duration { get; set; } = 0;

        public Path() { }

        public Path(string data, string style = default, double pathLength = default)
        {
            Data = new PathData(data);
            Style = style;
            PathLength = pathLength;
        }

        public override string ToString()
            => string.Format(
                    @"<path{0}{1}{2}/>",
                    string.IsNullOrWhiteSpace(Style) ? null : $@" style=""{Style}""",
                    PathLength > 0 == false ? null : $@" pathLength=""{PathLength}""",
                    Data == null ? null : $@" d=""{string.Join("", Data.ToString())}"""
                );

        public static Path Concat(params Path[] paths)
        {
            string combinedPath = string.Concat(paths.Select(p => p.Data.ToString()));

            var newDur = paths.Sum(p => p.Duration);

            return new Path(combinedPath) { Duration = newDur };
        }
    }
}
