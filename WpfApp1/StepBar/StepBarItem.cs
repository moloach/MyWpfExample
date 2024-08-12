﻿using System.Windows;
using System.Windows.Controls;


namespace WpfApp1.StepBarControl
{
    public class StepBarItem : ContentControl
    {
        #region Private属性

        #endregion

        #region 依赖属性定义
        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(string), typeof(StepBarItem));
        #endregion

        #region 依赖属性set get

        #endregion

        #region Constructors
        static StepBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepBarItem), new FrameworkPropertyMetadata(typeof(StepBarItem)));
        }
        #endregion

        #region Override方法
        #endregion

        #region Private方法

        #endregion
    }
}
