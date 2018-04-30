using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ElectronicsCalcApi.Models
{
    public class ResistanceColorCode
    {
        public int Id { get; set; }
        public string RingColor { get; set; }
        public string Code { get; set; }
        public Nullable<int> SignificantFigure { get; set; }
        public Nullable<decimal> Multiplier { get; set; }
        public Nullable<decimal> Tolerance { get; set; }
    }

    public partial class ElectronicsCalculatorDbContext : DbContext
    {
        public ElectronicsCalculatorDbContext()
            : base("name=ElectronicsCalculatorDbContext") { }

        public virtual DbSet<ResistanceColorCode> ResistanceColorCodes { get; set; }
    }

}