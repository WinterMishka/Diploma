using Diploma.EducationAccessSystemDataSetTableAdapters;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Diploma.Classes;

namespace Diploma
{
    public partial class ReportControl : UserControl
    {
        #region Конструктор
        public ReportControl()
        {
            InitializeComponent();
            LoadGroups();

            this.Load += (s, e) =>
            {
                if (FindForm() is FaceControl face)
                    UiSettingsManager.ApplyTo(face);
            };
        }
        #endregion

        #region Методы
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (!TryGetInterval(out DateTime from, out DateTime to))
                return;

            object value = listBox1.SelectedValue;
            int groupId = value is int i ? i :
                          value is long l ? (int)l :
                          value is DBNull || value == null ? -1 :
                          int.TryParse(value.ToString(), out int parsed) ? parsed : -1;
            int? paramGroup = groupId == -1 ? (int?)null : groupId;

            string dsName;
            DataTable tblCopy;
            string groupName = listBox1.Text;
            string curator = (listBox1.SelectedItem as DataRowView)
                               .Row.Table.Columns.Contains("Куратор")
                               ? (listBox1.SelectedItem as DataRowView)["Куратор"].ToString()
                               : string.Empty;
            string period = $"{from:dd.MM.yyyy} – {to:dd.MM.yyyy}";

            if (paramGroup == null)
            {
                var taAll = new usp_ОтсутствияЗаПериодTableAdapter();
                var allTbl = new EducationAccessSystemDataSet.usp_ОтсутствияЗаПериодDataTable();
                allTbl.Clear();
                taAll.Fill(allTbl, from, to);

                dsName = "AllAbsences";
                tblCopy = allTbl.Copy();
            }
            else
            {
                var taGroup = new usp_Отсутствия_ГруппыTableAdapter();
                var groupTbl = new EducationAccessSystemDataSet.usp_Отсутствия_ГруппыDataTable();
                groupTbl.Clear();
                taGroup.Fill(groupTbl, paramGroup, from, to);

                dsName = "GroupAbsences";
                tblCopy = groupTbl.Copy();
            }

            dataGridView1.DataSource = tblCopy;
            DataGridViewUI.BeautifyGrid(dataGridView1);

            using (var f = new Diploma.Forms.ReportForm(
                               tblCopy, dsName,
                               groupName, curator, period))
            {
                f.ShowDialog(FindForm());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (!TryGetInterval(out DateTime from, out DateTime to))
                return;

            object value = listBox1.SelectedValue;
            int groupId = value is int i ? i :
                          value is long l ? (int)l :
                          value is DBNull || value == null ? -1 :
                          int.TryParse(value.ToString(), out int parsed) ? parsed : -1;
            int? paramGroup = groupId == -1 ? (int?)null : groupId;

            DataTable tblCopy;
            if (paramGroup == null)
            {
                var taAll = new usp_ОтсутствияЗаПериодTableAdapter();
                var allTbl = new EducationAccessSystemDataSet.usp_ОтсутствияЗаПериодDataTable();
                allTbl.Clear();
                taAll.Fill(allTbl, from, to);
                tblCopy = allTbl.Copy();
            }
            else
            {
                var taGroup = new usp_Отсутствия_ГруппыTableAdapter();
                var groupTbl = new EducationAccessSystemDataSet.usp_Отсутствия_ГруппыDataTable();
                groupTbl.Clear();
                taGroup.Fill(groupTbl, paramGroup, from, to);
                tblCopy = groupTbl.Copy();
            }

            dataGridView1.DataSource = tblCopy;
            DataGridViewUI.BeautifyGrid(dataGridView1);
        }

        private void LoadGroups()
        {
            vw_ГруппыПолныеTableAdapter.Fill(educationAccessSystemDataSet.vw_ГруппыПолные);
            var tbl = educationAccessSystemDataSet.vw_ГруппыПолные;
            var row = tbl.Newvw_ГруппыПолныеRow();
            row.id_группы = -1;
            row.Название_полное = "Все группы";
            tbl.Rows.InsertAt(row, 0);
            listBox1.DataSource = tbl;
            listBox1.DisplayMember = "Название_полное";
            listBox1.ValueMember = "id_группы";
            listBox1.SelectedIndex = 0;
        }

        private bool TryGetInterval(out DateTime from, out DateTime to)
        {
            from = guna2DateTimePicker1.Value.Date;
            to = guna2DateTimePicker2.Value.Date;
            if (from > to)
            {
                MessageBox.Show("Начальная дата должна быть раньше конечной.",
                                "Неверный интервал",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        #endregion
    }
}
