using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileBrowser.Model
{
    class FileSystemModel
    {
        private readonly int _rowCount;
        private readonly int _columnCount;
        private readonly int _currentFileRow;
        private readonly int _currentFileColumn;

        private string[][] field;

        private void eraseField()
        {
            for (int i = 0; i < _rowCount; ++i)
            {
                for (int j = 0; j < _columnCount; ++j)
                {
                    field[i][j] = "";
                }
            }
        }

        private void updateBottomLevel(string parentPath)
        {
            if (!Directory.Exists(parentPath))
            {
                return;
            }

            int fieldIndex = 0;
            foreach (string currentFile in Directory.GetDirectories(parentPath).Concat(Directory.GetFiles(parentPath)))
            {
                if (fieldIndex < _columnCount)
                {
                    field[_currentFileRow + 1][fieldIndex] = currentFile;
                    ++fieldIndex;
                }
                else
                {
                    return;
                }
            }
        }

        private void updateLevel(int row, string newPath)
        {
            if (Directory.GetParent(newPath) == null)
            {
                field[row][_currentFileColumn] = newPath;
                return;
            }

            string parentPath = Directory.GetParent(newPath).FullName;

            string[] directories = Directory.GetDirectories(parentPath);

            int newPathIndex = 0;
            while (newPathIndex < directories.Length && directories[newPathIndex] != newPath)
            {
                ++newPathIndex;
            }

            if (newPathIndex == directories.Length)
            {
                // ERROR
                return;
            }


            // Обновляем состояния слева от центра
            int fieldIndex = _currentFileColumn;
            int currentDirIndex = newPathIndex;

            while (fieldIndex >= 0 && currentDirIndex >= 0)
            {
                field[row][fieldIndex] = directories[currentDirIndex];

                --fieldIndex;
                --currentDirIndex;
            }

            // Обновляем состоянония справа от центра
            fieldIndex = _columnCount / 2 + 1;
            currentDirIndex = newPathIndex + 1;

            while (fieldIndex < _columnCount && currentDirIndex < directories.Length)
            {
                field[row][fieldIndex] = directories[currentDirIndex];

                ++fieldIndex;
                ++currentDirIndex;
            }

            // Пытаемся дополнить строку файлами
            if (fieldIndex < _columnCount)
            {
                foreach (string currentFile in Directory.GetFiles(parentPath))
                {
                    if (fieldIndex < _columnCount)
                    {
                        field[row][fieldIndex] = currentFile;
                        ++fieldIndex;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void setCurrentFile(string path)
        {
            eraseField();
            updateBottomLevel(path);

            for (int row = _currentFileRow; row >= 0; --row)
            {
                updateLevel(row, path);

                if (Directory.GetParent(path) == null)
                {
                    return;
                }

                path = Directory.GetParent(path).FullName;
            }
        }

        public FileSystemModel(int rowCount, int columnCount, string currentPath)
        {
            // TODO: check argumnts
            // rowCount > 1
            // columnCount > 0
            _rowCount = rowCount;
            _columnCount = columnCount;

            _currentFileRow = _rowCount - 2;
            _currentFileColumn = _columnCount / 2;

            field = new string[_rowCount][];

            for (int i = 0; i < _rowCount; ++i)
            {
                field[i] = new string[_columnCount];
            }

            setCurrentFile(currentPath);
        }

        public void SelectNewCurrentFile(int row, int column)
        {
            // TODO: check 0 <= row < rowCount && 0 <= column < columnCount
            setCurrentFile(field[row][column]);
        }

        public void Print()
        {
            for (int i = 0; i < _rowCount; ++i)
            {
                for (int j = 0; j < _columnCount; ++j)
                {
                    if (field[i][j].Length > 0)
                    {
                        System.Console.Write(field[i][j].Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).Last());
                    }
                    System.Console.Write("\t\t");
                }

                System.Console.WriteLine();
            }
        }
    }
}
