using System.Collections.Generic;

namespace BanzaiTransactionalDemo.Commands
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
    }
}