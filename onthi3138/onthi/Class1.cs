using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace onthi
{
    public class MyCheckedListBox : CheckedListBox
    {
        private SolidBrush primaryColor = new SolidBrush(Color.White);
        private SolidBrush alternateColor = new SolidBrush(Color.Red);
        List<bool> qRight = new List<bool>();
        [Browsable(true)]
        public Color PrimaryColor
        {
            get { return primaryColor.Color; }
            set { primaryColor.Color = value; }
        }

        [Browsable(true)]
        public Color AlternateColor
        {
            get { return alternateColor.Color; }
            set { alternateColor.Color = value; }
        }

        public void setQRight(List<bool> l)
        {
            qRight = l;
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (Items.Count <= 0)
                return;

            var contentRect = e.Bounds;
            contentRect.X = 16;
            if (e.Index < qRight.Count) e.Graphics.FillRectangle(qRight[e.Index] ? primaryColor : alternateColor, contentRect);
            else e.Graphics.FillRectangle(primaryColor, contentRect);
            e.Graphics.DrawString(Convert.ToString(Items[e.Index]), e.Font, Brushes.Black, contentRect);
        }
    }
}
