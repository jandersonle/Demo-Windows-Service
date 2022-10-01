using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestService.FTPLoggingService
{
    public class GenericTimer<T>
    {
        private Func<T> fn;
        private int interval;
        private int timesToRepeat;

        public GenericTimer()
        {

        }

        public GenericTimer(Func<T> fn, int interval, int timesToRepeat)
        {
            this.fn = fn;
            this.interval = interval;
            this.timesToRepeat = timesToRepeat;
        }

        public void Start()
        {
            if (fn == null)
            {
                throw new ArgumentNullException();
            }

            else
            {
                try
                {
                    for (var i = 0; i < timesToRepeat; i++)
                    {
                        this.fn();
                        Thread.Sleep(interval);
                    }
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }
    }
}
