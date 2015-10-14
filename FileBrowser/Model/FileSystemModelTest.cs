using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBrowser.Model
{
    class FileSystemModelTest
    {
        static void Test()
        {
            FileSystemModel model = new FileSystemModel(5, 5, "C:\\");
            model.Print();
            model.SelectNewCurrentFile(4, 4);
            model.Print();
            model.SelectNewCurrentFile(4, 0);
            model.Print();
        }
    }
}
