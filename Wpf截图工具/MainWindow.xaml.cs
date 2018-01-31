using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Interop;

namespace Wpf截图工具 {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public ICommand HideCommand { get; } = new Command<Window>(win => win.Visibility = Visibility.Hidden);
		
		private IReadOnlyList<Rect> windowRects;

		public MainWindow() {
			InitializeComponent();

			pixelObserver.DataContext = editor;
			tipSize.DataContext = editor;
			tipRGB.DataContext = pixelObserver;
		}

		private void Grid_PreviewMouseMove(object sender, MouseEventArgs e) {
			var point = e.GetPosition(editor);
			pixelObserver.X = point.X;
			pixelObserver.Y = point.Y;
		}

		private void FocusWindow() {
			var curr = Mouse.GetPosition(this);
			foreach (var r in windowRects) {
				if (r.Contains(curr)) {
					editor.SelectX = r.X;
					editor.SelectY = r.Y;
					editor.SelectWidth = r.Width;
					editor.SelectHeight = r.Height;
					break;
				}
			}
		}

		private void editor_MouseMove(object sender, MouseEventArgs e) {
			if (editor.SelectState == SelectState.Fixed) {
				FocusWindow();
			}
		}

		private void editor_Click(object sender, RoutedEventArgs e) {
			Debug.Print("click");
			if (editor.SelectState == SelectState.Fixed) editor.Select();
		}

		private void editor_AcceptSelect(object sender, AcceptSelectEventArgs e) {
			Clipboard.SetImage(e.SelectBitmap);
			Hide();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			const int WS_EX_TOOLWINDOW = 0x00000080;
			var helper = new WindowInteropHelper(this);
			var exStyle = Tool.GetWindowExStyle(helper.Handle);
			Tool.SetWindowExStyle(helper.Handle, exStyle | WS_EX_TOOLWINDOW);

			HotKey.Register(this, ModifierKeys.Alt, Keys.A, delegate {
				if (Visibility != Visibility.Visible) {
					editor.BackgroundBitmap = Tool.ScreenSnapshot;
					editor.Reset();
					windowRects = Tool.GetWindowRects();
					Show();
				}
			});
			Hide();
		}

		private void editor_Close(object sender, RoutedEventArgs e) {
			Hide();
		}

		private void editor_CancelSelect(object sender, RoutedEventArgs e) {
			editor.Reset();
			FocusWindow();
		}
	}
}
