namespace Diploma
{
    partial class FaceControl
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceControl));
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2BtnResize = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnMinimize = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnClose = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.panelMainContent = new System.Windows.Forms.Panel();
            this.guna2BtnSidebarToggle = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnControlToggle = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnDatabase = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnCreateReport = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnTelegramBot = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnSettings = new Guna.UI2.WinForms.Guna2Button();
            this.panelNavButtons = new System.Windows.Forms.TableLayoutPanel();
            this.guna2BtnAddPerson = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.panelNavButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 30;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.DarkCyan;
            this.guna2Panel1.Controls.Add(this.tableLayoutPanel2);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1200, 30);
            this.guna2Panel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.guna2BtnResize, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.guna2BtnMinimize, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.guna2BtnClose, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1110, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(90, 30);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // guna2BtnResize
            // 
            this.guna2BtnResize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnResize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnResize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnResize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnResize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnResize.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnResize.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.guna2BtnResize.ForeColor = System.Drawing.Color.White;
            this.guna2BtnResize.Location = new System.Drawing.Point(0, 0);
            this.guna2BtnResize.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnResize.Name = "guna2BtnResize";
            this.guna2BtnResize.Size = new System.Drawing.Size(30, 30);
            this.guna2BtnResize.TabIndex = 1;
            this.guna2BtnResize.Text = "_";
            this.guna2BtnResize.Click += new System.EventHandler(this.guna2BtnResize_Click);
            // 
            // guna2BtnMinimize
            // 
            this.guna2BtnMinimize.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnMinimize.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnMinimize.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnMinimize.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnMinimize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnMinimize.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnMinimize.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold);
            this.guna2BtnMinimize.ForeColor = System.Drawing.Color.White;
            this.guna2BtnMinimize.Location = new System.Drawing.Point(30, 0);
            this.guna2BtnMinimize.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnMinimize.Name = "guna2BtnMinimize";
            this.guna2BtnMinimize.Size = new System.Drawing.Size(30, 30);
            this.guna2BtnMinimize.TabIndex = 0;
            this.guna2BtnMinimize.Text = "▢";
            this.guna2BtnMinimize.Click += new System.EventHandler(this.guna2BtnMinimize_Click);
            // 
            // guna2BtnClose
            // 
            this.guna2BtnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnClose.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnClose.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnClose.ForeColor = System.Drawing.Color.White;
            this.guna2BtnClose.Location = new System.Drawing.Point(60, 0);
            this.guna2BtnClose.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnClose.Name = "guna2BtnClose";
            this.guna2BtnClose.Size = new System.Drawing.Size(30, 30);
            this.guna2BtnClose.TabIndex = 0;
            this.guna2BtnClose.Text = "×";
            this.guna2BtnClose.Click += new System.EventHandler(this.guna2BtnClose_Click);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2Panel2.Controls.Add(this.panelMainContent);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2Panel2.Location = new System.Drawing.Point(200, 30);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(1000, 620);
            this.guna2Panel2.TabIndex = 2;
            // 
            // panelMainContent
            // 
            this.panelMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainContent.Location = new System.Drawing.Point(0, 0);
            this.panelMainContent.Margin = new System.Windows.Forms.Padding(0);
            this.panelMainContent.Name = "panelMainContent";
            this.panelMainContent.Size = new System.Drawing.Size(1000, 620);
            this.panelMainContent.TabIndex = 0;
            // 
            // guna2BtnSidebarToggle
            // 
            this.guna2BtnSidebarToggle.Animated = true;
            this.guna2BtnSidebarToggle.CustomBorderColor = System.Drawing.Color.ForestGreen;
            this.guna2BtnSidebarToggle.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnSidebarToggle.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSidebarToggle.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSidebarToggle.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnSidebarToggle.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnSidebarToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnSidebarToggle.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnSidebarToggle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2BtnSidebarToggle.ForeColor = System.Drawing.Color.White;
            this.guna2BtnSidebarToggle.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnSidebarToggle.Image")));
            this.guna2BtnSidebarToggle.ImageSize = new System.Drawing.Size(140, 100);
            this.guna2BtnSidebarToggle.Location = new System.Drawing.Point(0, 0);
            this.guna2BtnSidebarToggle.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnSidebarToggle.Name = "guna2BtnSidebarToggle";
            this.guna2BtnSidebarToggle.Size = new System.Drawing.Size(200, 88);
            this.guna2BtnSidebarToggle.TabIndex = 0;
            this.guna2BtnSidebarToggle.Click += new System.EventHandler(this.guna2BtnSidebarToggle_Click);
            // 
            // guna2BtnControlToggle
            // 
            this.guna2BtnControlToggle.Animated = true;
            this.guna2BtnControlToggle.CustomBorderColor = System.Drawing.Color.White;
            this.guna2BtnControlToggle.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnControlToggle.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnControlToggle.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnControlToggle.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnControlToggle.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnControlToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnControlToggle.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnControlToggle.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnControlToggle.ForeColor = System.Drawing.Color.White;
            this.guna2BtnControlToggle.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnControlToggle.Image")));
            this.guna2BtnControlToggle.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2BtnControlToggle.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2BtnControlToggle.Location = new System.Drawing.Point(0, 88);
            this.guna2BtnControlToggle.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnControlToggle.Name = "guna2BtnControlToggle";
            this.guna2BtnControlToggle.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.guna2BtnControlToggle.Size = new System.Drawing.Size(200, 88);
            this.guna2BtnControlToggle.TabIndex = 1;
            this.guna2BtnControlToggle.Text = "Включить контроль";
            this.guna2BtnControlToggle.Click += new System.EventHandler(this.guna2BtnControlToggle_Click);
            // 
            // guna2BtnDatabase
            // 
            this.guna2BtnDatabase.Animated = true;
            this.guna2BtnDatabase.CustomBorderColor = System.Drawing.Color.White;
            this.guna2BtnDatabase.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnDatabase.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnDatabase.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnDatabase.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnDatabase.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnDatabase.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnDatabase.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.guna2BtnDatabase.ForeColor = System.Drawing.Color.White;
            this.guna2BtnDatabase.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnDatabase.Image")));
            this.guna2BtnDatabase.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2BtnDatabase.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2BtnDatabase.Location = new System.Drawing.Point(0, 176);
            this.guna2BtnDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnDatabase.Name = "guna2BtnDatabase";
            this.guna2BtnDatabase.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.guna2BtnDatabase.Size = new System.Drawing.Size(200, 88);
            this.guna2BtnDatabase.TabIndex = 2;
            this.guna2BtnDatabase.Text = "База данных";
            this.guna2BtnDatabase.Click += new System.EventHandler(this.guna2BtnDatabase_Click);
            // 
            // guna2BtnCreateReport
            // 
            this.guna2BtnCreateReport.Animated = true;
            this.guna2BtnCreateReport.CustomBorderColor = System.Drawing.Color.White;
            this.guna2BtnCreateReport.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnCreateReport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnCreateReport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnCreateReport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnCreateReport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnCreateReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnCreateReport.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnCreateReport.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.guna2BtnCreateReport.ForeColor = System.Drawing.Color.White;
            this.guna2BtnCreateReport.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnCreateReport.Image")));
            this.guna2BtnCreateReport.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2BtnCreateReport.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2BtnCreateReport.Location = new System.Drawing.Point(0, 352);
            this.guna2BtnCreateReport.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnCreateReport.Name = "guna2BtnCreateReport";
            this.guna2BtnCreateReport.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.guna2BtnCreateReport.Size = new System.Drawing.Size(200, 88);
            this.guna2BtnCreateReport.TabIndex = 4;
            this.guna2BtnCreateReport.Text = "Создать отчёт";
            this.guna2BtnCreateReport.Click += new System.EventHandler(this.guna2BtnCreateReport_Click);
            // 
            // guna2BtnTelegramBot
            // 
            this.guna2BtnTelegramBot.Animated = true;
            this.guna2BtnTelegramBot.CustomBorderColor = System.Drawing.Color.White;
            this.guna2BtnTelegramBot.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnTelegramBot.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnTelegramBot.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnTelegramBot.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnTelegramBot.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnTelegramBot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnTelegramBot.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnTelegramBot.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.guna2BtnTelegramBot.ForeColor = System.Drawing.Color.White;
            this.guna2BtnTelegramBot.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnTelegramBot.Image")));
            this.guna2BtnTelegramBot.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2BtnTelegramBot.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2BtnTelegramBot.Location = new System.Drawing.Point(0, 440);
            this.guna2BtnTelegramBot.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnTelegramBot.Name = "guna2BtnTelegramBot";
            this.guna2BtnTelegramBot.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.guna2BtnTelegramBot.Size = new System.Drawing.Size(200, 88);
            this.guna2BtnTelegramBot.TabIndex = 5;
            this.guna2BtnTelegramBot.Text = "Телеграмм-бот";
            this.guna2BtnTelegramBot.Click += new System.EventHandler(this.guna2BtnTelegramBot_Click);
            // 
            // guna2BtnSettings
            // 
            this.guna2BtnSettings.Animated = true;
            this.guna2BtnSettings.CustomBorderColor = System.Drawing.Color.White;
            this.guna2BtnSettings.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnSettings.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSettings.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSettings.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnSettings.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnSettings.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnSettings.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.guna2BtnSettings.ForeColor = System.Drawing.Color.White;
            this.guna2BtnSettings.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnSettings.Image")));
            this.guna2BtnSettings.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2BtnSettings.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2BtnSettings.Location = new System.Drawing.Point(0, 528);
            this.guna2BtnSettings.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnSettings.Name = "guna2BtnSettings";
            this.guna2BtnSettings.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.guna2BtnSettings.Size = new System.Drawing.Size(200, 92);
            this.guna2BtnSettings.TabIndex = 6;
            this.guna2BtnSettings.Text = "Настройки";
            this.guna2BtnSettings.Click += new System.EventHandler(this.guna2BtnSettings_Click);
            // 
            // panelNavButtons
            // 
            this.panelNavButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.panelNavButtons.ColumnCount = 1;
            this.panelNavButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelNavButtons.Controls.Add(this.guna2BtnSettings, 0, 6);
            this.panelNavButtons.Controls.Add(this.guna2BtnTelegramBot, 0, 5);
            this.panelNavButtons.Controls.Add(this.guna2BtnCreateReport, 0, 4);
            this.panelNavButtons.Controls.Add(this.guna2BtnAddPerson, 0, 3);
            this.panelNavButtons.Controls.Add(this.guna2BtnDatabase, 0, 2);
            this.panelNavButtons.Controls.Add(this.guna2BtnControlToggle, 0, 1);
            this.panelNavButtons.Controls.Add(this.guna2BtnSidebarToggle, 0, 0);
            this.panelNavButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNavButtons.Location = new System.Drawing.Point(0, 30);
            this.panelNavButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panelNavButtons.Name = "panelNavButtons";
            this.panelNavButtons.RowCount = 7;
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.panelNavButtons.Size = new System.Drawing.Size(200, 620);
            this.panelNavButtons.TabIndex = 1;
            // 
            // guna2BtnAddPerson
            // 
            this.guna2BtnAddPerson.Animated = true;
            this.guna2BtnAddPerson.CustomBorderColor = System.Drawing.Color.White;
            this.guna2BtnAddPerson.CustomBorderThickness = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.guna2BtnAddPerson.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnAddPerson.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnAddPerson.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnAddPerson.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnAddPerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnAddPerson.FillColor = System.Drawing.Color.DarkCyan;
            this.guna2BtnAddPerson.Font = new System.Drawing.Font("Verdana", 14.25F);
            this.guna2BtnAddPerson.ForeColor = System.Drawing.Color.White;
            this.guna2BtnAddPerson.Image = ((System.Drawing.Image)(resources.GetObject("guna2BtnAddPerson.Image")));
            this.guna2BtnAddPerson.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2BtnAddPerson.ImageSize = new System.Drawing.Size(50, 50);
            this.guna2BtnAddPerson.Location = new System.Drawing.Point(0, 264);
            this.guna2BtnAddPerson.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnAddPerson.Name = "guna2BtnAddPerson";
            this.guna2BtnAddPerson.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.guna2BtnAddPerson.Size = new System.Drawing.Size(200, 88);
            this.guna2BtnAddPerson.TabIndex = 3;
            this.guna2BtnAddPerson.Text = "Добавить лицо";
            this.guna2BtnAddPerson.Click += new System.EventHandler(this.guna2BtnAddPerson_Click);
            // 
            // FaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.panelNavButtons);
            this.Controls.Add(this.guna2Panel1);
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "FaceControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.guna2Panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.guna2Panel2.ResumeLayout(false);
            this.panelNavButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Button guna2BtnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Guna.UI2.WinForms.Guna2Button guna2BtnResize;
        private Guna.UI2.WinForms.Guna2Button guna2BtnMinimize;
        private System.Windows.Forms.TableLayoutPanel panelNavButtons;
        private Guna.UI2.WinForms.Guna2Button guna2BtnSettings;
        private Guna.UI2.WinForms.Guna2Button guna2BtnTelegramBot;
        private Guna.UI2.WinForms.Guna2Button guna2BtnCreateReport;
        private Guna.UI2.WinForms.Guna2Button guna2BtnDatabase;
        private Guna.UI2.WinForms.Guna2Button guna2BtnControlToggle;
        private Guna.UI2.WinForms.Guna2Button guna2BtnSidebarToggle;
        private System.Windows.Forms.Panel panelMainContent;
        private Guna.UI2.WinForms.Guna2Button guna2BtnAddPerson;
    }
}

