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
using System.Collections.Generic;

namespace _311NYC.Helpers
{
    public class EntityResultsArgs<T> : ResultsArgs
    {
        public EntityResultsArgs(Exception ex)
            : base(ex)
        {
        }

        public EntityResultsArgs(IEnumerable<T> results)
            : base(null)
        {
            _results = results;
        }

        private IEnumerable<T> _results;

        public IEnumerable<T> Results
        {
            get { return _results; }
        }

    }
}
