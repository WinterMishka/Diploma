using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Diploma.Classes
{
    public static class CheckBoxUI
    {
        private static readonly Color ActiveColor = Color.LimeGreen;
        private static readonly Color InactiveColor = Color.IndianRed;

        public static void ApplyRecursive(Control root)
        {
            foreach (Control c in root.Controls)
            {
                if (c is Guna2CheckBox cb)
                    ApplyStyle(cb);
                if (c.HasChildren)
                    ApplyRecursive(c);
            }
        }

        public static void ApplyStyle(Guna2CheckBox cb)
        {
            cb.CheckedState.FillColor = ActiveColor;
            cb.CheckedState.BorderColor = ActiveColor;
            cb.UncheckedState.FillColor = InactiveColor;
            cb.UncheckedState.BorderColor = InactiveColor;
            try { cb.CheckMarkColor = Color.Black; } catch { }
        }
    }
}
