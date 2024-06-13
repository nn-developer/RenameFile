using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using RenameFile.Behaviors.Bases;

namespace RenameFile.Behaviors
{
    public class SelectAllTextBoxBehavior : FeBehaviorBase<TextBox>
    {
        /// <summary>
        /// フォーカス取得時の処理
        /// </summary>
        private void OnGotFocus(object sender, RoutedEventArgs e) => this.AssociatedObject.SelectAll();

        /// <summary>
        /// ビヘイビアのアタッチ処理
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.GotFocus += this.OnGotFocus;
        }

        /// <summary>
        /// ビヘイビアのデタッチ処理
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.GotFocus -= this.OnGotFocus;
        }
    }
}
