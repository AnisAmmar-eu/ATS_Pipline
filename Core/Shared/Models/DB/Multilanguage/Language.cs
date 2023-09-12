using Core.Shared.Models.DB.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Models.DB.Multilanguage
{
    public class Language : BaseEntity
    {
        public string RID { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
