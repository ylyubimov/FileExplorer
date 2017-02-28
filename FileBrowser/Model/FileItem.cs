using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using FileBrowser.View;

namespace FileBrowser.Model {
    /// <summary>
    /// Class for work with file, specified by its path and fileSystemModel
    /// </summary>
    public class FileItem : INotifyPropertyChanged {
        public FileItem(FileSystemModel model, string path)
        {
            _model = model;
            _path = path;
            _canExecute = true;
        }

        /// <summary>
        /// Specifies the name and icon of the file be the path
        /// </summary>
        public string Path
        {
			get {
                return _path;
            }
			set {
				_path = value;
				if( _path.Length > 0 ) {
					Name = _path.Split( new[] {System.IO.Path.DirectorySeparatorChar},
                        StringSplitOptions.RemoveEmptyEntries ).Last();

					FileAttributes attr = File.GetAttributes( _path );
					Icon = ToImageSource( ( attr & FileAttributes.Directory ) == FileAttributes.Directory
									? IconReader.GetFolderIcon( IconReader.IconSize.Large, IconReader.FolderType.Closed )
									: IconReader.GetFileIcon( _path, IconReader.IconSize.Large, false ) );
				} else {
					Name = _path;
					Icon = null;
				}
			}
		}

        /// <summary>
        /// Name of the file
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Icon of the file
        /// </summary>
        public ImageSource Icon
        {
            get;
            private set;
        }

        /// <summary>
        /// Applying click command to the file
        /// </summary>
        public ICommand ClickCommand
        {
            get {
                return _clickCommand ?? ( _clickCommand = new CommandHandler( SetCurrentFileAction, _canExecute ) );
            }
        }

        /// <summary>
        /// Set the file as the current file in file system
        /// </summary>
        public void SetCurrentFileAction()
        {
            _model.SetCurrentFile( _path );
        }

        /// <summary>
        /// Convert icon to the working format
        /// </summary>
        public static ImageSource ToImageSource(System.Drawing.Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon( icon.Handle, Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions() );

            return imageSource;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private ICommand _clickCommand;
        // File system for file
        private readonly FileSystemModel _model;
        // Flag to check can the file be executed or not
        private readonly bool _canExecute;
        // Path to the file
        private string _path;
    }

    /// <summary>
    /// Class checking and executing commands
    /// </summary>
    public class CommandHandler : ICommand {
		public CommandHandler( Action action, bool canExecute )
        {
			_action = action;
			_canExecute = canExecute;
		}

		public bool CanExecute( object parameter )
        {
			return _canExecute;
		}

		public void Execute( object parameter )
        {
			_action();
		}

        public event EventHandler CanExecuteChanged;
        private readonly Action _action;
        private readonly bool _canExecute;
    }
}