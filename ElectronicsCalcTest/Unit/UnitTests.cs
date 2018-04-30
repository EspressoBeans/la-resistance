using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElectronicsCalcApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicsCalcApi.Models;

namespace ElectronicsCalcApi.Infrastructure.Tests
{
    [TestClass()]
    public class UnitTests
    {
        [TestMethod()]
        public void Test_DoCalc()
        {
            //arrange
            OhmValuesModel ohmValues = new OhmValuesModel();
            OhmValuesModel ohmValues_check = new OhmValuesModel
            {
                ohms = 126000,
                tolerance = 0.05M,
                low = 119700,
                high = 132300
            };

            OhmCalcModel ohmCalc = new OhmCalcModel();
            List<int> digits = new List<int>();
            digits.Add(1);
            digits.Add(2);
            digits.Add(6);

            ohmCalc.significantFigures = digits;
            ohmCalc.multiplier = 1000;
            ohmCalc.tolerance = 0.05M;

            //act
            ohmValues = Utilities.DoCalc(ohmCalc);

            //assert
            Assert.AreEqual(ohmValues_check.ohms, ohmValues.ohms);
            Assert.AreEqual(ohmValues_check.tolerance, ohmValues.tolerance);
            Assert.AreEqual(ohmValues_check.low, ohmValues.low);
            Assert.AreEqual(ohmValues_check.high, ohmValues.high);
        }
    }
}