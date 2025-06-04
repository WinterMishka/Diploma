#region using
using System.Data;
using System.Windows.Forms;
using Diploma.Data;
#endregion

namespace Diploma.Helpers
{
    public class DbEntryService
    {
        #region Поля
        private readonly IAppDbService _db;
        #endregion

        #region Конструктор
        public DbEntryService(IAppDbService db)
        {
            _db = db;
        }
        #endregion

        #region Метод назначения куратора
        public void AssignCuratorIfConfirmed(DataGridView grid, ComboBox combo)
        {
            if (combo.SelectedValue == null || grid.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите группу и куратора.",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var drv = (DataRowView)grid.CurrentRow.DataBoundItem;
            int groupId = (int)drv["id_группы"];
            int curatorId = (int)combo.SelectedValue;

            var confirm = MessageBox.Show(
                $"Вы собираетесь назначить куратором:\n\n{combo.Text}\n\n" +
                $"группы:\n\n{drv["Название_полное"]}\n\nПродолжить?",
                "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            _db.AssignCurator(groupId, curatorId);
        }
        #endregion
    }
}
