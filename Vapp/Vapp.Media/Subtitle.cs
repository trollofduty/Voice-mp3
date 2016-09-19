using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapp.Media.Gaps;

namespace Vapp.Media
{
    public class Subtitle : Pause
    {
        #region Constructor

        public Subtitle(string text, TimeSpan start, TimeSpan end)
            : base(start, end)
        {
            this.Text = text;
        }

        #endregion

        #region Properties

        public string Text { get; set; }

        #endregion
    }
}
