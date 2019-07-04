using System;
using System.Collections.Generic;
using System.Text;
using Tylorhl.ArtsyAshers.Svg.Shapes.Animation;

namespace Tylorhl.ArtsyAshers.Svg.Shapes
{
    public class Circle : ISvgShape
    {
        public double R { get; set; } = 0;
        public double CX { get; set; } = 0;
        public double CY { get; set; } = 0;
        public double PathLength { get; set; } = 0;

        public AnimateMotion AnimateMotion { get; set; }

        public override string ToString()
        {
            return $@"<circle r=""{R}"" cx=""{CX}"" cy=""{CY}"" {(AnimateMotion == null ? "/>" : ">" + AnimateMotion.ToString() + "</circle>")}";
        }
    }
}
