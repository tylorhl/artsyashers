using System;
using System.Collections.Generic;
using System.Text;

namespace Tylorhl.ArtsyAshers.Svg.Shapes.Animation
{
    public class AnimateMotion
    {
        public string Dur { get; set; }
        public MPathAttribute MPath { get; set; }
        public string RepeatCount { get; set; }

        public override string ToString()
            => $@"<animateMotion dur=""{Dur}"" repeatCount=""{RepeatCount}"" {(MPath == null ? "/>" : ">" + MPath.ToString() + "</animationMotion >")}";

        public class MPathAttribute
        {
            public string Href { get; set; }

            public override string ToString()
                => $@"<mpath xlink:href=""{Href}"" />";
        }
    }
}
