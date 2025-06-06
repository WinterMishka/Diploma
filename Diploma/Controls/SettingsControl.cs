using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Guna.UI2.WinForms;
using System.Windows.Forms;
using Diploma.Classes;

namespace Diploma
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();

            CheckBoxUI.ApplyRecursive(this);
            guna2CheckBox1.Checked = UiSettingsManager.Current.StartFullScreen;
            guna2CheckBox1.CheckedChanged += (s, e) =>
            {
                UiSettingsManager.Current.StartFullScreen = guna2CheckBox1.Checked;
                CheckBoxUI.ApplyStyle(guna2CheckBox1);
                UiSettingsManager.Save();
            };

            this.Load += (s, e) =>
            {
                if (FindForm() is FaceControl face)
                    UiSettingsManager.ApplyTo(face);
            };
        }

        private static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                foreach (var child in GetAllControls(c))
                    yield return child;
                yield return c;
            }
        }

        private void ApplyToAllControls(Action<Control> action)
        {
            var form = FindForm();
            if (form == null) return;
            foreach (var ctrl in GetAllControls(form))
                action(ctrl);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            if (FindForm() is FaceControl form)
            {
                foreach (var btn in form.NavigationButtons)
                    btn.FillColor = colorDialog1.Color;
                foreach (var btn in form.WindowButtons)
                    btn.FillColor = colorDialog1.Color;
                form.TitlePanel.BackColor = colorDialog1.Color;
                UiSettingsManager.Current.NavFillColor = ColorTranslator.ToHtml(colorDialog1.Color);
                UiSettingsManager.Save();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            if (FindForm() is FaceControl form)
            {
                form.SetActiveBorderColor(colorDialog1.Color);
                UiSettingsManager.Current.NavBorderColor = ColorTranslator.ToHtml(colorDialog1.Color);
                UiSettingsManager.Save();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            var newColor = colorDialog1.Color;
            ApplyToAllControls(c =>
            {
                if (c is Guna2Button g)
                    g.FillColor = newColor;
                else if (c is Button b)
                    b.BackColor = newColor;
            });
            UiSettingsManager.Current.GlobalButtonColor = ColorTranslator.ToHtml(newColor);
            UiSettingsManager.Save();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;
            var target = Color.FromArgb(157, 193, 131);
            var newColor = colorDialog1.Color;
            ApplyToAllControls(c =>
            {
                var prop = c.GetType().GetProperty("FillColor");
                if (prop != null && prop.PropertyType == typeof(Color) && (Color)prop.GetValue(c) == target)
                    prop.SetValue(c, newColor);
                if (c.BackColor == target)
                    c.BackColor = newColor;
            });
            UiSettingsManager.Current.PanelFillColor = ColorTranslator.ToHtml(newColor);
            UiSettingsManager.Save();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string fontName = Interaction.InputBox("Введите название шрифта", "Шрифт", Font.FontFamily.Name);
            if (string.IsNullOrWhiteSpace(fontName)) return;
            try
            {
                var fam = new FontFamily(fontName);
                ApplyToAllControls(c =>
                {
                    c.Font = new Font(fam, c.Font.Size, c.Font.Style);
                    if (c is DataGridView dgv)
                    {
                        dgv.ColumnHeadersDefaultCellStyle.Font = c.Font;
                        dgv.DefaultCellStyle.Font = c.Font;
                    }
                });
                UiSettingsManager.Current.FontFamily = fontName;
                UiSettingsManager.Save();
            }
            catch
            {
                MessageBox.Show("Шрифт не найден");
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            string sizeStr = Interaction.InputBox("Введите размер шрифта", "Размер шрифта", Font.Size.ToString());
            if (float.TryParse(sizeStr, out float size) && size > 0)
            {
                ApplyToAllControls(c =>
                {
                    c.Font = new Font(c.Font.FontFamily, size, c.Font.Style);
                    if (c is DataGridView dgv)
                    {
                        dgv.ColumnHeadersDefaultCellStyle.Font = c.Font;
                        dgv.DefaultCellStyle.Font = c.Font;
                    }
                });
                UiSettingsManager.Current.FontSize = size;
                UiSettingsManager.Save();
            }
        }

        private void guna2ButtonReset_Click(object sender, EventArgs e)
        {
            UiSettingsManager.Reset();
            UiSettingsManager.Save();
            if (FindForm() is FaceControl face)
                UiSettingsManager.ApplyDefaults(face);
        }
    }
}
