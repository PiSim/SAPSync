using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPSync.ViewModels
{
    public  class LogDialogViewModel : BindableBase 
    {
        public LogDialogViewModel()
        {

        }

        public string LogText => ReadLog();

        private string ReadLog()
        {
            FileInfo logFile = new FileInfo(Properties.Settings.Default.GeneralLogPath);

            if (!logFile.Exists)
                return null;

            StreamReader logReader = logFile.OpenText();
            string output = logReader.ReadToEnd();
            logReader.Close();

            return output;
        }
    }
}
