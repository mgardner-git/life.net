
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Life;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class testStandardLife
    {
        [TestMethod]
        //Any live cell with fewer than two live neighbours dies, as if caused by under-population.
        public void testRule1_oneNeighbor()
        {
            //test with exactly one live neighbor each
            StandardLife test = new StandardLife(2);
            test.GetCell(0, 0).IsAlive = true;
            test.GetCell(0, 1).IsAlive = true;
            test.GetCell(1, 0).IsAlive = false;
            test.GetCell(1, 1).IsAlive = false;
            test.Iterate();

            Assert.IsFalse(test.GetCell(0,0).IsAlive);
            Assert.IsFalse(test.GetCell(0, 1).IsAlive);
            Assert.IsFalse(test.GetCell(1, 0).IsAlive);
            Assert.IsFalse(test.GetCell(1 ,1).IsAlive);

        }

        [TestMethod]
        public void testRule1_noNeighbors()
        {

            //test with no live neighbors
            StandardLife test2 = new StandardLife(2);
            test2.GetCell(0, 0).IsAlive = true;
            test2.GetCell(0, 1).IsAlive = false;
            test2.GetCell(1, 0).IsAlive = false;
            test2.GetCell(1, 1).IsAlive = false;
            test2.Iterate();

            Assert.IsFalse(test2.GetCell(0, 0).IsAlive);
            Assert.IsFalse(test2.GetCell(0, 1).IsAlive);
            Assert.IsFalse(test2.GetCell(1, 0).IsAlive);
            Assert.IsFalse(test2.GetCell(1, 1).IsAlive);


        }

        [TestMethod]
        //Any live cell with two or three live neighbours lives on to the next generation.
        public void testRule2()
        {
            //test with 2 live neighbors
            StandardLife test = new StandardLife(3);
            test.GetCell(0, 0).IsAlive = false;
            test.GetCell(0, 1).IsAlive = false;
            test.GetCell(0, 2).IsAlive = false;
            test.GetCell(1, 0).IsAlive = true;
            test.GetCell(1, 1).IsAlive = true;
            test.GetCell(1, 2).IsAlive = true;
            test.GetCell(2, 0).IsAlive = false;
            test.GetCell(2, 1).IsAlive = false;
            test.GetCell(2, 2).IsAlive = false;

            test.Iterate();
            Console.WriteLine(test);

            Assert.IsFalse(test.GetCell(0, 0).IsAlive);
            Assert.IsTrue(test.GetCell(0, 1).IsAlive);
            Assert.IsFalse(test.GetCell(0, 2).IsAlive);
            Assert.IsFalse(test.GetCell(1, 0).IsAlive);
            Assert.IsTrue(test.GetCell(1, 1).IsAlive);
            Assert.IsFalse(test.GetCell(1, 2).IsAlive);
            Assert.IsFalse(test.GetCell(2, 0).IsAlive);
            Assert.IsTrue(test.GetCell(2, 1).IsAlive);
            Assert.IsFalse(test.GetCell(2, 2).IsAlive);


        }
    }

}
