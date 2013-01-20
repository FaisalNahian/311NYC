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
using _311NYC.Helpers;
using _311NYC.Model;

namespace _311NYC.DataLoader
{
    /// <summary>
    /// Defines a method to be called when a point group has completed loading
    /// </summary>
    /// <param name="args"></param>
    public delegate void PointGroupLoadedCallback(EntityResultsArgs<PointGroup> args);

    /// <summary>
    /// Defines a method to be called when a region group has completed loading
    /// </summary>
    /// <param name="args"></param>
    public delegate void RegionGroupLoadedCallback(EntityResultsArgs<RegionGroup> args);

    /// <summary>
    /// Defines a method to be called when a sharedlandmark group has completed loading
    /// </summary>
    /// <param name="args"></param>
    public delegate void SharedLandmarkGroupLoadedCallback(EntityResultsArgs<SharedLandmarkGroup> args);
    
    /// <summary>
    /// Defines a method to be called when data has been uploaded to the server
    /// </summary>
    /// <param name="args"></param>
    public delegate void DataUploadCallback(DataUploadResultsArgs args);
}
