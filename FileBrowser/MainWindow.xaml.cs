using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileBrowser.Model;

namespace FileBrowser {
	public partial class MainWindow {
		public FileSystemModel Model { get; private set; }

		public MainWindow() {
			InitializeComponent();
			ListBox[] listBoxes = {PreviousLevel, CurrentLevel, NextLevel};
			ScrollViewer[] scrollViewers = {PreviousLevelScrollViewer, CurrentLevelScrollViewer, NextLevelScrollViewer};
			Model = new FileSystemModel(3, "C:\\", listBoxes, scrollViewers);
		}

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
			var listBox = (ListBox) sender;
			listBox.SelectedItem = null;
		}

		private void ExitButtonOnClick(object sender, MouseButtonEventArgs e) {
			Close();
		}

		private void MinimizeButtonOnClick(object sender, MouseButtonEventArgs e) {
			WindowState = WindowState.Minimized;
		}

		private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void KeyEvents(object sender, KeyEventArgs e)
		{
			if( e.Key == Key.W )
			{
				int randomFileNumber = (Model.Field[0].Count + 1) / 2 - 1;
				FileItem randomFile = Model.Field[0][randomFileNumber];
				Model.SetCurrentFile(randomFile.Path);
			}
			else if(e.Key == Key.S)
			{
				int randomFileNumber = (Model.Field[2].Count + 1) / 2 - 1;
				FileItem randomFile = Model.Field[2][randomFileNumber];
				Model.SetCurrentFile(randomFile.Path);
			}
			else if(e.Key == Key.A)
			{
				Model.SelectRightFile();

			}
			else if (e.Key == Key.D)
			{
				Model.SelectRightFile();
			}
		}
	}
}