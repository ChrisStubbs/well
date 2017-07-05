using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts
{
    public class GlobalUpliftsTask
    {
        public GlobalUpliftsTask()
        {

        }

        public void Execute()
        {
            Console.WriteLine($"Run {nameof(Execute)} of {nameof(GlobalUpliftsTask)}");
        }
    }
}
