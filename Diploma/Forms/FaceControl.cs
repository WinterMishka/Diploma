using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Diploma
{
    public partial class FaceControl : Form
    {
        #region Поля
        private UserInterfaceManager uiManager;
        private ContentLoader contentLoader;
        private EnableControl enableControl;
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
            enableControl = new EnableControl();
            uiManager.HighlightButton(guna2BtnControlToggle);
            contentLoader.Load(enableControl);
        }
        #endregion

        #region Обработчики
        private void guna2BtnSidebarToggle_Click(object sender, EventArgs e) => uiManager.ToggleSidebar();
        private void guna2BtnMinimize_Click(object sender, EventArgs e) => uiManager.ToggleWindowSize();
        private void guna2BtnClose_Click(object sender, EventArgs e) => Application.Exit();
        private void guna2BtnResize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void StopRecognitionIfNeeded()
        {
            if (enableControl != null && contentLoader.CurrentControl == enableControl)
                enableControl.StopRecognition();
        }

        private void guna2BtnControlToggle_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnControlToggle);
            if (enableControl == null)
                enableControl = new EnableControl();
            contentLoader.Load(enableControl);
        }

        private void guna2BtnDatabase_Click(object sender, EventArgs e)
        {
            StopRecognitionIfNeeded();
            uiManager.HighlightButton(guna2BtnDatabase);
            contentLoader.Load(new DatabaseControl());
        }

        private void guna2BtnAddPerson_Click(object sender, EventArgs e)
        {
            StopRecognitionIfNeeded();
            uiManager.HighlightButton(guna2BtnAddPerson);
            contentLoader.Load(new AddPersonControl());
        }

        private void guna2BtnCreateReport_Click(object sender, EventArgs e)
        {
            StopRecognitionIfNeeded();
            uiManager.HighlightButton(guna2BtnCreateReport);
            contentLoader.Load(new ReportControl());
        }

        private void guna2BtnTelegramBot_Click(object sender, EventArgs e)
        {
            StopRecognitionIfNeeded();
            uiManager.HighlightButton(guna2BtnTelegramBot);
            contentLoader.Load(new TelegramBotControl());
        }

        private void guna2BtnSettings_Click(object sender, EventArgs e)
        {
            StopRecognitionIfNeeded();
            uiManager.HighlightButton(guna2BtnSettings);
            contentLoader.Load(new SettingsControl());
        }

        public void SetActiveBorderColor(Color color)
        {
            uiManager.HighlightColor = color;
            if (uiManager.ActiveButton != null)
                uiManager.HighlightButton(uiManager.ActiveButton);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopRecognitionIfNeeded();
            if (contentLoader.CurrentControl is AddPersonControl add)
                add.DisposeCamera();
            base.OnFormClosing(e);
        }
        #endregion
    }
}
