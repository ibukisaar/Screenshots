using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Wpf截图工具 {
	public class AcceptSelectEventArgs : RoutedEventArgs {
		public BitmapSource SelectBitmap { get; }

		public AcceptSelectEventArgs(BitmapSource bitmap, RoutedEvent routedEvent, object source) : base(routedEvent, source) {
			SelectBitmap = bitmap;
		}
	}
}
