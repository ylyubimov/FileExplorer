using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Xml;
using FileBrowser.Model;

namespace FileBrowser {
	public partial class MainWindow : Window {
		private FileSystemModel _model;

		public MainWindow() {
			InitializeComponent();
			ListBox[] listBoxes = {PreviousLevel, CurrentLevel, NextLevel};
			ScrollViewer[] scrollViewers = {PreviousLevelScrollViewer, CurrentLevelScrollViewer, NextLevelScrollViewer};
			_model = new FileSystemModel(3, "C:\\", listBoxes, scrollViewers);
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

		private void ScrollViewerOnTouchDown(object sender, TouchEventArgs e) {
			throw new NotImplementedException();
		}

		private void ScrollViewerOnMouseWheel(object sender, MouseWheelEventArgs e) {
			throw new NotImplementedException();
		}
	}
}