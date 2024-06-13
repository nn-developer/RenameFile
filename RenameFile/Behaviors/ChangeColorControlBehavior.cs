using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using RenameFile.Behaviors.Bases;

namespace RenameFile.Behaviors
{
    public class ChangeColorControlBehavior : FeBehaviorBase<Control>
    {
        /// <summary>
        /// フォーカス取得時の背景色の依存プロパティ
        /// </summary>
        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register(
                "FocusedBackground",
                typeof(Brush),
                typeof(ChangeColorControlBehavior));

        /// <summary>
        /// フォーカス取得時の背景色
        /// </summary>
        public Brush FocusedBackground
        {
            get { return (Brush)GetValue(FocusedBackgroundProperty); }
            set { SetValue(FocusedBackgroundProperty, value); }
        }

        /// <summary>
        /// 規定の背景色
        /// </summary>
        private Brush? _defaultBackBrush = null;

        /// <summary>
        /// フォーカス取得時の処理
        /// </summary>
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this._defaultBackBrush = AssociatedObject.Background;
            AssociatedObject.Background = FocusedBackground;
        }

        /// <summary>
        /// フォーカス喪失時の処理
        /// </summary>
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            // 設定されている背景色を設定
            AssociatedObject.Background = this._defaultBackBrush;
        }

        /// <summary>
        /// ビヘイビアのアタッチ処理
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.GotFocus += this.OnGotFocus;

            AssociatedObject.LostFocus += this.OnLostFocus;
        }

        /// <summary>
        /// ビヘイビアのデタッチ処理
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.GotFocus -= this.OnGotFocus;

            AssociatedObject.LostFocus -= this.OnLostFocus;
        }
    }
}
