using System.IO;
using System.Linq;

namespace FileBrowser.Model
{
    public class FileSystemModel
    {
        readonly int _rowCount;
        readonly int _columnCount;
        readonly int _currentFileRow;
        readonly int _currentFileColumn;

        public FileItem[][] Field;

        private void eraseField()
        {
            for (int i = 0; i < _rowCount; ++i)
            {
                for (int j = 0; j < _columnCount; ++j)
                {
                    Field[i][j].Path = "";
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
                    Field[_currentFileRow + 1][fieldIndex].Path = currentFile;
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
                Field[row][_currentFileColumn].Path = newPath;
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
                Field[row][fieldIndex].Path = directories[currentDirIndex];

                --fieldIndex;
                --currentDirIndex;
            }

            // Обновляем состоянония справа от центра
            fieldIndex = _columnCount / 2 + 1;
            currentDirIndex = newPathIndex + 1;

            while (fieldIndex < _columnCount && currentDirIndex < directories.Length)
            {
                Field[row][fieldIndex].Path = directories[currentDirIndex];

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
                        Field[row][fieldIndex].Path = currentFile;
                        ++fieldIndex;
                    }
                    else
                    {
                        return;
                    }
                }
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

            Field = new FileItem[_rowCount][];

            for (int i = 0; i < _rowCount; ++i)
            {
                Field[i] = new FileItem[_columnCount];

                for (int j = 0; j < _columnCount; ++j)
                {
                    Field[i][j] = new FileItem(this);
                }
            }

            SetCurrentFile(currentPath);
        }

        public void SetCurrentFile(string path)
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

        public FileItem[][] GetField()
        {
            return Field;
        }

        public void Print()
        {
            for (int i = 0; i < _rowCount; ++i)
            {
                for (int j = 0; j < _columnCount; ++j)
                {
                    if (Field[i][j].Path.Length > 0)
                    {
                        System.Console.Write(Field[i][j].Name);
                    }
                    System.Console.Write("\t\t");
                }

                System.Console.WriteLine();
            }
        }
    }
}
