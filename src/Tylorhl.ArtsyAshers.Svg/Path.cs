using System;
using System.Collections.Generic;
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
    }
}
