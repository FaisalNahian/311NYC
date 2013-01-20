using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

namespace _311NYC
{
	public partial class LoadingBar : UserControl
	{
		public LoadingBar()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        #region EnableAnimation
        public static readonly DependencyProperty EnableAnimationProperty = DependencyProperty.Register(
          "EnableAnimation",
          typeof(bool),
          typeof(LoadingBar),
          new PropertyMetadata(OnEnableAnimationChanged));

        public bool EnableAnimation
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        private static void OnEnableAnimationChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadingBar ctrl = (LoadingBar)sender;
            ctrl.OnEnableAnimationChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnEnableAnimationChanged(object oldValue, object newValue)
        {
            if ((bool)newValue)
            {
                this.progressBar.IsIndeterminate = true;
            }
            else
            {
                this.progressBar.IsIndeterminate = false;
            }
        }
        #endregion

        #region LoadingText
        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register(
          "LoadingText",
          typeof(string),
          typeof(LoadingBar),
          new PropertyMetadata(OnLoadingTextChanged));

        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        private static void OnLoadingTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadingBar ctrl = (LoadingBar)sender;
            ctrl.OnLoadingTextChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnLoadingTextChanged(object oldValue, object newValue)
        {
            this.txtLoadingText.Text = newValue.ToString();
        }
        #endregion
		
		#region GridBackground
        public static readonly DependencyProperty GridBackgroundProperty = DependencyProperty.Register(
          "GridBackground",
          typeof(Brush),
          typeof(LoadingBar),
          new PropertyMetadata(OnGridBackgroundChanged));

        public Brush GridBackground
        {
            get { return (Brush)GetValue(GridBackgroundProperty); }
            set { SetValue(GridBackgroundProperty, value); }
        }

        private static void OnGridBackgroundChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadingBar ctrl = (LoadingBar)sender;
            ctrl.OnGridBackgroundChanged(e.OldValue, e.NewValue);
        }

        protected virtual void OnGridBackgroundChanged(object oldValue, object newValue)
        {
			this.LayoutRoot.Background =newValue as Brush;
        }
        #endregion
	}
}