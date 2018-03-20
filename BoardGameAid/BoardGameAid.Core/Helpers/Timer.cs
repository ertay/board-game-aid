using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardGameAid.Core.Helpers
{
    /// <summary>
    /// A custom implementation of a timer using Task.Delay.
    /// </summary>
    public class PclTimer
    {
        /// <summary>
        /// Whether the timer is started or not.
        /// </summary>
        private bool _started;

        /// <summary>
        /// Holds the time in seconds.
        /// </summary>
        private int _time;

        /// <summary>
        /// Gets the time as a timespan.
        /// </summary>
        public TimeSpan Time => TimeSpan.FromSeconds(_time);

        /// <summary>
        /// Event that fires on every tick. Contains the timespan of the current remaining time.
        /// </summary>
        public event EventHandler<TimeSpan> TimeElapsed;

        /// <summary>
        /// Create a new  timer.
        /// </summary>
        /// <param name="startTime"></param>
        public PclTimer(TimeSpan startTime)
        {
            _time = (int) startTime.TotalSeconds;
        }

        /// <summary>
        /// Starts the t timer.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken token = default(CancellationToken))
        {
            if (_started) return;

            _started = true;

            while (_started && _time > 0)
            {
                // wait 1000 ms, then fire the eveent
                await Task.Delay(1000, token).ConfigureAwait(false);
                if (--_time == 0)
                {
                    _started = false;
                }
                TimeElapsed?.Invoke(this, Time);
            }
        }
    }
}
