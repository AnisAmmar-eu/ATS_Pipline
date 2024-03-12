using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Shared.Models.DB.Kernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SignMatch.Models.DB.ToDoTable
{
    public class Match : SignedCycle
    {
        public int MatchableCycleId { get; set; }
    }
}