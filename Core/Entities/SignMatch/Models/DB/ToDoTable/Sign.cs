using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SignMatch.Models.DB.ToDoTable
{
    public class Sign
    {
        public int CycleID { get; set; }
        public int CycleRID { get; set; }
        public int CamID { get; set; }
        public int StationID { get; set; }
        public DateTimeOffset? CycleTS { get; set; }
    }
}