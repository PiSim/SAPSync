using Prism.Mvvm;
using System.IO;

namespace SAPSync.ViewModels
{
    public class LogDialogViewModel : BindableBase
    {
        #region Constructors

        public LogDialogViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public string LogText => ReadLog();

        #endregion Properties

        #region Methods

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

        #endregion Methods
    }
}