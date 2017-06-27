using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ThisFuckingLife
{
    class Cell
    {
        public Color NextColor { get; set; }
        public Color FillColor { get; set; }
        public bool State { get; set; }

        public static implicit operator int(Cell c)
        {
            return Convert.ToInt32(c.State);
        }

        public void ChangeColor()
        {
            if (FillColor.R != NextColor.R || FillColor.G != NextColor.G || FillColor.B != NextColor.B)
                FillColor = NextColor;
        }

        public Cell(bool state, Color color)
        {
            FillColor = color;
            State = state;
        }
    }
}
