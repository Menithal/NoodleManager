using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoodleManager
{
    public class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public partial class MainWindow : Window
    {
        public const int WM_NCHITTEST = 0x84;
        public const int HTCAPTION = 2;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;

        const int cGrip = 5;
        const int cCaption = 35;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            HwndSource windowSource = HwndSource.FromHwnd(windowHandle);
            windowSource.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCHITTEST)
            {
                int x = lParam.ToInt32() << 16 >> 16, y = lParam.ToInt32() >> 16;
                Point pos = PointFromScreen(new Point(x, y));

                if (VisualTreeHelper.HitTest(TopGrid, pos) == null)
                {
                    bool right = false;
                    bool left = false;
                    bool top = false;
                    bool bottom = false;

                    if (pos.X >= ActualWidth - cGrip)
                    {
                        right = true;
                    }
                    else if (pos.X < cGrip)
                    {
                        left = true;
                    }

                    if (pos.Y >= this.ActualHeight - cGrip)
                    {
                        bottom = true;
                    }
                    else if (pos.Y < cGrip)
                    {
                        top = true;
                    }

                    if (right)
                    {
                        if (bottom)
                        {
                            handled = true;
                            return (IntPtr)HTBOTTOMRIGHT;
                        }
                        else if (top)
                        {
                            handled = true;
                            return (IntPtr)HTTOPRIGHT;
                        }
                        else
                        {
                            handled = true;
                            return (IntPtr)HTRIGHT;
                        }
                    }
                    else if (left)
                    {
                        if (bottom)
                        {
                            handled = true;
                            return (IntPtr)HTBOTTOMLEFT;
                        }
                        else if (top)
                        {
                            handled = true;
                            return (IntPtr)HTTOPLEFT;
                        }
                        else
                        {
                            handled = true;
                            return (IntPtr)HTLEFT;
                        }
                    }
                    else if (bottom)
                    {
                        handled = true;
                        return (IntPtr)HTBOTTOM;
                    }
                    else if (top)
                    {
                        handled = true;
                        return (IntPtr)HTTOP;
                    }
                    else if (pos.Y < cCaption)
                    {
                        handled = true;
                        return (IntPtr)HTCAPTION;
                    }
                }
            }
            return IntPtr.Zero;
        }
    }
}
