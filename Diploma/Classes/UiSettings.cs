using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Diploma.Classes
{
    public class UiSettingsData
    {
        #region Свойства
        public string NavFillColor { get; set; }
        public string NavBorderColor { get; set; }
        public string GlobalButtonColor { get; set; }
        public string PanelFillColor { get; set; }
        public string FontFamily { get; set; }
        public float? FontSize { get; set; }
        public string FontColor { get; set; }
        public bool StartFullScreen { get; set; }
        #endregion
    }

    public static class UiSettingsManager
    {
        #region Поля
        private static readonly string FilePath = Path.Combine(Application.StartupPath, "ui_settings.json");
        #endregion

        #region Свойства
        public static UiSettingsData Current { get; private set; } = new UiSettingsData();
        #endregion

        #region Методы
        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    Current = JsonSerializer.Deserialize<UiSettingsData>(json) ?? new UiSettingsData();
                }
            }
            catch
            {
                Current = new UiSettingsData();
            }
        }

        public static void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        public static void Reset()
        {
            Current = new UiSettingsData();
        }

        public static void ApplyDefaults(FaceControl form)
        {
            var navFill = Color.DarkCyan;
            var navBorderFirst = Color.ForestGreen;
            var navBorderRest = Color.White;
            var panelFill = Color.FromArgb(157, 193, 131);
            var defaultFont = new Font("Verdana", 14.25f);

            bool first = true;
            foreach (var btn in form.NavigationButtons)
            {
                btn.FillColor = navFill;
                btn.CustomBorderColor = first ? navBorderFirst : navBorderRest;
                first = false;
            }
            var windowFont = new Font("Segoe UI", 11.25f, FontStyle.Bold);
            foreach (var btn in form.WindowButtons)
            {
                btn.FillColor = navFill;
                btn.ForeColor = Color.Black;
                btn.Font = windowFont;
                switch (btn.Name)
                {
                    case "guna2BtnResize":
                        btn.Text = "_";
                        break;
                    case "guna2BtnMinimize":
                        btn.Text = "▢";
                        break;
                    case "guna2BtnClose":
                        btn.Text = "×";
                        break;
                }
            }

            form.TitlePanel.BackColor = navFill;
            form.SetActiveBorderColor(navBorderFirst);

            foreach (var ctrl in GetAllControls(form))
            {
                if (form.WindowButtons.Contains(ctrl))
                    continue;

                if (ctrl is Guna2TabControl tab)
                {
                    tab.TabMenuBackColor = navFill;
                    tab.TabButtonIdleState.FillColor = navFill;
                    tab.TabButtonSelectedState.FillColor = navFill;
                    tab.TabButtonSelectedState.InnerColor = navBorderFirst;
                    tab.TabButtonIdleState.ForeColor = Color.White;
                    tab.TabButtonHoverState.ForeColor = Color.White;
                    tab.TabButtonSelectedState.ForeColor = Color.White;
                    foreach (TabPage page in tab.TabPages)
                    {
                        page.BackColor = panelFill;
                        page.ForeColor = Color.White;
                    }
                }
                else if (ctrl.BackColor != navFill &&
                        ctrl.BackColor != navBorderFirst &&
                        ctrl.BackColor != navBorderRest)
                {
                    ctrl.BackColor = panelFill;
                }

                ctrl.Font = new Font(defaultFont.FontFamily, defaultFont.Size, ctrl.Font.Style);
                ctrl.ForeColor = Color.Black;

                if (ctrl is DataGridView dgv)
                {
                    dgv.ColumnHeadersDefaultCellStyle.Font = ctrl.Font;
                    dgv.DefaultCellStyle.Font = ctrl.Font;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                    dgv.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            foreach (var ctrl in GetAllControls(form))
            {
                if (form.NavigationButtons.Contains(ctrl) || form.WindowButtons.Contains(ctrl))
                    continue;
                if (ctrl is Guna2Button g)
                {
                    g.FillColor = Color.FromArgb(94, 148, 255);
                    g.ForeColor = Color.White;
                }
                else if (ctrl is Button b)
                {
                    b.BackColor = panelFill;
                    b.ForeColor = Color.White;
                }
            }
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

        public static void ApplyTo(FaceControl form)
        {
            var s = Current;
            if (!string.IsNullOrEmpty(s.NavFillColor))
            {
                var c = ColorTranslator.FromHtml(s.NavFillColor);
                foreach (var btn in form.NavigationButtons)
                    btn.FillColor = c;
                foreach (var btn in form.WindowButtons)
                {
                    btn.FillColor = c;
                    btn.ForeColor = Color.Black;
                }
                form.TitlePanel.BackColor = c;
                foreach (var tab in GetAllControls(form).OfType<Guna2TabControl>())
                {
                    tab.TabMenuBackColor = c;
                    tab.TabButtonIdleState.FillColor = c;
                    tab.TabButtonSelectedState.FillColor = c;
                }
            }
            if (!string.IsNullOrEmpty(s.NavBorderColor))
            {
                var c = ColorTranslator.FromHtml(s.NavBorderColor);
                form.SetActiveBorderColor(c);
                foreach (var tab in GetAllControls(form).OfType<Guna2TabControl>())
                    tab.TabButtonSelectedState.InnerColor = c;
            }
            if (!string.IsNullOrEmpty(s.GlobalButtonColor))
            {
                var c = ColorTranslator.FromHtml(s.GlobalButtonColor);
                var excluded = new HashSet<string>
                {
                    "guna2BtnSidebarToggle",
                    "guna2BtnControlToggle",
                    "guna2BtnDatabase",
                    "guna2BtnAddPerson",
                    "guna2BtnCreateReport",
                    "guna2BtnTelegramBot",
                    "guna2BtnSettings",
                    "guna2Panel1",
                    "guna2BtnResize",
                    "guna2BtnMinimize",
                    "guna2BtnClose"
                };
                foreach (var ctrl in GetAllControls(form))
                {
                    if (excluded.Contains(ctrl.Name))
                        continue;

                    if (ctrl is Guna2Button g)
                        g.FillColor = c;
                    else if (ctrl is Button b)
                        b.BackColor = c;
                }
            }
            if (!string.IsNullOrEmpty(s.PanelFillColor))
            {
                var c = ColorTranslator.FromHtml(s.PanelFillColor);
                var target = Color.FromArgb(157, 193, 131);
                foreach (var ctrl in GetAllControls(form))
                {
                    var prop = ctrl.GetType().GetProperty("FillColor");
                    if (prop != null && prop.PropertyType == typeof(Color) && (Color)prop.GetValue(ctrl) == target)
                        prop.SetValue(ctrl, c);
                    if (ctrl.BackColor == target)
                        ctrl.BackColor = c;
                }
            }
            if (!string.IsNullOrEmpty(s.FontFamily))
            {
                try
                {
                    var fam = new FontFamily(s.FontFamily);
                    foreach (var ctrl in GetAllControls(form))
                    {
                        ctrl.Font = new Font(fam, s.FontSize ?? ctrl.Font.Size, ctrl.Font.Style);
                        if (ctrl is DataGridView dgv)
                        {
                            dgv.ColumnHeadersDefaultCellStyle.Font = ctrl.Font;
                            dgv.DefaultCellStyle.Font = ctrl.Font;
                        }
                    }
                }
                catch { }
            }
            else if (s.FontSize.HasValue)
            {
                foreach (var ctrl in GetAllControls(form))
                {
                    ctrl.Font = new Font(ctrl.Font.FontFamily, s.FontSize.Value, ctrl.Font.Style);
                    if (ctrl is DataGridView dgv)
                    {
                        dgv.ColumnHeadersDefaultCellStyle.Font = ctrl.Font;
                        dgv.DefaultCellStyle.Font = ctrl.Font;
                    }
                }
            }
            if (!string.IsNullOrEmpty(s.FontColor))
            {
                var c = ColorTranslator.FromHtml(s.FontColor);
                foreach (var ctrl in GetAllControls(form))
                {
                    ctrl.ForeColor = c;
                    if (ctrl is DataGridView dgv)
                    {
                        dgv.ColumnHeadersDefaultCellStyle.ForeColor = c;
                        dgv.DefaultCellStyle.ForeColor = c;
                    }
                    else if (ctrl is Guna2TabControl tab)
                    {
                        tab.TabButtonHoverState.ForeColor = c;
                        tab.TabButtonIdleState.ForeColor = c;
                        tab.TabButtonSelectedState.ForeColor = c;
                        foreach (TabPage page in tab.TabPages)
                            page.ForeColor = c;
                    }
                }
            }
        }
        #endregion
    }
}

