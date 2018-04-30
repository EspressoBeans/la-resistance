using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElectronicsCalcApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicsCalcApi.Models;
using ElectronicsCalcApi.Infrastructure;
using System.Configuration;

namespace ElectronicsCalcApi.Controllers.Tests
{
    //IMPORTANT - Vic Guadalupe
    //THE INTEGRATION TESTS ARE FOR TESTING WITH ENTITY FRAMEWORK.  
    //ALTHOUGH NOT BEST NOR STANDARD PRACTICE TO WRITE TEST CODE AGAINST THE LOGIC LAYER THAT ACCESSES DYNAMIC DATA, AS WELL AS TEST EQUALITY OF HARD-CODED VARIABLES TO DYNAMIC DATA, THIS IS FOR DEMO PURPOSES.

    [TestClass()]
    public class IntegrationTests

    {
        [TestMethod()]
        public void GetResistanceValue_TestArraySingle()
        {
            //arrange
            List<string> colors = new List<string>();
            colors.Add("Black");

            OhmBandsListModel ohmBands_input = new
                OhmBandsListModel
                {
                    bandColors = colors
                };

            OhmValuesModel ohmValues_check = new OhmValuesModel
            {
                ohms = 0,
                tolerance = 0,
                low = 0,
                high = 0
            };


            //act
            LaResistance laResistance = new LaResistance(ohmBands_input);
            OhmValuesModel ohmValues_output = new OhmValuesModel
            {
                ohms = laResistance.ohms,
                tolerance = laResistance.tolerance,
                low = laResistance.ohmsLow,
                high = laResistance.ohmsHigh
            };

            //assert
            Assert.AreEqual(ohmValues_check.ohms, ohmValues_output.ohms);
            Assert.AreEqual(ohmValues_check.tolerance, ohmValues_output.tolerance);
            Assert.AreEqual(ohmValues_check.low, ohmValues_output.low);
            Assert.AreEqual(ohmValues_check.high, ohmValues_output.high);
        }

        [TestMethod()]
        public void GetResistanceValue_TestArrayMultiple()
        {
            //arrange
            List<string> colors = new List<string>();
            colors.Add("Brown");
            colors.Add("Red");
            colors.Add("Blue");
            colors.Add("Orange");
            colors.Add("Gold");

            OhmBandsListModel ohmBands_input = new
                OhmBandsListModel
            {
                bandColors = colors
            };

            OhmValuesModel ohmValues_check = new OhmValuesModel
            {
                ohms = 126000,
                tolerance = 5,
                low = 119700,
                high = 132300
            };


            //act
            LaResistance laResistance = new LaResistance(ohmBands_input);
            OhmValuesModel ohmValues_output = new OhmValuesModel
            {
                ohms = laResistance.ohms,
                tolerance = laResistance.tolerance,
                low = laResistance.ohmsLow,
                high = laResistance.ohmsHigh
            };

            //assert
            //Assert.IsTrue(true);  //if it gets here the test passed.
            //we shouldn't test equality with data in a database, demo project.
            Assert.AreEqual(ohmValues_check.ohms, ohmValues_output.ohms);
            Assert.AreEqual(ohmValues_check.tolerance, ohmValues_output.tolerance);
            Assert.AreEqual(ohmValues_check.low, ohmValues_output.low);
            Assert.AreEqual(ohmValues_check.high, ohmValues_output.high);
        }
    }
}