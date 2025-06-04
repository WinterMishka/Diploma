using System.Windows.Forms;

namespace Diploma
{
    public class ContentLoader
    {
        private Panel _targetPanel;
        public UserControl CurrentControl { get; private set; }


        public ContentLoader(Panel targetPanel)
        {
            _targetPanel = targetPanel;
        }

        public void Load(UserControl control)
        {
            _targetPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            _targetPanel.Controls.Add(control);
            CurrentControl = control;
        }
    }
}
