using System;
using Life;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class testGeneticLife
    {
        [TestMethod]
        //Any live cell with fewer than two live neighbours dies, as if caused by under population
        public void TestRule1()
        {
            //test with 0 live neighbors
            GeneticLife test = new GeneticLife(2);
            test.GetCell(0, 0).IsAlive = true;
            test.GetCell(0, 1).IsAlive = false;
            test.GetCell(1, 0).IsAlive = false;
            test.GetCell(1, 1).IsAlive = false;
            test.Iterate();

            Assert.IsFalse(test.GetCell(0, 0).IsAlive);
            Assert.IsFalse(test.GetCell(0, 1).IsAlive);
            Assert.IsFalse(test.GetCell(1, 0).IsAlive);
            Assert.IsFalse(test.GetCell(1, 1).IsAlive);


            //test with 1 live neighbors
            test = new GeneticLife(2);
            test.GetCell(0, 0).IsAlive = true;
            test.GetCell(0, 1).IsAlive = true;
            test.GetCell(1, 0).IsAlive = false;
            test.GetCell(1, 1).IsAlive = false;
            test.Iterate();

            Assert.IsFalse(test.GetCell(0, 0).IsAlive);
            Assert.IsFalse(test.GetCell(0, 1).IsAlive);
            Assert.IsFalse(test.GetCell(1, 0).IsAlive);
            Assert.IsFalse(test.GetCell(1, 1).IsAlive);
        }

        //Any live cell with 2 or 3 neighbours lives on to the next generation.  It's color remains the same
        [TestMethod]
        public void testRule2()
        {
            //test with 2 neighbors
            GeneticLife test = new GeneticLife(2);
            ColorCell testCell = test.GetCell(0, 0);
            testCell.IsAlive = true;
            testCell.red = Byte.MaxValue / 2;
            testCell.green = Byte.MaxValue / 2;
            testCell.blue = Byte.MaxValue / 2;
            
            test.GetCell(0, 1).IsAlive = true;
            test.GetCell(0, 1).red = Byte.MaxValue;
            test.GetCell(1, 0).IsAlive = true;
            test.GetCell(1, 0).blue = Byte.MaxValue;
            test.GetCell(1, 1).IsAlive = false;
            test.Iterate();

            Assert.IsTrue(test.GetCell(0, 0).IsAlive);
            Assert.AreEqual(Byte.MaxValue / 2, testCell.red);
            Assert.AreEqual(Byte.MaxValue / 2, testCell.green);
            Assert.AreEqual(Byte.MaxValue / 2, testCell.blue);

            //test with 3 neighbors
            test = new GeneticLife(2);
            testCell = test.GetCell(0, 0);
            testCell.IsAlive = true;
            testCell.red = Byte.MaxValue / 2;
            testCell.green = Byte.MaxValue / 2;
            testCell.blue = Byte.MaxValue / 2;

            test.GetCell(0, 1).IsAlive = true;
            test.GetCell(0, 1).red = Byte.MaxValue;
            test.GetCell(1, 0).IsAlive = true;
            test.GetCell(1, 0).blue = Byte.MaxValue;
            test.GetCell(1, 1).IsAlive = true;
            test.Iterate();

            Assert.IsTrue(test.GetCell(0, 0).IsAlive);
            Assert.AreEqual(Byte.MaxValue / 2, testCell.red);
            Assert.AreEqual(Byte.MaxValue / 2, testCell.green);
            Assert.AreEqual(Byte.MaxValue / 2, testCell.blue);



        }

        //Any live cell with more than 3 live neighbours dies, as if by overpopulation
        [TestMethod]
        public void testRule3()
        {
            //test with 4 neighbors
            GeneticLife test = new GeneticLife(3);
            test.GetCell(0, 0).IsAlive = false;
            test.GetCell(0, 1).IsAlive = true;
            test.GetCell(0, 2).IsAlive = false;
            test.GetCell(1, 0).IsAlive = true;            
            test.GetCell(1, 1).IsAlive = true;
            test.GetCell(1, 2).IsAlive = true;
            test.GetCell(2, 0).IsAlive = false;
            test.GetCell(2, 1).IsAlive = true;
            test.GetCell(2, 2).IsAlive = false;
            test.Iterate();

            Assert.IsFalse(test.GetCell(1, 1).IsAlive);
        }

        //Any dead cell with exactly three live neighbours becomes alive, It's color becomes the average of the colors of live cells in it's local block of 9
        [TestMethod]
        public void testRule4()
        {
            GeneticLife test = new GeneticLife(2);
            ColorCell testCell = test.GetCell(0, 0);
            testCell.IsAlive = false;
            test.GetCell(0, 1).IsAlive = true;
            initializeSimpleColors(test.GetCell(0, 1));
            test.GetCell(0, 1).IsAlive = true;
            initializeSimpleColors(test.GetCell(1, 0));
            test.GetCell(1, 0).IsAlive = true;
            initializeSimpleColors(test.GetCell(1, 1));
            test.GetCell(1, 1).IsAlive = true;
            test.Iterate();

            testCell = test.GetCell(0, 0);
            Assert.IsTrue(testCell.IsAlive);
            Assert.AreEqual(testCell.red, 1);
            Assert.AreEqual(testCell.green, 2);
            Assert.AreEqual(testCell.blue, 3);
        }

        private void initializeSimpleColors(ColorCell cell)
        {
            cell.red = 1;
            cell.green = 2;
            cell.blue = 3;
        }

    }
}
