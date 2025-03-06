using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeTest
{
    using System;

    namespace ConsoleApplication3
    {
        public class Program3
        {
            public static void Main()
            {
                var counter = new Counter();
                counter.ThresholdReached += counter_ThresholdReached;                
            }

            static void counter_ThresholdReached(object? sender, ThresholdReachedEventArgs e)
            {
                Console.WriteLine("The threshold of {0} was reached at {1}.", e.Threshold, e.TimeReached e.test);
                Environment.Exit(0);
            }
        }

        class Counter
        {
             protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
            {
                ThresholdReached?.Invoke(this, e);
            }

            public event EventHandler<ThresholdReachedEventArgs>? ThresholdReached;
        }

        public class ThresholdReachedEventArgs : EventArgs
        {
            public int Threshold { get; set; }
            public DateTime TimeReached { get; set; }
            public int test { get; set; }
        }
    }
}
