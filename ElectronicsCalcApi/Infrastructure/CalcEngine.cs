using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectronicsCalcApi.Models;
using ElectronicsCalcApi.Repositories;

namespace ElectronicsCalcApi.Infrastructure
{
    public interface IOhmValueCalculator
    {

        //NOTE: NO LONGER USED, INT DOES NOT GIVE ACCURATE OUTPUT OF VALUES
        /// <summary>
        /// Calculates the Ohm value of a resistor based on the band colors.
        /// </summary>
        /// <param name="bandAColor">The color of the first figure of component value band.</param>
        /// <param name="bandBColor">The color of the second significant figure band.</param>
        /// <param name="bandCColor">The color of the decimal multiplier band.</param>
        /// <param name="bandDColor">The color of the tolerance value band.</param>
        /// <returns>The Ohm value as int.</returns>
        //int CalculateOhmValue(string bandAColor,
        //                      string bandBColor,
        //                      string bandCColor,
        //                      string bandDColor);


        /// <summary>
        /// Calculates the Ohm value of a resistor based on the band colors.
        /// </summary>
        /// <param name="bandAColor">The color of the first figure of component value band.</param>
        /// <param name="bandBColor">The color of the second significant figure band.</param>
        /// <param name="bandCColor">The color of the decimal multiplier band.</param>
        /// <param name="bandDColor">The color of the tolerance value band.</param>
        /// <returns>The Ohm value as decimal.</returns>
        decimal CalculateOhmValue(string bandAColor,
                              string bandBColor,
                              string bandCColor,
                              string bandDColor);


        /// <summary>
        /// Total color bands on a resistor is variable depending on application.  
        /// </summary>
        /// <param name="bandColors">List collection of strings containing color bands</param>
        /// <param name="resistance">Output of base resistance in Ohms</param>
        /// <param name="low">Output of lowest tolerance Ohms value.</param>
        /// <param name="high">Output of highest tolerance Ohms value.</param>
        /// <returns>Outputs the Ohm value, low tolerance, and high tolerance values in decimal.</returns>
        void CalculateOhmValue(List<string> bandColors,
                               out decimal resistance,
                               out decimal tolerance,
                               out decimal low,
                               out decimal high);
    }

