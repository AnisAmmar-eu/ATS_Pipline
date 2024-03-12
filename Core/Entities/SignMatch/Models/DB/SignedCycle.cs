using Core.Entities.Vision.SignedCycles.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SignMatch.Models.DB
{
    public abstract class SignedCycle
    {
        public DateTimeOffset? CycleTS { get; set; }
        public DataSetID DataSetID { get; set; }
        public string? San1File { get; set; }
        public string? San2File { get; set; }
    }
}