using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Diploma
{
    public class SidebarHighlighter
    {
        #region Поля
        private readonly List<Guna2Button> _buttons;
        private Color _activeColor = Color.ForestGreen;
        private Color _inactiveColor = Color.White;
        private Guna2Button _activeButton;
        #endregion

        #region Конструктор
        public SidebarHighlighter(List<Guna2Button> buttons)
        {
            _buttons = buttons;
        }
        #endregion

        #region Свойства
        public Color ActiveColor
        {
            get => _activeColor;
            set
            {
                _activeColor = value;
                if (_activeButton != null)
                    SetActive(_activeButton);
            }
        }

        public Color InactiveColor
        {
            get => _inactiveColor;
            set
            {
                _inactiveColor = value;
                if (_activeButton != null)
                    SetActive(_activeButton);
            }
        }

        public Guna2Button ActiveButton => _activeButton;
        #endregion

        #region Методы
        public void SetActive(Guna2Button activeButton)
        {
            foreach (var btn in _buttons)
                btn.CustomBorderColor = _inactiveColor;

            activeButton.CustomBorderColor = _activeColor;
            _activeButton = activeButton;
        }
        #endregion
    }

    public class NavButtonState
    {
        #region Свойства
        public string Text { get; set; }
        public Padding Padding { get; set; }
        public ContentAlignment TextAlign { get; set; }
        public Font Font { get; set; }
        public HorizontalAlignment ImageAlign { get; set; }
        #endregion
    }

    public class UserInterfaceManager
    {
        #region Поля
        private readonly Form _form;
        private readonly Panel _panelNavButtons;
        private readonly SidebarHighlighter _sidebar;
        private readonly Dictionary<Guna2Button, NavButtonState> _buttonStates;
        private readonly Dictionary<Guna2Button, string> _buttonTexts;
        private readonly Guna2Button _btnSidebarToggle;
        private bool _isCollapsed;

        private const int CollapsedWidthNormal = 80;
        private const int CollapsedWidthMax = 120;
        private const int ExpandedWidthNormal = 200;
        private const int ExpandedWidthMax = 300;
        #endregion

        #region Конструктор
        public UserInterfaceManager(Form form, Panel panelNavButtons, Guna2Button btnSidebarToggle, List<Guna2Button> navButtons)
        {
            _form = form;
            _panelNavButtons = panelNavButtons;
            _btnSidebarToggle = btnSidebarToggle;
            _sidebar = new SidebarHighlighter(navButtons);

            _buttonTexts = new Dictionary<Guna2Button, string>();
            _buttonStates = new Dictionary<Guna2Button, NavButtonState>();

            foreach (var btn in navButtons)
            {
                if (btn == btnSidebarToggle) continue;

                _buttonTexts[btn] = btn.Text;
                _buttonStates[btn] = new NavButtonState
                {
                    Text = btn.Text,
                    Padding = new Padding(50, 0, 0, 0),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ImageAlign = HorizontalAlignment.Left,
                    Font = btn.Font
                };
            }
        }
        #endregion

        #region Методы
        public void ToggleSidebar()
        {
            _isCollapsed = !_isCollapsed;
            ApplyLayout();
            _btnSidebarToggle.Image = _isCollapsed
                ? Properties.Resources.ImageMenu
                : Properties.Resources.ImageArrowLeft;
            _sidebar.SetActive(_btnSidebarToggle);
        }

        public void ToggleWindowSize()
        {
            _form.WindowState = _form.WindowState == FormWindowState.Normal
                ? FormWindowState.Maximized
                : FormWindowState.Normal;
            ApplyLayout();
        }

        public void ApplyLayout()
        {
            bool isMax = _form.WindowState == FormWindowState.Maximized;
            _panelNavButtons.Width = _isCollapsed
                ? (isMax ? CollapsedWidthMax : CollapsedWidthNormal)
                : (isMax ? ExpandedWidthMax : ExpandedWidthNormal);

            foreach (Control ctrl in _panelNavButtons.Controls)
            {
                if (ctrl is Guna2Button btn)
                {
                    if (btn == _btnSidebarToggle)
                    {
                        btn.Text = _isCollapsed ? "" : " ";
                        btn.ImageSize = _isCollapsed
                            ? new Size(40, 40)
                            : (isMax ? new Size(280, 150) : new Size(140, 100));
                        btn.Padding = Padding.Empty;
                        btn.ImageAlign = HorizontalAlignment.Center;
                    }
                    else if (_isCollapsed)
                    {
                        btn.Text = "";
                        btn.Padding = Padding.Empty;
                        btn.ImageAlign = HorizontalAlignment.Center;
                        btn.ImageSize = isMax ? new Size(70, 70) : new Size(50, 50);
                    }
                    else if (_buttonStates.TryGetValue(btn, out var st))
                    {
                        btn.Text = st.Text;
                        btn.Padding = isMax
                            ? new Padding(80, st.Padding.Top, st.Padding.Right, st.Padding.Bottom)
                            : st.Padding;
                        btn.TextAlign = (HorizontalAlignment)st.TextAlign;
                        btn.ImageAlign = st.ImageAlign;
                        btn.Font = st.Font;
                        btn.ImageSize = isMax ? new Size(80, 80) : new Size(50, 50);
                    }
                }
            }
        }

        public void HighlightButton(Guna2Button button) => _sidebar.SetActive(button);

        public Color HighlightColor
        {
            get => _sidebar.ActiveColor;
            set => _sidebar.ActiveColor = value;
        }

        public Guna2Button ActiveButton => _sidebar.ActiveButton;
        #endregion
    }
}
