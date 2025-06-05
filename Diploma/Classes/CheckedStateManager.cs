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
            _checkBoxes = checkBoxes.ToList();
        }
        #endregion

        #region Методы проверки режима
        public bool IsGroupMode() => _checkBoxes[0].Checked;
        public bool IsCourseMode() => _checkBoxes[1].Checked;
        public bool IsSpecialityMode() => _checkBoxes[2].Checked;
        public bool IsStatusMode() => _checkBoxes[3].Checked;
        public bool IsCuratorMode() => _checkBoxes[4].Checked;
        #endregion
        #endregion
    }
}