    public interface IOhmsLaw
    {
        /// <summary>
        /// Outputs an Ohm's Law (Current = Voltage/Resistance) value based on the passed values.  Requires exactly 2 arguments of the 3 total parameters.
        /// </summary>
        /// <param name="I">Current</param>
        /// <param name="V">Voltage</param>
        /// <param name="R">Resistance</param>
        /// <returns></returns>
        decimal CalculateValue(decimal? I, decimal? V, decimal? R);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CodedResistance : IOhmValueCalculator
    {
        private IRepository<ResistanceColorCode, int, string> _repository;

        public CodedResistance()
        {
            ElectronicsCalculatorDbContext context = new ElectronicsCalculatorDbContext();
            _repository = new ColorCodesRepository(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bandAColor"></param>
        /// <param name="bandBColor"></param>
        /// <param name="bandCColor"></param>
        /// <param name="bandDColor"></param>
        /// <returns></returns>
        public decimal CalculateOhmValue(string bandAColor, string bandBColor, string bandCColor, string bandDColor)
        {
            string sOhms;
            int iOhms;
            decimal dOhms = 0;

            int sf1 = _repository.Get(bandAColor).SignificantFigure ?? 0;
            int sf2 = _repository.Get(bandBColor).SignificantFigure ?? 0;
            decimal m = _repository.Get(bandCColor).Multiplier ?? 0;
            decimal t = _repository.Get(bandDColor).Tolerance ?? 0;

            //with first two bands, use table to match color and concatenate significant figures, then parse to number
            sOhms = sf1.ToString() + sf2.ToString();

            //make sure we have a number (and greater than zero) to continue, (tryparse will execute first then set the out variable)
            if (int.TryParse(sOhms, out iOhms) && (iOhms > 0))
            {
                //with third band, multiply parsed number with multiplier value, set ohms value as decimal to retain numbers on each side of decimal place.
                dOhms = iOhms * (decimal)m;
            }

            return dOhms;
        }

        /// <summary>
        /// BETA FEATURE:  Resistors can have a variable amount of bands, this method converts color band codes to Ohms resistance value.
        /// Currently does not account for Fail Rate, nor Temperature Coefficient which is relevant for resistors with color band count greater than 5.
        /// </summary>
        /// <param name="bandColors">A collection of colors.</param>
        /// <param name="resistance"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public void CalculateOhmValue(List<string> bandColors, out decimal resistance, out decimal tolerance, out decimal low, out decimal high)
        {
            //throw new NotImplementedException();


            //1. read ResistanceColorCodes table from database into iqueryable collection
            int iOhms;
            decimal multiplier;
            string sigfigs = string.Empty;

            resistance = 0;
            tolerance = 0;
            low = 0;
            high = 0;

            //single banded resistors only indicate value of resistance with single significant figure
            if (bandColors.Count == 1)
            {
                decimal ohm = _repository.Get(bandColors[0]).SignificantFigure ?? 0;  //extract value
                resistance = low = high = ohm;  //set all values to same
            }
            //banded resistors are not manufactured with band counts of 2 or greater than 6
            else if ((bandColors.Count >= 3) && (bandColors.Count < 6))
            {
                //the last 2 bands (for resistors with 3 to 5 bands), represent multiplier and tolerance respectively.
                multiplier = (_repository.Get(bandColors[bandColors.Count - 2]).Multiplier ?? 0);
                tolerance = (_repository.Get(bandColors[bandColors.Count - 1]).Tolerance ?? 0);

                //now remove the last 2 from List so that just significant figures bands remain
                bandColors.RemoveAt(bandColors.Count - 1);
                bandColors.RemoveAt(bandColors.Count - 1);

                for (int i = 0; i < bandColors.Count; i++)
                {
                    sigfigs += (_repository.Get(bandColors[i]).SignificantFigure ?? 0).ToString();
                }

                if (int.TryParse(sigfigs, out iOhms) && (iOhms > 0))
                {
                    //sigfig bands converted to number, multiply parsed number with multiplier value, set ohms value as decimal to retain numbers on each side of decimal place.
                    resistance = iOhms * multiplier;

                    //now calculate the low and high tolerace
                    low = resistance - (resistance * tolerance);
                    high = resistance + (resistance * tolerance);
                }
            }
            else if ((bandColors.Count == 6))
            {
                //band count of 6 currently not supported.
            }
        }
    }

    public class LaResistance
    {
        private List<string> _bandColors;
        private decimal _ohms;
        private decimal _tolerance;
        private decimal _ohmsLow;
        private decimal _ohmsHigh;

        public decimal ohms { get; }
        public decimal tolerance { get; }
        public decimal ohmsLow { get; }
        public decimal ohmsHigh { get; }

        /// <summary>
        /// This constructor method will set the ohms value property, as well as low and high tolerance values properties from passed variable length collection of band color codes.
        /// </summary>
        /// <param name="bandColors"></param>
       // public LaResistance(List<string> bandColors)
        public LaResistance(OhmBandsListModel model)
        {
            _bandColors = model.bandColors;

            CodedResistance calc = new CodedResistance();
            calc.CalculateOhmValue(_bandColors, out _ohms, out _tolerance, out _ohmsLow, out _ohmsHigh);
            ohms = _ohms;
            tolerance = _tolerance * 100; //for outputting pct
            ohmsLow = _ohmsLow;
            ohmsHigh = _ohmsHigh;
        }


        /// <summary>
        /// This constructor method is the most basic and only sets ohms value from passed four band color codes.
        /// </summary>
        /// <param name="bandAColor"></param>
        /// <param name="bandBcolor"></param>
        /// <param name="bandCColor"></param>
        /// <param name="bandDcolor"></param>
        public LaResistance(string bandAColor, string bandBcolor, string bandCColor, string bandDcolor)
        {
            CodedResistance calc = new CodedResistance();
            ohms = calc.CalculateOhmValue(bandAColor, bandBcolor, bandCColor, bandDcolor);
        }

        /// <summary>
        /// This constructor method will only set ohms value property
        /// </summary>
        /// <param name="model"></param>
        public LaResistance(OhmBandsModel model)
        {
            CodedResistance calc = new CodedResistance();
            ohms = calc.CalculateOhmValue(model.bandAColor, model.bandBColor, model.bandCColor, model.bandDColor);
        }
    }


}