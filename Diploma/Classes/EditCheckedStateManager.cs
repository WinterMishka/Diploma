#region using
using System.Collections.Generic;
using Guna.UI2.WinForms;
#endregion

namespace Diploma.Helpers
{
    public class EditCheckedStateManager
    {
        #region Поля
        private readonly Dictionary<int, Guna2CheckBox> _checkBoxes;
        #endregion

        #region Конструктор
        public EditCheckedStateManager(Dictionary<int, Guna2CheckBox> checkBoxes)
        {
            _checkBoxes = checkBoxes;
        }
        #endregion

        #region Методы
        public int GetCheckedIndex()
        {
            if (_checkBoxes[1].Checked) return 0;
            if (_checkBoxes[2].Checked) return 1;
            if (_checkBoxes[3].Checked) return 2;

            if (_checkBoxes[4].Checked) return 3;

            if (_checkBoxes[5].Checked) return 4;

            if (_checkBoxes[6].Checked) return 5;

            if (_checkBoxes[7].Checked) return 6;

            if (_checkBoxes[8].Checked) return 7;

            return -1;
        }


        public bool IsChecked(int index) =>
            _checkBoxes.TryGetValue(index, out var cb) && cb.Checked;
        #endregion
    }
}
