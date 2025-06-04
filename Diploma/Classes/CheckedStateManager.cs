#region using
using System.Linq;
using System.Windows.Forms;
#endregion

namespace Diploma.Helpers
{
    public class CheckedStateManager
    {
        #region Поля
        private readonly CheckedListBox _checkedList;
        #endregion

        #region Конструктор
        public CheckedStateManager(CheckedListBox checkedList)
        {
            _checkedList = checkedList;
        }
        #endregion

        #region Методы проверки режима
        public bool IsGroupMode() => _checkedList.GetItemChecked(0);
        public bool IsCourseMode() => _checkedList.GetItemChecked(1);
        public bool IsSpecialityMode() => _checkedList.GetItemChecked(2);
        public bool IsStatusMode() => _checkedList.GetItemChecked(3);
        public bool IsCuratorMode() => _checkedList.GetItemChecked(4);
        #endregion

        #region Метод переключения одного чекбокса
        public void EnforceSingleCheck(ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < _checkedList.Items.Count; i++)
                {
                    if (i != e.Index)
                        _checkedList.SetItemChecked(i, false);
                }
            }
        }
        #endregion
    }
}
