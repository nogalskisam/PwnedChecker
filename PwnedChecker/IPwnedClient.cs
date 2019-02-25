using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PwnedChecker
{
    public interface IPwnedClient
    {
        Task<int> CheckPassword(string password);
    }
}
