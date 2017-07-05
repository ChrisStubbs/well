using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.Clean
{
    public class CleanTask
    {
        public CleanTask()
        {
            
        }

        public void Execute()
        {
            Console.WriteLine($"Run {nameof(Execute)} of {nameof(CleanTask)}");
        }
    }
}
