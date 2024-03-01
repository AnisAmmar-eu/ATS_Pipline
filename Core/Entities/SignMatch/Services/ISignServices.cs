using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SignMatch.Services
{
    public interface ISignServices
    {
        Task<int> SignAnode(string anodeID);
    }
}