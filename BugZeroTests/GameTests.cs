using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BugsZero;

namespace BugZeroTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Should_Establish_Safe_Net()
        {
            Random randomizer = new Random(123455);
            var log = new ConsoleLog();
            for (int i = 0; i < 15; i++)
            {
                GameRunner.PlayGame(randomizer, log);
            }

            var expected = File.ReadAllText("D:/Refactor/BugsZero/BugZeroTests/GameTest.itsLockedDown.approved.txt");
            Assert.AreEqual(expected.Replace("\r\n", ""), log.ToString());
        }
    }
}
