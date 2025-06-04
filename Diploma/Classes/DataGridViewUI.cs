using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diploma.Classes
{
    public class DataGridViewUI
    {
        public static void BeautifyGrid(DataGridView _dataGridView)
        {
            var gv = _dataGridView;
            gv.BorderStyle = BorderStyle.None;
            gv.EnableHeadersVisualStyles = false;
            gv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0x2F, 0x8F, 0x8F);
            gv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gv.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 11, FontStyle.Bold);
            gv.DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Regular);
            gv.Columns["id_учащегося"].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            gv.Columns["ФИО"].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;
            gv.Columns["Даты_отсутствия"].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;
            gv.Columns["Даты_отсутствия"].DefaultCellStyle.WrapMode =
                DataGridViewTriState.True;
            gv.Columns["Сумма_пропусков"].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gv.RowHeadersVisible = false;
            gv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(0xE8, 0xF5, 0xE9);
        }
    }
}
