using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    class Assists
    {
        #region Badge 标记
        #region 标记内容
        public static string GetBadge(DependencyObject obj)
        {
            return (string)obj.GetValue(BadgeProperty);
        }

        public static void SetBadge(DependencyObject obj, string value)
        {
            obj.SetValue(BadgeProperty, value);
        }

        // Using a DependencyProperty as the backing store for Badge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.RegisterAttached("Badge", typeof(string), typeof(Assists), new PropertyMetadata(null, BadgePropertyChanged));

        private static void BadgePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uIElement)
            {
                var adorner = (BadgeAdorber)uIElement.GetOrAddAdorner(typeof(BadgeAdorber));
                adorner.SetContext(e.NewValue?.ToString());
            }
        }
        #endregion

        #region 标记位置
        public static EnumAlignment GetBadgeAlignment(DependencyObject obj)
        {
            return (EnumAlignment)obj.GetValue(BadgeAlignmentProperty);
        }

        public static void SetBadgeAlignment(DependencyObject obj, EnumAlignment value)
        {
            obj.SetValue(BadgeAlignmentProperty, value);
        }

        // Using a DependencyProperty as the backing store for BadgeAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeAlignmentProperty =
            DependencyProperty.RegisterAttached("BadgeAlignment", typeof(EnumAlignment), typeof(Assists), new PropertyMetadata(EnumAlignment.TopRight, BadgeAlignmentPropertyChanged));

        private static void BadgeAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uIElement)
            {
                var adorner = (BadgeAdorber)uIElement.GetOrAddAdorner(typeof(BadgeAdorber));
                adorner.SetAlignment((EnumAlignment)e.NewValue);
            }
        }
        #endregion

        #region 是否隐藏标记
        public static bool GetHideBadge(DependencyObject obj)
        {
            return (bool)obj.GetValue(HideBadgeProperty);
        }

        public static void SetHideBadge(DependencyObject obj, bool value)
        {
            obj.SetValue(HideBadgeProperty, value);
        }

        // Using a DependencyProperty as the backing store for HideBadge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HideBadgeProperty =
            DependencyProperty.RegisterAttached("HideBadge", typeof(bool), typeof(Assists), new PropertyMetadata(false, HideBadgePropertyChanged));

        private static void HideBadgePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uIElement)
            {
                var adorner = (BadgeAdorber)uIElement.GetOrAddAdorner(typeof(BadgeAdorber));
                adorner.Visibility = (bool)e.NewValue ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        #endregion

        #endregion
    }


    public enum EnumAlignment
    {
        [Description("左上")]
        TopLeft,
        [Description("中上")]
        TopCenter,
        [Description("右上")]
        TopRight,
        [Description("左中")]
        CenterLeft,
        [Description("中")]
        Middle,
        [Description("右中")]
        CenterRight,
        [Description("左下")]
        BottomLeft,
        [Description("中下")]
        BottomCenter,
        [Description("右下")]
        BottomRight,
    }
}
