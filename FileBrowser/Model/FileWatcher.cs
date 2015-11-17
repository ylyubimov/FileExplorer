using System.Collections.Generic;

namespace FileBrowser.Model
{
    class FileWatcher
    {
        private readonly List<FileItem> _previousLevel = new List<FileItem>();
        private readonly List<FileItem> _currentLevel = new List<FileItem>();
        private readonly List<FileItem> _nextLevel = new List<FileItem>();

        private int _previousFileIndex;
        private int _currentFileIndex;
        private int _nextFileIndex;

        public List<FileItem> PreviousLevel { get { return _previousLevel; } }
        public List<FileItem> CurrentLevel { get { return _currentLevel; } } 
        public List<FileItem> NextLevel { get { return _nextLevel; } } 

        
    }
}
