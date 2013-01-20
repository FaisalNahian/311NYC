using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace _311NYC.Helpers
{
    public class DataUploadResultsArgs : ResultsArgs
    {
        public DataUploadResultsArgs(Exception ex) : base(ex)
        {
            UploadSuccessfull = false;
        }

        public DataUploadResultsArgs(bool success) : base(null)
        {
            UploadSuccessfull = success;
        }

        public bool UploadSuccessfull { get; private set; }
    }
}
