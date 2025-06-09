using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Diploma.Classes;

namespace Diploma
{
    public partial class FaceControl : Form
    {
        #region Поля
        private UserInterfaceManager uiManager;
        private ContentLoader contentLoader;
        private readonly Dictionary<Type, UserControl> _pages = new Dictionary<Type, UserControl>();
        #endregion

        public IEnumerable<Guna2Button> NavigationButtons { get; private set; }
        public IEnumerable<Guna2Button> WindowButtons { get; private set; }
        public TableLayoutPanel NavPanel => panelNavButtons;
        public Guna2Panel TitlePanel => guna2Panel1;

        #region Конструктор
        public FaceControl()
        {
            InitializeComponent();

            var navButtons = new List<Guna2Button>
            {
                guna2BtnSidebarToggle,
                guna2BtnControlToggle,
                guna2BtnDatabase,
                guna2BtnAddPerson,
                guna2BtnCreateReport,
                guna2BtnTelegramBot,
                guna2BtnSettings
            };

            NavigationButtons = navButtons;
            WindowButtons = new[] { guna2BtnResize, guna2BtnMinimize, guna2BtnClose };

            uiManager = new UserInterfaceManager(this, panelNavButtons, guna2BtnSidebarToggle, navButtons);
            uiManager.ApplyLayout();
            contentLoader = new ContentLoader(panelMainContent);
            uiManager.HighlightButton(guna2BtnControlToggle);
            contentLoader.Load(GetPage<EnableControl>());

            var dbPage = GetPage<DatabaseControl>();
            dbPage.DataImported += (s, e) => GetPage<AddPersonControl>().ReloadReferenceData();
        }
        #endregion

        #region Обработчики
        private void guna2BtnSidebarToggle_Click(object sender, EventArgs e) => uiManager.ToggleSidebar();
        private void guna2BtnMinimize_Click(object sender, EventArgs e) => uiManager.ToggleWindowSize();
        private void guna2BtnClose_Click(object sender, EventArgs e) => Application.Exit();
        private void guna2BtnResize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void guna2BtnControlToggle_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnControlToggle);
            contentLoader.Load(GetPage<EnableControl>());
        }

        private void guna2BtnDatabase_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnDatabase);
            contentLoader.Load(GetPage<DatabaseControl>());
        }

        private void guna2BtnAddPerson_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnAddPerson);
            contentLoader.Load(GetPage<AddPersonControl>());
        }

        private void guna2BtnCreateReport_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnCreateReport);
            contentLoader.Load(GetPage<ReportControl>());
        }

        private void guna2BtnTelegramBot_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnTelegramBot);
            contentLoader.Load(GetPage<TelegramBotControl>());
        }

        private void guna2BtnSettings_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnSettings);
            contentLoader.Load(GetPage<SettingsControl>());
        }

        public void SetActiveBorderColor(Color color)
        {
            uiManager.HighlightColor = color;
            if (uiManager.ActiveButton != null)
                uiManager.HighlightButton(uiManager.ActiveButton);
        }

        private T GetPage<T>() where T : UserControl, new()
        {
            if (!_pages.TryGetValue(typeof(T), out var ctrl))
            {
                ctrl = new T();
                _pages[typeof(T)] = ctrl;
            }
            return (T)ctrl;
        }
        #endregion
    }
}
