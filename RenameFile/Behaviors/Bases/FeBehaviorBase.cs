using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RenameFile.Behaviors.Bases
{
    /// <summary>
    /// FrameworkElement用の基本Behavior
    /// </summary>
    /// <typeparam name="T">対象のFrameworkElement</typeparam>
    /// <remarks>
    /// FrameworkElementが破棄されるときにデタッチ処理が実行されない場合があるため、
    /// FrameworkElementのロード及びアンロードイベントで設定と解除を実施。
    /// </remarks>
    public abstract class FeBehaviorBase<T> : Behavior<T> where T : FrameworkElement
    {
        /// <summary>
        /// Behaviorのアタッチ処理
        /// </summary>
        protected override void OnAttached()
        { 
            base.OnAttached();

            this.AssociatedObject.Loaded += this.OnLoaded;

            this.AssociatedObject.Unloaded += this.OnUnloaded;
        }

        /// <summary>
        /// Behaviorのデタッチ処理
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.Loaded -= this.OnLoaded;

            this.AssociatedObject.Unloaded -= this.OnUnloaded;
        }

        /// <summary>
        /// 設定処理の実行有無
        /// </summary>
        private bool _isSetuped = false;

        /// <summary>
        /// アタッチしたオブジェクトのロードイベント処理
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e) => this.Setup();

        /// <summary>
        /// 設定処理
        /// </summary>
        private void Setup()
        {
            // 設定処理の実行有無を判定
            if (this._isSetuped)
                return;

            // 設定処理時の処理
            this.OnSetuped();

            // 設定処理を実行済に設定
            this._isSetuped = true;
        }

        /// <summary>
        /// 設定処理時の処理
        /// </summary>
        protected virtual void OnSetuped() { }

        /// <summary>
        /// アタッチしたオブジェクトのアンロードイベント処理
        /// </summary>
        private void OnUnloaded(object sender, RoutedEventArgs e) => this.TearDown();

        /// <summary>
        /// 設定解除処理
        /// </summary>
        private void TearDown()
        {
            // 設定処理の実行有無を判定
            if (!this._isSetuped)
                return;

            // 設定解除処理の実行有無を判定
            this.OnTearDown();

            // 設定処理を実行済を解除
            this._isSetuped = false;
        }

        /// <summary>
        /// 設定解除時の処理
        /// </summary>
        protected virtual void OnTearDown() { }
    }
}
