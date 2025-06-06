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
        public string NavFillColor { get; set; }
        public string NavBorderColor { get; set; }
        public string GlobalButtonColor { get; set; }
        public string PanelFillColor { get; set; }
        public string FontFamily { get; set; }
        public float? FontSize { get; set; }
        public bool StartFullScreen { get; set; }
    }

    public static class UiSettingsManager
    {
        private static readonly string FilePath = Path.Combine(Application.StartupPath, "ui_settings.json");

        public static UiSettingsData Current { get; private set; } = new UiSettingsData();

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
            var navBorder = Color.ForestGreen;
            var panelFill = Color.FromArgb(157, 193, 131);

            foreach (var btn in form.NavigationButtons)
            {
                btn.FillColor = navFill;
                btn.CustomBorderColor = navBorder;
            }
            foreach (var btn in form.WindowButtons)
                btn.FillColor = navFill;

            form.TitlePanel.BackColor = navFill;
            form.SetActiveBorderColor(navBorder);

            foreach (var ctrl in GetAllControls(form))
            {
                if (ctrl is Guna2Button g)
                    g.FillColor = navFill;
                else if (ctrl is Button b)
                    b.BackColor = navFill;

                var prop = ctrl.GetType().GetProperty("FillColor");
                if (prop != null && prop.PropertyType == typeof(Color))
                    prop.SetValue(ctrl, panelFill);
                if (ctrl.BackColor != navFill && ctrl.BackColor != navBorder)
                    ctrl.BackColor = panelFill;

                if (ctrl is Guna2TabControl tab)
                {
                    tab.TabMenuBackColor = navFill;
                    tab.TabButtonIdleState.FillColor = navFill;
                    tab.TabButtonSelectedState.FillColor = navFill;
                    tab.TabButtonSelectedState.InnerColor = navBorder;
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
                    btn.FillColor = c;
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
                foreach (var btn in form.NavigationButtons)
                    btn.CustomBorderColor = c;
                foreach (var tab in GetAllControls(form).OfType<Guna2TabControl>())
                    tab.TabButtonSelectedState.InnerColor = c;
            }
            if (!string.IsNullOrEmpty(s.GlobalButtonColor))
            {
                var c = ColorTranslator.FromHtml(s.GlobalButtonColor);
                foreach (var ctrl in GetAllControls(form))
                {
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
                        if (ctrl.GetType().GetProperty("Text") != null)
                            ctrl.Font = new Font(fam, s.FontSize ?? ctrl.Font.Size, ctrl.Font.Style);
                }
                catch { }
            }
            else if (s.FontSize.HasValue)
            {
                foreach (var ctrl in GetAllControls(form))
                    if (ctrl.GetType().GetProperty("Text") != null)
                        ctrl.Font = new Font(ctrl.Font.FontFamily, s.FontSize.Value, ctrl.Font.Style);
            }
        }
    }
}

