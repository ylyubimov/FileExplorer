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
using FileBrowser.View.Sphere;

namespace FileBrowser {
    public partial class MainWindow : Window {
        private readonly FileSystemModel _model;
        private List<List<FileItem>> _field;

        public MainWindow() {
            _model = new FileSystemModel( 3, "C:\\" );
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            PreviousLevel.ItemsSource = _model.Field[0];
            CurrentLevel.ItemsSource = _model.Field[1];
            NextLevel.ItemsSource = _model.Field[2];

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e) {
	        _field = _model.Field.ToList();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            PreviousLevel.ItemsSource = _field[0];
            CurrentLevel.ItemsSource = _field[1];
            NextLevel.ItemsSource = _field[2];
        }

        private void ExitButtonOnClick(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}