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
        #region Методы
        public static void BeautifyGrid(DataGridView gv)
        {
            gv.BorderStyle = BorderStyle.None;
            gv.EnableHeadersVisualStyles = false;
            gv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0x2F, 0x8F, 0x8F);

            var fore = Color.White;
            if (!string.IsNullOrEmpty(UiSettingsManager.Current.FontColor))
                fore = ColorTranslator.FromHtml(UiSettingsManager.Current.FontColor);

            gv.ColumnHeadersDefaultCellStyle.ForeColor = fore;
            gv.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 11, FontStyle.Bold);
            gv.DefaultCellStyle.Font = new Font("Verdana", 10, FontStyle.Regular);
            gv.DefaultCellStyle.ForeColor = fore;

            if (gv.Columns.Contains("id_учащегося"))
                gv.Columns["id_учащегося"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            if (gv.Columns.Contains("ФИО"))
                gv.Columns["ФИО"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (gv.Columns.Contains("Даты_отсутствия"))
            {
                gv.Columns["Даты_отсутствия"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gv.Columns["Даты_отсутствия"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            if (gv.Columns.Contains("Количество_пропусков"))
                gv.Columns["Количество_пропусков"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gv.RowHeadersVisible = true;
            gv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gv.MultiSelect = false;
            gv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(0xE8, 0xF5, 0xE9);
        }
        #endregion
    }
}
