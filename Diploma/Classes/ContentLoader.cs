using System.Windows.Forms;

namespace Diploma
{
    public class ContentLoader
    {
        #region Поля
        private Panel _targetPanel;
        public UserControl CurrentControl { get; private set; }
        #endregion

        #region Конструктор
        public ContentLoader(Panel targetPanel)
        {
            _targetPanel = targetPanel;
        }
        #endregion

        #region Методы
        public void Load(UserControl control)
        {
            _targetPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            _targetPanel.Controls.Add(control);
            CurrentControl = control;
        }
        #endregion
    }
}
