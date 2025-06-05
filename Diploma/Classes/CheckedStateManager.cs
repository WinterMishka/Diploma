#region using
using System.Collections.Generic;
using System.Linq;
using Guna.UI2.WinForms;
#endregion

namespace Diploma.Helpers
{
    public class CheckedStateManager
    {
        #region Поля
        private readonly IList<Guna2CheckBox> _checkBoxes;
        #endregion

        #region Конструктор
        public CheckedStateManager(IEnumerable<Guna2CheckBox> checkBoxes)
        {
            _checkBoxes = checkBoxes?.ToList() ?? new List<Guna2CheckBox>();
        }
        #endregion

        #region Методы проверки режима
        private bool GetChecked(int index) =>
            _checkBoxes.Count > index && _checkBoxes[index].Checked;
        public bool IsGroupMode() => GetChecked(0);
        public bool IsCourseMode() => GetChecked(1);
        public bool IsSpecialityMode() => GetChecked(2);
        public bool IsStatusMode() => GetChecked(3);
        public bool IsCuratorMode() => GetChecked(4);
        #endregion
        #endregion
    }
}
