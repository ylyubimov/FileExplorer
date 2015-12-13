using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileBrowser.Model {
	public class FileSystemModel {
		private readonly int _currentFileRow;
		private readonly int _currentFileColumn;
		private readonly List<FileItem>[] _field;

		public List<FileItem>[] Field {
			get { return _field; }
		}

		private void EraseField() {
			for (int i = 0; i < _field.Length; ++i) {
				_field[i].Clear();
			}
		}

		private void UpdateBottomLevel(string parentPath) {
			if (!Directory.Exists(parentPath)) {
				return;
			}

			foreach (var file in Directory.GetDirectories(parentPath).Concat(Directory.GetFiles(parentPath))) {
				_field[_currentFileRow + 1].Add(new FileItem(this));
				_field[_currentFileRow + 1].Last().Path = file;
			}
		}

		// закидывает в строчку row файлы из директории newPath. используется для того, чтобы обновлять все линии, кроме нижней
		private void UpdateLevel(int row, string newPath) {
			if (Directory.GetParent(newPath) == null) {
				_field[row].Add(new FileItem(this));
				_field[row].Last().Path = newPath;
				return;
			}

			var parentPath = Directory.GetParent(newPath).FullName;
			var directories = Directory.GetDirectories(parentPath);
			var files = Directory.GetFiles(parentPath);

			foreach (var directory in directories) {
				_field[row].Add(new FileItem(this));
				_field[row].Last().Path = directory;
			}
			foreach (var file in files) {
				_field[row].Add(new FileItem(this));
				_field[row].Last().Path = file;
			}

//			int newPathIndex = 0;
//			while (newPathIndex < directories.Length && directories[newPathIndex] != newPath) {
//				++newPathIndex;
//			}
//
//			if (newPathIndex == directories.Length) {
//				// ERROR
//				return;
//			}
//
//
//			// Обновляем состояния слева от центра
//			int fieldIndex = _currentFileColumn;
//			int currentDirIndex = newPathIndex;
//
//			while (fieldIndex >= 0 && currentDirIndex >= 0) {
//				_field[row][fieldIndex].Path = directories[currentDirIndex];
//
//				--fieldIndex;
//				--currentDirIndex;
//			}
//
//			// Обновляем состоянония справа от центра
//			fieldIndex = _columnCount / 2 + 1;
//			currentDirIndex = newPathIndex + 1;
//
//			while (fieldIndex < _columnCount && currentDirIndex < directories.Length) {
//				_field[row][fieldIndex].Path = directories[currentDirIndex];
//
//				++fieldIndex;
//				++currentDirIndex;
//			}
//
//			// Пытаемся дополнить строку файлами
//			if (fieldIndex >= _columnCount) {
//				return;
//			}
//			foreach (var currentFile in files) {
//				if (fieldIndex < _columnCount) {
//					_field[row][fieldIndex].Path = currentFile;
//					++fieldIndex;
//				} else {
//					return;
//				}
//			}
		}

		public FileSystemModel(int rowCount, string currentPath) {
			// TODO: check argumnts
			// rowCount > 1

			_currentFileRow = rowCount - 2;
			_currentFileColumn = 0;

			_field = new List<FileItem>[rowCount];
			for (int i = 0; i < rowCount; i++) {
				_field[i] = new List<FileItem>();
			}

			SetCurrentFile(currentPath);
		}

		public void SetCurrentFile(string path) {
			if (path == "") {
				return;
			}
			EraseField();
			UpdateBottomLevel(path);

			for (int row = _currentFileRow; row >= 0; --row) {
				UpdateLevel(row, path);
				if (Directory.GetParent(path) == null) {
					return;
				}
				path = Directory.GetParent(path).FullName;
			}
		}

		public void Print() {
			for (int i = 0; i < _field.Length; ++i) {
				for (int j = 0; j < _field[i].Count; ++j) {
					if (_field[i][j].Path.Length > 0) {
						System.Console.Write(_field[i][j].Name);
					}
					System.Console.Write("\t\t");
				}

				System.Console.WriteLine();
			}
		}
	}
}