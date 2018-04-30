using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElectronicsCalcApi.Models;
using ElectronicsCalcApi.Infrastructure;

namespace ElectronicsCalcApi.Controllers
{
    public class ResistanceColorCodesController : ApiController
    {
        /// <summary>
        /// This method signature returns all color codes.
        /// </summary>
        /// <returns>List<ResistanceColorCode></returns>
        [HttpGet]
        public List<ResistanceColorCode> GetAllColorCodes()
        {
            try
            {
                using (var context = new ElectronicsCalculatorDbContext())
                {
                    List<ResistanceColorCode> codes = new List<ResistanceColorCode>();
                    codes = (from c in context.ResistanceColorCodes
                             select c).ToList();
                    return codes;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This method signature returns one record based on passed id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ResistanceColorCode</returns>
        [HttpGet]
        public ResistanceColorCode GetColorCode(int id)
        {
            try
            {
                using (var context = new ElectronicsCalculatorDbContext())
                {
                    ResistanceColorCode code = new ResistanceColorCode();
                    code = (from c in context.ResistanceColorCodes
                            where c.Id.Equals(id)
                            select c).FirstOrDefault();
                    return code;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// This method signature returns one record based on passed color.  
        /// Since signature is not default int datatype, to call this method use parameter name "api/ResistanceColorCode?color=White
        /// </summary>
        /// <param name="color"></param>
        /// <returns>ResistanceColorCode</returns>
        [HttpGet]
        public ResistanceColorCode GetColorCode(string color)
        {
            try
            {
                using (var context = new ElectronicsCalculatorDbContext())
                {
                    ResistanceColorCode code = new ResistanceColorCode();
                    code = (from c in context.ResistanceColorCodes
                            where c.RingColor.Equals(color)
                            select c).FirstOrDefault();
                    return code;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //TODO: COMMENT
        [HttpPost]
        [Route("api/GetResistorValues/4bands")]
        public OhmValuesModel GetResistanceValue(OhmBandsModel ohmBands)
        {
            try
            {
                using (var context = new ElectronicsCalculatorDbContext())
                {

                    LaResistance laResistance = new LaResistance(ohmBands);
                    OhmValuesModel ohmValues = new OhmValuesModel
                    {
                        ohms = laResistance.ohms,
                        low = laResistance.ohmsLow,
                        high = laResistance.ohmsHigh
                    };

                    return ohmValues;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]

        [Route("api/GetResistorValues/array")]
        public OhmValuesModel GetResistanceValue(OhmBandsListModel ohmBands)
        {
            try
            {
                using (var context = new ElectronicsCalculatorDbContext())
                {

                    LaResistance laResistance = new LaResistance(ohmBands);
                    OhmValuesModel ohmValues = new OhmValuesModel
                    {
                        ohms = laResistance.ohms,
                        tolerance = laResistance.tolerance,
                        low = laResistance.ohmsLow,
                        high = laResistance.ohmsHigh
                    };

                    return ohmValues;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }



}
