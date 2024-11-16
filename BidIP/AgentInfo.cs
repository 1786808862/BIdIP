using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BidIP
{
    public static class AgentInfo
    {
        public static Dictionary<string, string> IPs = new Dictionary<string, string>();
        public static Dictionary<string, int> ssyCount = new Dictionary<string, int>();
        public static Dictionary<string, DateTime> lastTime = new Dictionary<string, DateTime>();
        public static Dictionary<string, DateTime> lastSSYTime = new Dictionary<string, DateTime>();
    }
}
