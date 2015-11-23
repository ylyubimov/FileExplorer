using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using QuickZip.Tools;

namespace FileBrowser.Model
{
    public class FileItem : INotifyPropertyChanged
    {
        private readonly FileSystemModel _model;

        private string _path;

        private string _name;
        private ImageSource _icon;

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                if (_path.Length > 0)
                {
                    Name = _path.Split(new char[] { System.IO.Path.DirectorySeparatorChar },
                        StringSplitOptions.RemoveEmptyEntries).Last();

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
                }
                else
                {
                    Name = _path;
                    Icon = null;
                }
            }
        }

        public string Name 
        { 
            get 
            { 
                return _name; 
            }
            private set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public ImageSource Icon
        {
            get
            {
                return _icon;
            }
            private set
            {
                _icon = value;
                OnPropertyChanged("Icon");
            }
        }

        public FileItem(FileSystemModel model)
        {
            _model = model;
            _canExecute = true;
        }

        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(MyAction, _canExecute));
            }
        }
        private readonly bool _canExecute;

        public FileItem() {
            _model = null;
            _canExecute = false;
        }

        public void MyAction()
        {
            _model.SetCurrentFile(_path);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public static ImageSource ToImageSource(System.Drawing.Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
    
    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
