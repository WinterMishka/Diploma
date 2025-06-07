#region using
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace Diploma.Helpers
{
    public class SearchFilterManager
    {
        #region Поля
        private readonly ComboBox[] _all;
        #endregion

        #region Конструктор
        public SearchFilterManager(ComboBox[] allComboBoxes)
        {
            _all = allComboBoxes;
        }
        #endregion

        #region Методы
        public void ApplyDropDownStyles(params ComboBox[] fkComboBoxes)
        {
            var fk = new HashSet<ComboBox>(fkComboBoxes);

            foreach (var cb in _all)
                cb.DropDownStyle = fk.Contains(cb)
                    ? ComboBoxStyle.DropDownList
                    : ComboBoxStyle.Simple;
        }
        #endregion
    }
}
