using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileBrowser.Model {
	public class FileSystemModel {
        public FileSystemModel(int rowCount, string currentPath, IEnumerable<ListBox> listBoxes,
            IEnumerable<ScrollViewer> scrollViewers)
        {
            if (rowCount <= 1)
            {
                throw new ArgumentException("Row count must be 2 or more");
            }

            _currentFileRow = rowCount - 2;
            _currentFileColumn = 0;

            _field = new List<FileItem>[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                _field[i] = new List<FileItem>();
            }

            _listBoxes = listBoxes.ToList();
            _scrollViewers = scrollViewers.ToList();

            SetCurrentFile(currentPath);
        }

        /// <summary>
        /// Specifies the field (table) for file system model.
        /// </summary>
		public List<FileItem>[] Field
        {
			get {
                return _field;
            }
		}

        /// <summary>
        /// Specifies the scrolls for file system model.
        /// </summary>
        public List<ScrollViewer> ScrollViewer
        {
			get {
                return _scrollViewers;
            }
		}

        /// <summary>
        /// Select the right file if its possible
        /// </summary>
        public void SelectRightFile()
		{
			// Check that the right file exists
			if( _currentFileColumn + 1 < _field[1].Count ) {
				SetCurrentFile( _field[1][_currentFileColumn + 1].Path );
			}
		}


        /// <summary>
        /// Select the left file if its possible
        /// </summary>
        public void SelectLeftFile()
		{
            // Check that the left file exists
            if( _currentFileColumn > 0 ) {
				SetCurrentFile( _field[1][_currentFileColumn - 1].Path );
			}
		}

        /// <summary>
        /// Set current file using full path to it
        /// </summary>
        public void SetCurrentFile( string path )
        {
            if( ( File.GetAttributes( path ) & FileAttributes.Directory ) != FileAttributes.Directory ) {
                System.Diagnostics.Process.Start( path );
                return;
            }

            if( path.Length == 0 ) {
                return;
            }

            try {
                Directory.GetDirectories( path );
                Directory.GetFiles( path );
            }
            catch( UnauthorizedAccessException ) {
                return;
            }

            updateBottomLevel( path );
            var tmpPath = path;

            for( int row = _currentFileRow; row >= 0; --row ) {
                // Update parent rows
                updateLevel( row, tmpPath );
                if( Directory.GetParent( tmpPath ) == null ) {
                    break;
                }
                tmpPath = Directory.GetParent( tmpPath ).FullName;
            }

            for( int i = 0; i < _field.Length; ++i ) {
                _listBoxes[i].Items.Clear();
                foreach( var element in _field[i] ) {
                    _listBoxes[i].Items.Add( element );
                }
            }

            double koef = 1.0 * ( _scrollViewers[_currentFileRow].ExtentWidth 
                - _scrollViewers[_currentFileRow].ViewportWidth ) 
                / _field[_currentFileRow].Count();

            _scrollViewers[_currentFileRow].ScrollToHorizontalOffset( _currentFileColumn * koef );
        }

        /// <summary>
        /// Clears all field table
        /// </summary>
        private void eraseField()
        {
            foreach( var row in _field ) {
                row.Clear();
            }
        }

        /// <summary>
        /// Fill bottom lever of the field table
        /// </summary>
        private void fillBottomLevel( string path )
        {
			foreach( var file in Directory.GetDirectories( path ).Concat( Directory.GetFiles( path ) ) ) {
				addNewItemInField( _currentFileRow + 1, file );
			}
		}

        /// <summary>
        /// Update 2 bottom
        /// </summary>
        private void updateBottomLevel( string parentPath )
        {
			// Clear all field
			eraseField();

			// Fill the middle row
			if( !Directory.Exists( parentPath ) ) {
				_field[_currentFileRow].Add( new FileItem( this, parentPath ) );
				return;
			}

			// Fill the bottom row
			fillBottomLevel( parentPath );
		}

        /// <summary>
        /// Add all files from newPath to the row
        /// </summary>
        private void updateLevel( int row, string newPath )
        {
			// Check if it is root directory
			if( Directory.GetParent( newPath ) == null ) {
				addNewItemInField( row, newPath );
				return;
			}

			var parentPath = Directory.GetParent( newPath ).FullName;
			var directories = Directory.GetDirectories( parentPath );
			var files = Directory.GetFiles( parentPath );

			_currentFileColumn = findCurrentColumn( row, newPath, parentPath );

			foreach( var directory in directories.Concat( files ) ) {
				addNewItemInField( row, directory );
			}
		}

        /// <summary>
        /// Add new file in the field
        /// </summary>
        private void addNewItemInField( int row, string path )
        {
			_field[row].Add( new FileItem( this, path ) );
			_field[row].Last().Path = path;
		}

        /// <summary>
        /// Find current column number of the selected file
        /// </summary>
        private int findCurrentColumn( int row, string newPath, string parentPath )
        {
			var directories = Directory.GetDirectories( parentPath );

			if( row == _currentFileRow ) {
				for( int i = 0; i < directories.Length; i++ ) {
					if( directories[i] == newPath ) {
						return i;
					}
				}
			}

			return _currentFileColumn;
		}

        // Number of currently selected row of the file
        private readonly int _currentFileRow;
        // Number of currently selected column of the file
        private int _currentFileColumn;
        // Field table, contains all files
        private readonly List<FileItem>[] _field;
        private readonly List<ListBox> _listBoxes;
        // Scrolls for all rows in the table
        private readonly List<ScrollViewer> _scrollViewers;
    }
}