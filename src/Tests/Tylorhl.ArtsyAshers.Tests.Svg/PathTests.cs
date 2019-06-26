using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tylorhl.ArtsyAshers.Svg;

namespace Tylorhl.ArtsyAshers.Tests.Svg
{
    [TestClass]
    public class PathTests
    {
        [TestMethod]
        [Ignore] // TODO: For now
        [DataRow("")]
        [DataRow("U")]
        [DataRow(";")]
        [DataRow("M0,0L0,1")]
        [DataRow("M0,0l0,1z")]
        [DataRow("M 64.800781 0 A 64.800003 64.5 0 0 0 0 64.5 A 64.800003 64.5 0 0 0 64.800781 129 A 64.800003 64.5 0 0 0 87.107422 125.00586 A 111 111.2 0 0 0 69 185.69922 A 111 111.2 0 0 0 180 296.90039 A 111 111.2 0 0 0 291 185.69922 A 111 111.2 0 0 0 272.92188 124.94531 A 64.800003 64.5 0 0 0 295.30078 129 A 64.800003 64.5 0 0 0 360.09961 64.5 A 64.800003 64.5 0 0 0 295.30078 0 A 64.800003 64.5 0 0 0 230.5 64.5 A 64.800003 64.5 0 0 0 235.69727 89.636719 A 111 111.2 0 0 0 180 74.5 A 111 111.2 0 0 0 124.50781 89.552734 A 64.800003 64.5 0 0 0 129.59961 64.5 A 64.800003 64.5 0 0 0 64.800781 0 z")]
        public void BasicParsingTest(string pathData)
        {
        }

        [TestMethod]
        [DataRow("M0,0L0,1L0,0L1,0", @"<path d=""M0,0L0,1 0,0 1,0""/>")]
        public void FormattingTest(string pathData, string expected)
        {
            var xml = new Path(pathData).ToString();
            Assert.AreEqual(xml, expected, $"Path output '{xml}' doesn't match expected value '{expected}'.");
        }
    }
}
