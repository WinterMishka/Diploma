using Diploma;

namespace Diploma
{
    partial class EnableControl
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.guna2CheckBox2 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.guna2BtnStopRecognition = new Guna.UI2.WinForms.Guna2Button();
            this.guna2PanelLastCapturedFace = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2PanelSelectedFace = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.guna2BtnStartRecognition = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CheckBox1 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.guna2PictureBoxLiveCamera = new Guna.UI2.WinForms.Guna2PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ФИО = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Статус = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Дата = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Время = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBoxLiveCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 620);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.guna2PictureBoxLiveCamera, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1000, 403);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.guna2CheckBox2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.guna2BtnStopRecognition, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.guna2PanelLastCapturedFace, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.guna2PanelSelectedFace, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.guna2BtnStartRecognition, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.guna2CheckBox1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(500, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(500, 403);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // guna2CheckBox2
            // 
            this.guna2CheckBox2.AutoSize = true;
            this.guna2CheckBox2.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBox2.CheckedState.BorderRadius = 0;
            this.guna2CheckBox2.CheckedState.BorderThickness = 0;
            this.guna2CheckBox2.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CheckBox2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2CheckBox2.Location = new System.Drawing.Point(250, 0);
            this.guna2CheckBox2.Margin = new System.Windows.Forms.Padding(0);
            this.guna2CheckBox2.Name = "guna2CheckBox2";
            this.guna2CheckBox2.Size = new System.Drawing.Size(250, 60);
            this.guna2CheckBox2.TabIndex = 5;
            this.guna2CheckBox2.Text = "Уход";
            this.guna2CheckBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.guna2CheckBox2.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CheckBox2.UncheckedState.BorderRadius = 0;
            this.guna2CheckBox2.UncheckedState.BorderThickness = 0;
            this.guna2CheckBox2.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // guna2BtnStopRecognition
            // 
            this.guna2BtnStopRecognition.Animated = true;
            this.guna2BtnStopRecognition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2BtnStopRecognition.BorderRadius = 30;
            this.guna2BtnStopRecognition.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnStopRecognition.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnStopRecognition.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnStopRecognition.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnStopRecognition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnStopRecognition.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(64)))), ((int)(((byte)(40)))));
            this.guna2BtnStopRecognition.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnStopRecognition.ForeColor = System.Drawing.Color.White;
            this.guna2BtnStopRecognition.Location = new System.Drawing.Point(250, 60);
            this.guna2BtnStopRecognition.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnStopRecognition.Name = "guna2BtnStopRecognition";
            this.guna2BtnStopRecognition.Size = new System.Drawing.Size(250, 60);
            this.guna2BtnStopRecognition.TabIndex = 3;
            this.guna2BtnStopRecognition.Text = "Выключить";
            this.guna2BtnStopRecognition.Click += new System.EventHandler(this.guna2BtnStopRecognition_Click);
            // 
            // guna2PanelLastCapturedFace
            // 
            this.guna2PanelLastCapturedFace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2PanelLastCapturedFace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelLastCapturedFace.Location = new System.Drawing.Point(0, 120);
            this.guna2PanelLastCapturedFace.Margin = new System.Windows.Forms.Padding(0);
            this.guna2PanelLastCapturedFace.Name = "guna2PanelLastCapturedFace";
            this.guna2PanelLastCapturedFace.Size = new System.Drawing.Size(250, 283);
            this.guna2PanelLastCapturedFace.TabIndex = 0;
            // 
            // guna2PanelSelectedFace
            // 
            this.guna2PanelSelectedFace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2PanelSelectedFace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PanelSelectedFace.FillColor = System.Drawing.Color.Empty;
            this.guna2PanelSelectedFace.FillColor2 = System.Drawing.Color.Empty;
            this.guna2PanelSelectedFace.FillColor3 = System.Drawing.Color.Empty;
            this.guna2PanelSelectedFace.FillColor4 = System.Drawing.Color.Empty;
            this.guna2PanelSelectedFace.Location = new System.Drawing.Point(250, 120);
            this.guna2PanelSelectedFace.Margin = new System.Windows.Forms.Padding(0);
            this.guna2PanelSelectedFace.Name = "guna2PanelSelectedFace";
            this.guna2PanelSelectedFace.Size = new System.Drawing.Size(250, 283);
            this.guna2PanelSelectedFace.TabIndex = 1;
            // 
            // guna2BtnStartRecognition
            // 
            this.guna2BtnStartRecognition.Animated = true;
            this.guna2BtnStartRecognition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2BtnStartRecognition.BorderRadius = 30;
            this.guna2BtnStartRecognition.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnStartRecognition.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2BtnStartRecognition.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2BtnStartRecognition.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2BtnStartRecognition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2BtnStartRecognition.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(64)))), ((int)(((byte)(40)))));
            this.guna2BtnStartRecognition.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2BtnStartRecognition.ForeColor = System.Drawing.Color.White;
            this.guna2BtnStartRecognition.Location = new System.Drawing.Point(0, 60);
            this.guna2BtnStartRecognition.Margin = new System.Windows.Forms.Padding(0);
            this.guna2BtnStartRecognition.Name = "guna2BtnStartRecognition";
            this.guna2BtnStartRecognition.Size = new System.Drawing.Size(250, 60);
            this.guna2BtnStartRecognition.TabIndex = 2;
            this.guna2BtnStartRecognition.Text = "Включить";
            this.guna2BtnStartRecognition.Click += new System.EventHandler(this.guna2BtnStartRecognition_Click);
            // 
            // guna2CheckBox1
            // 
            this.guna2CheckBox1.AutoSize = true;
            this.guna2CheckBox1.Checked = true;
            this.guna2CheckBox1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBox1.CheckedState.BorderRadius = 0;
            this.guna2CheckBox1.CheckedState.BorderThickness = 0;
            this.guna2CheckBox1.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.guna2CheckBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2CheckBox1.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.guna2CheckBox1.Location = new System.Drawing.Point(0, 0);
            this.guna2CheckBox1.Margin = new System.Windows.Forms.Padding(0);
            this.guna2CheckBox1.Name = "guna2CheckBox1";
            this.guna2CheckBox1.Size = new System.Drawing.Size(250, 60);
            this.guna2CheckBox1.TabIndex = 4;
            this.guna2CheckBox1.Text = "Приход";
            this.guna2CheckBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.guna2CheckBox1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CheckBox1.UncheckedState.BorderRadius = 0;
            this.guna2CheckBox1.UncheckedState.BorderThickness = 0;
            this.guna2CheckBox1.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // guna2PictureBoxLiveCamera
            // 
            this.guna2PictureBoxLiveCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PictureBoxLiveCamera.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.guna2PictureBoxLiveCamera.ImageRotate = 0F;
            this.guna2PictureBoxLiveCamera.Location = new System.Drawing.Point(0, 0);
            this.guna2PictureBoxLiveCamera.Margin = new System.Windows.Forms.Padding(0);
            this.guna2PictureBoxLiveCamera.Name = "guna2PictureBoxLiveCamera";
            this.guna2PictureBoxLiveCamera.Size = new System.Drawing.Size(500, 403);
            this.guna2PictureBoxLiveCamera.TabIndex = 1;
            this.guna2PictureBoxLiveCamera.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 403);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1000, 217);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // ФИО
            // 
            this.ФИО.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ФИО.DataPropertyName = "ФИО";
            this.ФИО.HeaderText = "ФИО";
            this.ФИО.Name = "ФИО";
            // 
            // Статус
            // 
            this.Статус.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Статус.DataPropertyName = "Статус";
            this.Статус.HeaderText = "Статус";
            this.Статус.Name = "Статус";
            // 
            // Дата
            // 
            this.Дата.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Дата.DataPropertyName = "Дата";
            this.Дата.HeaderText = "Дата";
            this.Дата.Name = "Дата";
            // 
            // Время
            // 
            this.Время.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Время.DataPropertyName = "Время";
            this.Время.HeaderText = "Время";
            this.Время.Name = "Время";
            // 
            // EnableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(193)))), ((int)(((byte)(131)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EnableControl";
            this.Size = new System.Drawing.Size(1000, 620);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBoxLiveCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Guna.UI2.WinForms.Guna2Panel guna2PanelLastCapturedFace;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel guna2PanelSelectedFace;
        private Guna.UI2.WinForms.Guna2Button guna2BtnStopRecognition;
        private Guna.UI2.WinForms.Guna2Button guna2BtnStartRecognition;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBoxLiveCamera;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBox2;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ФИО;
        private System.Windows.Forms.DataGridViewTextBoxColumn Статус;
        private System.Windows.Forms.DataGridViewTextBoxColumn Дата;
        private System.Windows.Forms.DataGridViewTextBoxColumn Время;
    }
}
