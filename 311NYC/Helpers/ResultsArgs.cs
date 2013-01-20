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
    public class ResultsArgs : EventArgs
    {
        public ResultsArgs(Exception ex)
        {
            _error = ex;
        }

        private Exception _error;

        public Exception Error
        {
            get { return _error; }
            set { _error = value; }
        }

    }
}
