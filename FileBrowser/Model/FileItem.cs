using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using QuickZip.Tools;

namespace FileBrowser.Model {
	public class FileItem : INotifyPropertyChanged {
		private readonly FileSystemModel _model;
		private readonly bool _canExecute;
		private string _path;
		public event PropertyChangedEventHandler PropertyChanged;

		public string Path {
			get { return _path; }
			set {
				_path = value;
				if (_path.Length > 0) {
					Name = _path.Split(new[] {System.IO.Path.DirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries).Last();

					FileToIconConverter iconConverter = new FileToIconConverter();

					Icon = iconConverter.GetImage(_path, 120);

					//FileAttributes attr = File.GetAttributes(_path);
					//if ((attr & FileAttributes.Directory) == FileAttributes.Directory) 
					//{
					//    Icon = ToImageSource(IconReader.GetFolderIcon(IconReader.IconSize.ExtraLarge,
					//        IconReader.FolderType.Closed));
					//}
					//else
					//{
					//    Icon = ToImageSource(IconReader.GetFileIcon(_path, IconReader.IconSize.ExtraLarge, false));
					//}
				} else {
					Name = _path;
					Icon = null;
				}
			}
		}

		public string Name { get; private set; }

		public ImageSource Icon { get; private set; }

		public FileItem(FileSystemModel model, string path) {
			_model = model;
			_path = path;
			_canExecute = true;
		}

		private ICommand _clickCommand;

		public ICommand ClickCommand {
			get { return _clickCommand ?? (_clickCommand = new CommandHandler(MyAction, _canExecute)); }
		}

		public void MyAction() {
			_model.SetCurrentFile(_path);
		}

		public static ImageSource ToImageSource(System.Drawing.Icon icon) {
			ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty,
					System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

			return imageSource;
		}
	}

	public class CommandHandler : ICommand {
		private readonly Action _action;
		private readonly bool _canExecute;
		public event EventHandler CanExecuteChanged;

		public CommandHandler(Action action, bool canExecute) {
			_action = action;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) {
			return _canExecute;
		}

		public void Execute(object parameter) {
			_action();
		}
	}
}