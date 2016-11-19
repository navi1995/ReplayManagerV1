using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplayManagerv1
{
    public class ListViewEx : ListView
    {
        private bool m_ByDoubleClick = false; // indicate if the click is double click

        protected override void OnItemCheck(System.Windows.Forms.ItemCheckEventArgs ice)
        {
            if (m_ByDoubleClick)
            {
                ice.NewValue = ice.CurrentValue;
                m_ByDoubleClick = false;
            }
            else
                base.OnItemCheck(ice);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Clicks == 2)
                m_ByDoubleClick = true; // Set to true here since Clicks equals 2 (a double click)

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_ByDoubleClick = false; // Set to false by default
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            // The following code is to prevent the MouseDoubleClick event if the double click is on the CheckBox
            ListViewHitTestInfo ti = HitTest(e.X, e.Y);
            if (ti.Location != ListViewHitTestLocations.StateImage)
                base.OnMouseDoubleClick(e);
        }
    }
}
