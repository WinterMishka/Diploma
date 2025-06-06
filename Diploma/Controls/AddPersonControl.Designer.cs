using System;

namespace Diploma
{
    partial class AddPersonControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2BtnSnap = new Guna.UI2.WinForms.Guna2Button();
            this.flpThumbnails = new System.Windows.Forms.FlowLayoutPanel();
            this.guna2PbLiveCamera = new Guna.UI2.WinForms.Guna2PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2RadioButton2 = new Guna.UI2.WinForms.Guna2RadioButton();
            this.guna2RadioButton1 = new Guna.UI2.WinForms.Guna2RadioButton();
            this.lstStatuses = new System.Windows.Forms.ListBox();
            this.lstSpecialities = new System.Windows.Forms.ListBox();
            this.lstCourses = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rdoEmployee = new Guna.UI2.WinForms.Guna2RadioButton();
            this.rdoStudent = new Guna.UI2.WinForms.Guna2RadioButton();
            this.lblFullName = new System.Windows.Forms.Label();
            this.guna2BtnSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BtnClear = new Guna.UI2.WinForms.Guna2Button();
            this.lblType = new System.Windows.Forms.Label();
            this.lblGroup = new System.Windows.Forms.Label();
            this.lblCourse = new System.Windows.Forms.Label();
            this.lblSpeciality = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lstGroups = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PbLiveCamera)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 620);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.guna2BtnSnap, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.flpThumbnails, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.guna2PbLiveCamera, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(300, 620);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // guna2BtnSnap
            // 
            this.guna2BtnSnap.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSnap.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSnap.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnSnap.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnSnap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnSnap.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnSnap.ForeColor = System.Drawing.Color.White;
            this.guna2BtnSnap.Location = new System.Drawing.Point(0, 279);
            this.guna2BtnSnap.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnSnap.Name = "guna2BtnSnap";
            this.guna2BtnSnap.Size = new System.Drawing.Size(300, 62);
            this.guna2BtnSnap.TabIndex = 1;
            this.guna2BtnSnap.Text = "Сделать фотографию";
            this.guna2BtnSnap.Click += new System.EventHandler(this.guna2BtnSnap_Click);
            // 
            // flpThumbnails
            // 
            this.flpThumbnails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpThumbnails.Location = new System.Drawing.Point(3, 344);
            this.flpThumbnails.Name = "flpThumbnails";
            this.flpThumbnails.Size = new System.Drawing.Size(294, 273);
            this.flpThumbnails.TabIndex = 2;
            // 
            // guna2PbLiveCamera
            // 
            this.guna2PbLiveCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PbLiveCamera.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2PbLiveCamera.ImageRotate = 0F;
            this.guna2PbLiveCamera.Location = new System.Drawing.Point(0, 0);
            this.guna2PbLiveCamera.Margin = new System.Windows.Forms.Padding(0);
            this.guna2PbLiveCamera.Name = "guna2PbLiveCamera";
            this.guna2PbLiveCamera.Size = new System.Drawing.Size(300, 279);
            this.guna2PbLiveCamera.TabIndex = 3;
            this.guna2PbLiveCamera.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(300, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(700, 620);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.guna2RadioButton2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.guna2RadioButton1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lstStatuses, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.lstSpecialities, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.lstCourses, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblFullName, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.guna2BtnSave, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.guna2BtnClear, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.lblType, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblGroup, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblCourse, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.lblSpeciality, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.lblStatus, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.lstGroups, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.comboBox1, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(700, 620);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // guna2RadioButton2
            // 
            this.guna2RadioButton2.AutoSize = true;
            this.guna2RadioButton2.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2RadioButton2.CheckedState.BorderThickness = 0;
            this.guna2RadioButton2.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2RadioButton2.CheckedState.InnerColor = System.Drawing.Color.White;
            this.guna2RadioButton2.CheckedState.InnerOffset = -4;
            this.guna2RadioButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2RadioButton2.Location = new System.Drawing.Point(350, 0);
            this.guna2RadioButton2.Margin = new System.Windows.Forms.Padding(0);
            this.guna2RadioButton2.Name = "guna2RadioButton2";
            this.guna2RadioButton2.Size = new System.Drawing.Size(350, 77);
            this.guna2RadioButton2.TabIndex = 18;
            this.guna2RadioButton2.Text = "Вставить из файла";
            this.guna2RadioButton2.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2RadioButton2.UncheckedState.BorderThickness = 2;
            this.guna2RadioButton2.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2RadioButton2.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // guna2RadioButton1
            // 
            this.guna2RadioButton1.AutoSize = true;
            this.guna2RadioButton1.Checked = true;
            this.guna2RadioButton1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2RadioButton1.CheckedState.BorderThickness = 0;
            this.guna2RadioButton1.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2RadioButton1.CheckedState.InnerColor = System.Drawing.Color.White;
            this.guna2RadioButton1.CheckedState.InnerOffset = -4;
            this.guna2RadioButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2RadioButton1.Location = new System.Drawing.Point(0, 0);
            this.guna2RadioButton1.Margin = new System.Windows.Forms.Padding(0);
            this.guna2RadioButton1.Name = "guna2RadioButton1";
            this.guna2RadioButton1.Size = new System.Drawing.Size(350, 77);
            this.guna2RadioButton1.TabIndex = 17;
            this.guna2RadioButton1.TabStop = true;
            this.guna2RadioButton1.Text = "Вставить вручную";
            this.guna2RadioButton1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2RadioButton1.UncheckedState.BorderThickness = 2;
            this.guna2RadioButton1.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.guna2RadioButton1.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            // 
            // lstStatuses
            // 
            this.lstStatuses.DisplayMember = "название";
            this.lstStatuses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstStatuses.FormattingEnabled = true;
            this.lstStatuses.ItemHeight = 23;
            this.lstStatuses.Location = new System.Drawing.Point(350, 462);
            this.lstStatuses.Margin = new System.Windows.Forms.Padding(0);
            this.lstStatuses.Name = "lstStatuses";
            this.lstStatuses.Size = new System.Drawing.Size(350, 77);
            this.lstStatuses.TabIndex = 16;
            this.lstStatuses.ValueMember = "название";
            // 
            // lstSpecialities
            // 
            this.lstSpecialities.DisplayMember = "название";
            this.lstSpecialities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSpecialities.FormattingEnabled = true;
            this.lstSpecialities.ItemHeight = 23;
            this.lstSpecialities.Location = new System.Drawing.Point(350, 385);
            this.lstSpecialities.Margin = new System.Windows.Forms.Padding(0);
            this.lstSpecialities.Name = "lstSpecialities";
            this.lstSpecialities.Size = new System.Drawing.Size(350, 77);
            this.lstSpecialities.TabIndex = 15;
            this.lstSpecialities.ValueMember = "название";
            // 
            // lstCourses
            // 
            this.lstCourses.DisplayMember = "наименование";
            this.lstCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCourses.FormattingEnabled = true;
            this.lstCourses.ItemHeight = 23;
            this.lstCourses.Location = new System.Drawing.Point(350, 308);
            this.lstCourses.Margin = new System.Windows.Forms.Padding(0);
            this.lstCourses.Name = "lstCourses";
            this.lstCourses.Size = new System.Drawing.Size(350, 77);
            this.lstCourses.TabIndex = 14;
            this.lstCourses.ValueMember = "наименование";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.rdoEmployee, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.rdoStudent, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(350, 154);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(350, 77);
            this.tableLayoutPanel6.TabIndex = 11;
            // 
            // rdoEmployee
            // 
            this.rdoEmployee.AutoSize = true;
            this.rdoEmployee.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdoEmployee.CheckedState.BorderThickness = 0;
            this.rdoEmployee.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdoEmployee.CheckedState.InnerColor = System.Drawing.Color.White;
            this.rdoEmployee.CheckedState.InnerOffset = -4;
            this.rdoEmployee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoEmployee.Location = new System.Drawing.Point(175, 0);
            this.rdoEmployee.Margin = new System.Windows.Forms.Padding(0);
            this.rdoEmployee.Name = "rdoEmployee";
            this.rdoEmployee.Size = new System.Drawing.Size(175, 77);
            this.rdoEmployee.TabIndex = 1;
            this.rdoEmployee.Text = "Сотрудник";
            this.rdoEmployee.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.rdoEmployee.UncheckedState.BorderThickness = 2;
            this.rdoEmployee.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rdoEmployee.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rdoEmployee.CheckedChanged += new System.EventHandler(this.rdoEmployee_CheckedChanged);
            // 
            // rdoStudent
            // 
            this.rdoStudent.AutoSize = true;
            this.rdoStudent.Checked = true;
            this.rdoStudent.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdoStudent.CheckedState.BorderThickness = 0;
            this.rdoStudent.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.rdoStudent.CheckedState.InnerColor = System.Drawing.Color.White;
            this.rdoStudent.CheckedState.InnerOffset = -4;
            this.rdoStudent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoStudent.Location = new System.Drawing.Point(0, 0);
            this.rdoStudent.Margin = new System.Windows.Forms.Padding(0);
            this.rdoStudent.Name = "rdoStudent";
            this.rdoStudent.Size = new System.Drawing.Size(175, 77);
            this.rdoStudent.TabIndex = 0;
            this.rdoStudent.TabStop = true;
            this.rdoStudent.Text = "Студент";
            this.rdoStudent.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.rdoStudent.UncheckedState.BorderThickness = 2;
            this.rdoStudent.UncheckedState.FillColor = System.Drawing.Color.Transparent;
            this.rdoStudent.UncheckedState.InnerColor = System.Drawing.Color.Transparent;
            this.rdoStudent.CheckedChanged += new System.EventHandler(this.rdoStudent_CheckedChanged);
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFullName.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblFullName.Location = new System.Drawing.Point(3, 77);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(344, 77);
            this.lblFullName.TabIndex = 4;
            this.lblFullName.Text = "ФИО";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2BtnSave
            // 
            this.guna2BtnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnSave.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnSave.ForeColor = System.Drawing.Color.White;
            this.guna2BtnSave.Location = new System.Drawing.Point(0, 539);
            this.guna2BtnSave.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnSave.Name = "guna2BtnSave";
            this.guna2BtnSave.Size = new System.Drawing.Size(350, 81);
            this.guna2BtnSave.TabIndex = 2;
            this.guna2BtnSave.Text = "Сохранить";
            this.guna2BtnSave.Click += new System.EventHandler(this.guna2BtnSave_Click);
            // 
            // guna2BtnClear
            // 
            this.guna2BtnClear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnClear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnClear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnClear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnClear.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnClear.ForeColor = System.Drawing.Color.White;
            this.guna2BtnClear.Location = new System.Drawing.Point(350, 539);
            this.guna2BtnClear.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnClear.Name = "guna2BtnClear";
            this.guna2BtnClear.Size = new System.Drawing.Size(350, 81);
            this.guna2BtnClear.TabIndex = 3;
            this.guna2BtnClear.Text = "Сбросить значения";
            this.guna2BtnClear.Click += new System.EventHandler(this.guna2BtnClear_Click);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblType.Location = new System.Drawing.Point(3, 154);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(344, 77);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Тип";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGroup
            // 
            this.lblGroup.AutoSize = true;
            this.lblGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGroup.Location = new System.Drawing.Point(3, 231);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(344, 77);
            this.lblGroup.TabIndex = 6;
            this.lblGroup.Text = "Группа";
            this.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCourse
            // 
            this.lblCourse.AutoSize = true;
            this.lblCourse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCourse.Location = new System.Drawing.Point(3, 308);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new System.Drawing.Size(344, 77);
            this.lblCourse.TabIndex = 7;
            this.lblCourse.Text = "Курс";
            this.lblCourse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeciality
            // 
            this.lblSpeciality.AutoSize = true;
            this.lblSpeciality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSpeciality.Location = new System.Drawing.Point(3, 385);
            this.lblSpeciality.Name = "lblSpeciality";
            this.lblSpeciality.Size = new System.Drawing.Size(344, 77);
            this.lblSpeciality.TabIndex = 8;
            this.lblSpeciality.Text = "Специальность";
            this.lblSpeciality.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(3, 462);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(344, 77);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Статус/должность";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstGroups
            // 
            this.lstGroups.DisplayMember = "Название_полное";
            this.lstGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstGroups.FormattingEnabled = true;
            this.lstGroups.ItemHeight = 23;
            this.lstGroups.Location = new System.Drawing.Point(350, 231);
            this.lstGroups.Margin = new System.Windows.Forms.Padding(0);
            this.lstGroups.Name = "lstGroups";
            this.lstGroups.Size = new System.Drawing.Size(350, 77);
            this.lstGroups.TabIndex = 13;
            this.lstGroups.ValueMember = "Название_полное";
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(350, 77);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(350, 31);
            this.comboBox1.TabIndex = 19;
            // 
            // AddPersonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AddPersonControl";
            this.Size = new System.Drawing.Size(1000, 620);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PbLiveCamera)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Guna.UI2.WinForms.Guna2Button guna2BtnSnap;
        private System.Windows.Forms.FlowLayoutPanel flpThumbnails;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PbLiveCamera;
        private System.Windows.Forms.BindingSource учащиесяBindingSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Guna.UI2.WinForms.Guna2RadioButton guna2RadioButton2;
        private Guna.UI2.WinForms.Guna2RadioButton guna2RadioButton1;
        private System.Windows.Forms.ListBox lstStatuses;
        private System.Windows.Forms.ListBox lstSpecialities;
        private System.Windows.Forms.ListBox lstCourses;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Guna.UI2.WinForms.Guna2RadioButton rdoEmployee;
        private Guna.UI2.WinForms.Guna2RadioButton rdoStudent;
        private System.Windows.Forms.Label lblFullName;
        private Guna.UI2.WinForms.Guna2Button guna2BtnSave;
        private Guna.UI2.WinForms.Guna2Button guna2BtnClear;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.Label lblSpeciality;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListBox lstGroups;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
