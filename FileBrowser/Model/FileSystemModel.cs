using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileBrowser.Model {
	public class FileSystemModel {
		private readonly int _currentFileRow;
		private int _currentFileColumn;

		// Файлы в каждой строчке (в каждой параллели)
		private readonly List<FileItem>[] _field;
		private readonly List<ListBox> _listBoxes;
		private readonly List<ScrollViewer> _scrollViewers;

		public List<FileItem>[] Field {
			get { return _field; }
		}

		public List<ScrollViewer> ScrollViewer {
			get { return _scrollViewers; }
		}

		public FileSystemModel(int rowCount, string currentPath, IEnumerable<ListBox> listBoxes, IEnumerable<ScrollViewer> scrollViewers) {
			if (rowCount <= 1) {
				throw new ArgumentException("row count must be 2 or more");
			}

			_currentFileRow = rowCount - 2; // 1, то есть средняя строчка в случае, когда их всего 3
			_currentFileColumn = 0;

			_field = new List<FileItem>[rowCount];
			for (int i = 0; i < rowCount; i++) {
				_field[i] = new List<FileItem>();
			}

			_listBoxes = listBoxes.ToList();
			_scrollViewers = scrollViewers.ToList();

			SetCurrentFile(currentPath);
		}

		private void EraseField() {
			foreach (var row in _field) {
				row.Clear();
			}
		}

		public void SelectRightFile()
		{
			// Если можно сдвинуть вправо
			if (_currentFileColumn + 1 < _field[1].Count) {
				SetCurrentFile(_field[1][_currentFileColumn + 1].Path);
			}
		}

		public void SelectLeftFile()
		{
			// Если можно сдвинуть влево
			if (_currentFileColumn > 0)
			{
				SetCurrentFile(_field[1][_currentFileColumn - 1].Path);
			}
		}

		private void FillBottomLevel(string path) {
			foreach (var file in Directory.GetDirectories(path).Concat(Directory.GetFiles(path))) {
				AddNewItemInField(_currentFileRow + 1, file);
			}
		}

		// Обновляет нижние две строчки
		private void UpdateBottomLevel(string parentPath) {
			// Очищаем все строчки
			EraseField();

			// Заполняем средний уровень 
			if (!Directory.Exists(parentPath)) {
				_field[_currentFileRow].Add(new FileItem(this, parentPath));
				return;
			}

			// Заполняем нижний уровень директориями и файлами из parentPath
			FillBottomLevel(parentPath);
		}

		// закидывает в строчку row файлы из директории newPath. используется для того, чтобы обновлять все линии, кроме нижней
		private void UpdateLevel(int row, string newPath) {
			// Если текущая директория -- корневая
			if (Directory.GetParent(newPath) == null) {
				AddNewItemInField(row, newPath);
				return;
			}

			var parentPath = Directory.GetParent(newPath).FullName;
			var directories = Directory.GetDirectories(parentPath);
			var files = Directory.GetFiles(parentPath);

			// Устанавливаем номер текущего столбца в _currentFileColumn
			_currentFileColumn = FindCurrentRow(row, newPath, parentPath);

			foreach (var directory in directories.Concat(files)) {
				AddNewItemInField(row, directory);
			}
		}

		private void AddNewItemInField(int row, string path) {
			_field[row].Add(new FileItem(this, path));
			_field[row].Last().Path = path;
		}

		// Устанавливаем номер текущего столбца
		private int FindCurrentRow(int row, string newPath, string parentPath) {
			var directories = Directory.GetDirectories(parentPath);
			if (row == _currentFileRow) {
				for (int i = 0; i < directories.Length; i++) {
					if (directories[i] == newPath) {
						return i;
					}
				}
			}
			return _currentFileColumn;
		}

		public void SetCurrentFile(string path) {
			if ((File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory) {
				System.Diagnostics.Process.Start(path);
				return;
			}

			if (path == "") {
				return;
			}

			try {
				Directory.GetDirectories(path);
				Directory.GetFiles(path);
			} catch (UnauthorizedAccessException) {
				return;
			}

			UpdateBottomLevel(path);
			var tmpPath = path;

			for (int row = _currentFileRow; row >= 0; --row) {
				// Обновляем "родительские" строчки - те, которые выше текущей - и текущую зачем-то второй раз
				// В случае, когда строчек всего 3, просто обновляем верхнюю.
				UpdateLevel(row, tmpPath);
				if (Directory.GetParent(tmpPath) == null) {
					break;
				}
				tmpPath = Directory.GetParent(tmpPath).FullName;
			}

			for (int i = 0; i < _field.Length; ++i) {
				_listBoxes[i].Items.Clear();
				foreach (var element in _field[i]) {
					_listBoxes[i].Items.Add(element);
				}
			}

			foreach (var scrollViewer in _scrollViewers) {
				scrollViewer.ScrollToHorizontalOffset(200);
			}
			_scrollViewers[_currentFileRow].ScrollToHorizontalOffset(52 * _currentFileColumn);
		}

	}
}