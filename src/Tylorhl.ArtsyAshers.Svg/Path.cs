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
    }
}
