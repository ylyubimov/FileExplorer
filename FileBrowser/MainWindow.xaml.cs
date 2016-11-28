using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FileBrowser.Model;
using System;

namespace FileBrowser {
	public partial class MainWindow {
		public FileSystemModel Model { get; private set; }
		private double CurrentXPosition, CurrentYPosition;

		public MainWindow() {
			InitializeComponent();
			ListBox[] listBoxes = {PreviousLevel, CurrentLevel, NextLevel};
			ScrollViewer[] scrollViewers = {PreviousLevelScrollViewer, CurrentLevelScrollViewer, NextLevelScrollViewer};
			Model = new FileSystemModel(3, "C:\\", listBoxes, scrollViewers);
			CurrentXPosition = 0;
			CurrentYPosition = 0;
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

		private void SelectUpperFile()
		{
			int randomFileNumber = (Model.Field[0].Count + 1) / 2 - 1;
			FileItem randomFile = Model.Field[0][randomFileNumber];
			Model.SetCurrentFile(randomFile.Path);
		}

		private void SelectBottomFile()
		{
			int randomFileNumber = (Model.Field[2].Count + 1) / 2 - 1;
			FileItem randomFile = Model.Field[2][randomFileNumber];
			Model.SetCurrentFile(randomFile.Path);
		}

		private void KeyEvents(object sender, KeyEventArgs e)
		{
			if( e.Key == Key.W )
			{
				SelectUpperFile();
			}
			else if( e.Key == Key.S )
			{
				SelectBottomFile();
			}
			else if( e.Key == Key.A )
			{
				Model.SelectRightFile();

			}
			else if( e.Key == Key.D )
			{
				Model.SelectRightFile();
			}
		}

		private void UntouchHandler(object sender, TouchEventArgs e)
		{
			CurrentXPosition = 0;
			CurrentYPosition = 0;
		}

		private void TouchHandler(object sender, TouchEventArgs e)
		{
			TouchPoint tp = e.GetTouchPoint(cnv);
			// Не рассматриваем небольшие отклонения. Число подобрано из эмпирических соображений
			if (Math.Abs(CurrentXPosition - tp.Position.X) >= 50)
			{
				// Делаем логику только если мы в процессе перемещения пальца и у-позиция уже не 0.
				if (CurrentXPosition != 0)
				{
					// Смотрим, в какую сторону сдвинулся палец
					if (CurrentXPosition - tp.Position.X > 0)
					{
						Model.SelectRightFile();
					}
					else
					{
						Model.SelectLeftFile();
					}

				}
				// В любом случае обновляем координату
				CurrentXPosition = tp.Position.X;
			}

			if (Math.Abs(CurrentYPosition - tp.Position.Y) >= 50)
			{
				// Делаем логику только если мы в процессе перемещения пальца и у-позиция уже не 0.
				if (CurrentYPosition != 0)
				{
					if (CurrentYPosition - tp.Position.Y > 0)
					{
						SelectBottomFile();
					}
					else
					{
						SelectUpperFile();
					}
				}
				// В любом случае обновляем координату
				CurrentYPosition = tp.Position.Y;
			}
		}
	}
}