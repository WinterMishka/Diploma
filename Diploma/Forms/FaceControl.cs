using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Diploma
{
    public partial class FaceControl : Form
    {
        private UserInterfaceManager uiManager;
        private ContentLoader contentLoader;

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

            uiManager = new UserInterfaceManager(this, panelNavButtons, guna2BtnSidebarToggle, navButtons);
            uiManager.ApplyLayout();
            contentLoader = new ContentLoader(panelMainContent);
        }

        private void guna2BtnSidebarToggle_Click(object sender, EventArgs e) => uiManager.ToggleSidebar();
        private void guna2BtnMinimize_Click(object sender, EventArgs e) => uiManager.ToggleWindowSize();
        private void guna2BtnClose_Click(object sender, EventArgs e) => Application.Exit();
        private void guna2BtnResize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

        private void guna2BtnControlToggle_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnControlToggle);
            contentLoader.Load(new EnableControl());
        }

        private void guna2BtnDatabase_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnDatabase);
            contentLoader.Load(new DatabaseControl());
        }

        private void guna2BtnAddPerson_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnAddPerson);
            contentLoader.Load(new AddPersonControl());
        }

        private void guna2BtnCreateReport_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnCreateReport);
            contentLoader.Load(new ReportControl());
        }

        private void guna2BtnTelegramBot_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnTelegramBot);
            contentLoader.Load(new TelegramBotControl());
        }

        private void guna2BtnSettings_Click(object sender, EventArgs e)
        {
            uiManager.HighlightButton(guna2BtnSettings);
            contentLoader.Load(new SettingsControl());
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (contentLoader.CurrentControl is AddPersonControl add)
                add.DisposeCamera();
            base.OnFormClosing(e);

        }
    }
}
