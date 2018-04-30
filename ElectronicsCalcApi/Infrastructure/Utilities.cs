using ElectronicsCalcApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicsCalcApi.Infrastructure
{
    public static class Utilities
    {
        /// <summary>
        /// Does the actual calculation of resistance, tolerance, low and high values for a resistor, given the derived band values.
        /// </summary>
        /// <param name="ocm_input"></param>
        /// <returns>Returns an object containing calc'ed values</returns>
        public static OhmValuesModel DoCalc(OhmCalcModel ocm_input)
        {
            //init returning object
            OhmValuesModel ovm_output = new OhmValuesModel();
            ovm_output.ohms = 0;
            ovm_output.tolerance = 0;
            ovm_output.low = 0;
            ovm_output.high = 0;

            //use all significant figures and turn into a concat'ed string
            string sigfigs = string.Empty;
            foreach (var sigfig in ocm_input.significantFigures)
            {
                sigfigs += sigfig.ToString();
            }

            //convert concated significant figures into a calculable number and calc it against multiplier and tolerance values.
            int iOhms;
            if (int.TryParse(sigfigs, out iOhms) && (iOhms > 0))
            {
                //sigfig bands converted to number, multiply parsed number with multiplier value, set ohms value as decimal to retain numbers on each side of decimal place.
                ovm_output.ohms = iOhms * ocm_input.multiplier;

                //now calculate the low and high tolerace
                ovm_output.low = ovm_output.ohms - (ovm_output.ohms * ocm_input.tolerance);
                ovm_output.high = ovm_output.ohms + (ovm_output.ohms * ocm_input.tolerance);

                //set output tolerance val
                ovm_output.tolerance = ocm_input.tolerance;
            }

            return ovm_output;
        }
    }
}