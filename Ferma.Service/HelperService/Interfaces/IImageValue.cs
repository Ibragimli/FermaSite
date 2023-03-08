using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.HelperService.Interfaces
{
    public interface IImageValue
    {
        public Task<string> ValueStr(string key);
        public Task<int> ValueInt(string key);

    }
}
