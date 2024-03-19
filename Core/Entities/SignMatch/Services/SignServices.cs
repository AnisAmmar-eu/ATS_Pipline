using Core.Shared.Dictionaries;
using Core.Shared.DLLvision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SignMatch.Services
{
    public class SignServices : ISignServices
    {
        public Task<int> SignAnode(string anodeID)
        {
            return Task.Run(
                () => DLLvision.fcx_sign(
                    0,
                    0,
                    ConfigDictionary.ImagesPath,
                    anodeID,
                    ConfigDictionary.ImagesOutputPath));
        }
    }
}