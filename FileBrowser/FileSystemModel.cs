using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileBrowser
{
    class FileSystemModel
    {
        int rowCount;
        int columnCount;
        int currentFileRow;
        int currentFileColumn;

        private string[][] field;

        private void eraseField()
        {
            for (int i = 0; i < rowCount; ++i)
            {
                for (int j = 0; j < columnCount; ++j)
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
                if (fieldIndex < columnCount)
                {
                    field[currentFileRow + 1][fieldIndex] = currentFile;
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
                field[row][currentFileColumn] = newPath;
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
            int fieldIndex = currentFileColumn;
            int currentDirIndex = newPathIndex;

            while (fieldIndex >= 0 && currentDirIndex >= 0)
            {
                field[row][fieldIndex] = directories[currentDirIndex];

                --fieldIndex;
                --currentDirIndex;
            }

            // Обновляем состоянония справа от центра
            fieldIndex = columnCount / 2 + 1;
            currentDirIndex = newPathIndex + 1;

            while (fieldIndex < columnCount && currentDirIndex < directories.Length)
            {
                field[row][fieldIndex] = directories[currentDirIndex];

                ++fieldIndex;
                ++currentDirIndex;
            }

            // Пытаемся дополнить строку файлами
            if (fieldIndex < columnCount)
            {
                foreach (string currentFile in Directory.GetFiles(parentPath))
                {
                    if (fieldIndex < columnCount)
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

            for (int row = currentFileRow; row >= 0; --row)
            {
                updateLevel(row, path);

                if (Directory.GetParent(path) == null)
                {
                    return;
                }

                path = Directory.GetParent(path).FullName;
            }
        }

        public FileSystemModel(int _rowCount, int _columnCount, string currentPath)
        {
            // TODO: check argumnts
            // rowCount > 1
            // columnCount > 0
            rowCount = _rowCount;
            columnCount = _columnCount;

            currentFileRow = rowCount - 2;
            currentFileColumn = columnCount / 2;

            field = new string[rowCount][];

            for (int i = 0; i < rowCount; ++i)
            {
                field[i] = new string[columnCount];
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
            for (int i = 0; i < rowCount; ++i)
            {
                for (int j = 0; j < columnCount; ++j)
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
