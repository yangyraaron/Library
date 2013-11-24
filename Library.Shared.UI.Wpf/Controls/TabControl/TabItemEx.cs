using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Library.Client.Wpf.Controls
{
    [TemplatePart(Name = PART_CLOSE_BUTTON, Type = typeof(ButtonBase))]
    public class TabItemEx:TabItem
    {
        private const string PART_CLOSE_BUTTON = "PART_CloseButton";
        private ButtonBase _closeButton;

        static TabItemEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItemEx),
                new FrameworkPropertyMetadata(typeof(TabItemEx)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._closeButton = GetTemplateChild(PART_CLOSE_BUTTON) as ButtonBase;

            if (this._closeButton == null)
                return;
            
            //attach the close handler to the close button
            this._closeButton.Click += new RoutedEventHandler(Close);
        }

        private void Close(object o,RoutedEventArgs e)
        {
            TabControl tabControl = WpfExtensions.FindLogicParent<TabControl>(this);

            if (tabControl == null)
                return;

            tabControl.Items.Remove(this);

            //after remove the item from the tabcontrol,detach the hander from the click event
            this._closeButton.Click -= new RoutedEventHandler(Close);
        }

        /// <summary>
        /// when mouse enters then add the header as the tooltip
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            this.ToolTip = Header.CloneElement();
            e.Handled = true;
        }

        /// <summary>
        /// when mouse leaves then remove the tooltip
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            this.ToolTip = null;
            e.Handled = true;
        }
    }
}
