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

        public override string ToString()
            => string.Format(
                    @"<path{0}{1}{2}/>",
                    string.IsNullOrWhiteSpace(Style) ? null : $@" style=""{Style}""",
                    PathLength > 0 ? null : $@" pathLength=""{PathLength}""",
                    Data == null ? null : $@" d=""{Data.ToString()}"""
                );
    }
}
