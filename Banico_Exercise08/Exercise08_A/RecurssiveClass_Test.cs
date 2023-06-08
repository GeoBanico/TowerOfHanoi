using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Banico_Exercise08
{
    class RecurssiveClass_Test
    {
        [TestCase(5,14)]
        [TestCase(14,5)]
        public void test(int ex1, int ex2) 
        {

            var ans = ex1 % ex2;

            Console.WriteLine(ans);
        }

        [TestCase(10,9, 1)]
        [TestCase(120, 9, 3)]
        public void GCD_FindingGCD_CheckAns(int m, int n, int ans) 
        {
            //Arrange
            var RC = new RecurssiveClass();

            //Act
            var gcd = RC.GCD(m, n);

            //Assert
            gcd.Should().Be(ans);
        }

        [TestCase(3, 1.162d)]
        [TestCase(5, 2.061d)]
        public void SummingSeries_FindDoubleTotal_CheckAns(double i, double ans) 
        {
            //Arrange
            var RC = new RecurssiveClass();

            //Act
            var SS = Math.Round(RC.SummingSeries(i),3);

            //Assert
            SS.Should().Be(ans);
        }

        [TestCase(999910, 37)]
        [TestCase(9999, 36)]
        public void SummingInts_FindInttotal_CheckAns(int num, int ans)
        {
            //Arrange
            var RC = new RecurssiveClass();

            //Act
            var SI = RC.SummingInts(num);

            //Assert
            SI.Should().Be(ans);
        }
    }
}
