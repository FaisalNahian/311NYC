using System.Windows.Input;
using Microsoft.Phone.Controls.Maps;
using System;

//namespace _311NYC.Controls
//{
    //this is no longer supported because the WP7 Bing SL map control does not allow yo to inherit from Pushpin
    //and there are no MapMouseDragEvents but does have MapDragEvents

//    public class DraggablePushPin : Pushpin
//    {
//        private bool isDragging = false;
//        EventHandler<MapDragEventArgs> ParentMapMousePanHandler;
        
//        MouseButtonEventHandler ParentMapMouseLeftButtonUpHandler;
//        MouseEventHandler ParentMapMouseMoveHandler;

//        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
//        {
//            // Check if the Map Event Handlers have been created/attached to the Map
//            // If not, then attach them. This is done in the "Pushpin.OnMouseLeftButtonDown"
//            // event because we don't know when the Pushpin is added to a Map or MapLayer, but
//            // we do konw that when this event is fired the Pushpin will already have been added.
//            var parentLayer = this.Parent as MapLayer;
//            if (parentLayer != null)
//            {
//                var parentMap = parentLayer.ParentMap;
//                if (parentMap != null)
//                {
//                    if (this.ParentMapMousePanHandler == null)
//                    {
//                        this.ParentMapMousePanHandler = new EventHandler<MapDragEventArgs>(ParentMap_MousePan);
//                        parentMap.MousePan += this.ParentMapMousePanHandler;
//                    }
//                    if (this.ParentMapMouseLeftButtonUpHandler == null)
//                    {
//                        this.ParentMapMouseLeftButtonUpHandler = new MouseButtonEventHandler(ParentMap_MouseLeftButtonUp);
//                        parentMap.MouseLeftButtonUp += this.ParentMapMouseLeftButtonUpHandler;
//                    }
//                    if (this.ParentMapMouseMoveHandler == null)
//                    {
//                        this.ParentMapMouseMoveHandler = new MouseEventHandler(ParentMap_MouseMove);
//                        parentMap.MouseMove += this.ParentMapMouseMoveHandler;
//                    }
//                }
//            }

//            // Enable Dragging
//            this.isDragging = true;

//            base.OnMouseLeftButtonDown(e);
//        }

//        #region "Mouse Event Handler Methods"

//        void ParentMap_MousePan(object sender, MapMouseDragEventArgs e)
//        {
//            // If the Pushpin is being dragged, specify that the Map's MousePan
//            // event is handled. This is to suppress the Panning of the Map that
//            // is done when the mouse drags the map.
//            if (this.isDragging)
//            {
//                e.Handled = true;
//            }
//        }

//        void ParentMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
//        {
//            // Left Mouse Button released, stop dragging the Pushpin
//            this.isDragging = false;
//        }

//        void ParentMap_MouseMove(object sender, MouseEventArgs e)
//        {
//            var map = sender as Microsoft.Maps.MapControl.Map;
//            // Check if the user is currently dragging the Pushpin
//            if (this.isDragging)
//            {
//                // If so, the Move the Pushpin to where the Mouse is.
//                var mouseMapPosition = e.GetPosition(map);
//                mouseMapPosition.Y = mouseMapPosition.Y + (this.Height * 3 / 4);
//                var mouseGeocode = map.ViewportPointToLocation(mouseMapPosition);
//                this.Location = mouseGeocode;
//            }
//        }

//        #endregion
//    }
//}