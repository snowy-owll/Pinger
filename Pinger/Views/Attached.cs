using System.Windows;

namespace Pinger.Views
{
    public class Attached : DependencyObject
    {
        public static readonly DependencyProperty TitleObjectProperty =
            DependencyProperty.RegisterAttached("TitleObject", typeof(object), typeof(Attached),
            new PropertyMetadata((object)null));        

        public static object GetTitleObject(DependencyObject d)
        {
            return d.GetValue(TitleObjectProperty);
        }

        public static void SetTitleObject(DependencyObject d, object value)
        {
            d.SetValue(TitleObjectProperty, value);
        }        
    }
}
