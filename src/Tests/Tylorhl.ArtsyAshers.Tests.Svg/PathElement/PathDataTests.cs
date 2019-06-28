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
        [DataRow("", typeof(ArgumentException))]
        [DataRow("M0,1", null)]
        [DataRow("M0,1.0", null)]
        [DataRow("M0,1,", typeof(ArgumentException))]
        [DataRow("M0,1,1,0", null)]
        [DataRow("M0,1 1,0", null)]
        [DataRow("M0,1 1,0 2", typeof(ArgumentException))]
        [DataRow("M0,1 1,0 2,", typeof(ArgumentException))]
        [DataRow("M0,1 1,0M", typeof(ArgumentOutOfRangeException))]
        [DataRow("M0,1 1,0M0,1", null)]
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
