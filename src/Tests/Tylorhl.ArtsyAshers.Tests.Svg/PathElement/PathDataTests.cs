using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Tylorhl.ArtsyAshers.Svg.PathElement;

namespace Tylorhl.ArtsyAshers.Tests.Svg.PathElement
{
    [TestClass]
    public class PathDataTests
    {
        [TestMethod]
        [DataRow(null, typeof(ArgumentNullException))]
        [DataRow("", typeof(NullReferenceException))]
        [DataRow("M0,1z", null)]
        [DataRow("M0,1.0", null)]
        [DataRow("M0,1,", typeof(ArgumentException))]
        [DataRow("M0,1,1,0", null)]
        [DataRow("M0,1 1,0z", null)]
        [DataRow("M0,1 1,0 2", typeof(ArgumentException))]
        [DataRow("M0,1 1,0 2,", typeof(ArgumentException))]
        [DataRow("M0,1 1,0M", typeof(ArgumentOutOfRangeException))]
        [DataRow("M0,1 1,0M0,1z", null)]
        [DataRow("M 64.800781 0 A 64.800003 64.5 0 0 0 0 64.5 A 64.800003 64.5 0 0 0 64.800781 129 A 64.800003 64.5 0 0 0 87.107422 125.00586 A 111 111.2 0 0 0 69 185.69922 A 111 111.2 0 0 0 180 296.90039 A 111 111.2 0 0 0 291 185.69922 A 111 111.2 0 0 0 272.92188 124.94531 A 64.800003 64.5 0 0 0 295.30078 129 A 64.800003 64.5 0 0 0 360.09961 64.5 A 64.800003 64.5 0 0 0 295.30078 0 A 64.800003 64.5 0 0 0 230.5 64.5 A 64.800003 64.5 0 0 0 235.69727 89.636719 A 111 111.2 0 0 0 180 74.5 A 111 111.2 0 0 0 124.50781 89.552734 A 64.800003 64.5 0 0 0 129.59961 64.5 A 64.800003 64.5 0 0 0 64.800781 0 z", null)]
        public void PathDataCreationTest(string data, Type expectedException)
        {
            try
            {
                var pathData = new PathData(data);
            }
            catch (Exception ex)
            {
                if (!expectedException.Equals(ex?.GetType()))
                    throw;
            }
        }
    }
}
