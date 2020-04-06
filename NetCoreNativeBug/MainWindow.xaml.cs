using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
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

namespace NetCoreNativeBug {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        [SecuritySafeCritical]
        IntPtr CreateWindow() {
            return Native.CreateWindowEx(524416, new IntPtr(SharedWindowClassAtom), string.Empty, -2046820352, 0, 0, 0, 0, new WindowInteropHelper(this).Owner, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public MainWindow() {
            InitializeComponent();
        }


        static ushort sharedWindowClassAtom;
        static Native.WndProc sharedWndProc;

        static ushort SharedWindowClassAtom {
            get {
                GetSharedWindowClassAtom();
                return sharedWindowClassAtom;
            }
        }

        [SecuritySafeCritical]
        static void GetSharedWindowClassAtom() {
            if((int)sharedWindowClassAtom == 0) {
                sharedWndProc = Native.DefWindowProc;
                Native.WNDCLASS newWNDCLASS = new Native.WNDCLASS();
                newWNDCLASS.cbClsExtra = 0;
                newWNDCLASS.cbWndExtra = 0;
                newWNDCLASS.hbrBackground = IntPtr.Zero;
                newWNDCLASS.hCursor = IntPtr.Zero;
                newWNDCLASS.hIcon = IntPtr.Zero;
                newWNDCLASS.lpfnWndProc = sharedWndProc;
                newWNDCLASS.lpszClassName = "WindowName";
                newWNDCLASS.lpszMenuName = (string)null;
                newWNDCLASS.style = 0U;
                sharedWindowClassAtom = Native.RegisterClass(ref newWNDCLASS);
            }
        }

        void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            CreateWindow();
            MessageBox.Show($"class atom {sharedWindowClassAtom}");
        }
    }

    static class Native {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int dwExStyle, IntPtr classAtom, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [SecuritySafeCritical]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [SecuritySafeCritical]
        public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WNDCLASS {
            public uint style;
            public Delegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszMenuName;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpszClassName;
        }
    }
}