using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
                switch (c)
                {
                    case Guna2CheckBox cb:
                        ApplyStyle(cb);
                        break;
                    case Guna2RadioButton rb:
                        ApplyStyle(rb);
                        break;
                    case CheckedListBox clb:
                        ApplyStyle(clb);
                        break;
                }
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

        public static void ApplyStyle(Guna2RadioButton rb)
        {
            rb.CheckedState.FillColor = ActiveColor;
            rb.CheckedState.BorderColor = ActiveColor;
            rb.CheckedState.InnerColor = Color.Black;
            rb.UncheckedState.FillColor = InactiveColor;
            rb.UncheckedState.BorderColor = InactiveColor;
            rb.UncheckedState.InnerColor = Color.Transparent;
        }

        public static void ApplyStyle(CheckedListBox clb)
        {
            clb.DrawMode = DrawMode.OwnerDrawFixed;
            clb.DrawItem -= Clb_DrawItem;
            clb.DrawItem += Clb_DrawItem;
            clb.ItemCheck -= Clb_ItemCheck;
            clb.ItemCheck += Clb_ItemCheck;
        }

        private static void Clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var clb = (CheckedListBox)sender;
            clb.Invalidate(clb.GetItemRectangle(e.Index));
        }

        private static void Clb_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var clb = (CheckedListBox)sender;
            bool isChecked = clb.GetItemChecked(e.Index);
            Color fill = isChecked ? ActiveColor : InactiveColor;

            using (var br = new SolidBrush(fill))
                e.Graphics.FillRectangle(br, e.Bounds);

            var state = isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
            var checkRect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, 16, 16);
            CheckBoxRenderer.DrawCheckBox(e.Graphics, checkRect.Location, state);

            string text = clb.GetItemText(clb.Items[e.Index]);
            var textRect = new Rectangle(e.Bounds.X + 22, e.Bounds.Y, e.Bounds.Width - 24, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, text, e.Font, textRect, Color.Black, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }
    }
}
