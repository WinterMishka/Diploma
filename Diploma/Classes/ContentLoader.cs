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
            if (control == null)
                return;

            if (CurrentControl != null)
                CurrentControl.Visible = false;

            if (!_targetPanel.Controls.Contains(control))
            {
                control.Dock = DockStyle.Fill;
                _targetPanel.Controls.Add(control);
            }

            control.Visible = true;
            control.BringToFront();
            CurrentControl = control;
        }
        #endregion
    }
}
