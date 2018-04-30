using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ElectronicsCalcApi.Models;

namespace ElectronicsCalcApi.Repositories
{
    public class ColorCodesRepository : IRepository<ResistanceColorCode, int, string>
    {
        public ElectronicsCalculatorDbContext _context { get; set; }

        public ColorCodesRepository(ElectronicsCalculatorDbContext context)
        {
            this._context = context;
        }

        public ResistanceColorCode Get(int id)
        {
            return _context.ResistanceColorCodes.Find(id);
        }

        public IEnumerable<ResistanceColorCode> Get()
        {
            return _context.ResistanceColorCodes.ToList();
        }

        public ResistanceColorCode Get(string filter)
        {
            return _context.ResistanceColorCodes.FirstOrDefault(c => c.RingColor.Equals(filter));
        }
    }
}