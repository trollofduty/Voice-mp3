using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Media.Gaps
{
    public class Pause
    {
        #region Constructor

        public Pause(TimeSpan start, TimeSpan end)
        {
            this.Start = start;
            this.End = end;
        }

        #endregion

        #region Properties

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public TimeSpan Length
        {
            get { return this.End - this.Start; }
        }

        #endregion
    }
}
