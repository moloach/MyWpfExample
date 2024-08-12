using System;
using System.Windows.Controls;
using System.Windows.Data;


using SystemItemsControl = System.Windows.Controls.ItemsControl;

namespace WpfApp1.StepBarControl
{

    public enum EnumCompare
    {
        None,
        Less,
        Equal,
        Large
    }


    public class IsProgressedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((values[0] is ContentControl && values[1] is int) == false)
            {
                return EnumCompare.None;
            }

            ContentControl contentControl = values[0] as ContentControl;
            int progress = (int)values[1];
            SystemItemsControl itemsControl = SystemItemsControl.ItemsControlFromItemContainer(contentControl);

            if (itemsControl == null)
            {
                return EnumCompare.None;
            }

            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(contentControl);

            if (index < progress)
            {
                return EnumCompare.Less;
            }
            else if (index == progress)
            {
                return EnumCompare.Equal;
            }
            return EnumCompare.Large;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }



    public class IsLastItemConverter : IMultiValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ContentControl contentPresenter = value[0] as ContentControl;
            SystemItemsControl itemsControl = SystemItemsControl.ItemsControlFromItemContainer(contentPresenter);

            bool flag = false;
            if (itemsControl != null)
            {
                int index = itemsControl.ItemContainerGenerator.IndexFromContainer(contentPresenter);
                flag = (index == (itemsControl.Items.Count - 1));
            }

            return flag;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
