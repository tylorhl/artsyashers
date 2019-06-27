using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Tylorhl.ArtsyAshers.Svg.PathElement.Commands;

namespace Tylorhl.ArtsyAshers.Tests.Svg.PathElement.Commands
{
    [TestClass]
    public class PathCommandTests
    {
        [TestMethod]
        [DataRow(null, typeof(ArgumentException))]
        [DataRow("", typeof(ArgumentException))]
        [DataRow("M0,1", null)]
        [DataRow("M0,1.0", null)]
        [DataRow("M0,1,", typeof(ArgumentException))]
        [DataRow("M0,1,1,0", null)]
        [DataRow("M0,1 1,0", null)]
        [DataRow("M0,1 1,0 2", typeof(ArgumentException))]
        [DataRow("M0,1 1,0 2,", typeof(ArgumentException))]
        [DataRow("M0,1 1,0M", typeof(FormatException))]
        [DataRow("M0,1 1,0M0,1", typeof(FormatException))]
        public void CommandCreationTest(string commandString, Type expectedException)
        {
            try
            {
                PathCommand.Create(commandString);
            }
            catch (Exception ex)
            {
                if(!expectedException.Equals(ex?.GetType()))
                    throw;
            }
        }
    }
}
