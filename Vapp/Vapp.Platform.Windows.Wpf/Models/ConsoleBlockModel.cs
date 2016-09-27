﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vapp.Platform.Windows.Wpf.Models
{
    class ConsoleBlockModel
    {
        #region Properties

        public string Text { get; set; }

        public Brush Colour { get; set; } = Brushes.Black;

        #endregion
    }
}
