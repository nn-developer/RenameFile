using Microsoft.Xaml.Behaviors;
using RenameFile.Behaviors.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RenameFile.Behaviors
{
    public class IntegerOnlyTextBoxBehavior : FeBehaviorBase<TextBox>
    {
        /// <summary>
        /// イベント登録
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            // テキスト取得時の処理イベントの追加
            this.AssociatedObject.PreviewTextInput += this.OnPreviewTextInput;

            // PreviewTextInputにてスペースをスルーしないようにする
            this.AssociatedObject.InputBindings.Add(
                new KeyBinding(
                    ApplicationCommands.NotACommand, 
                    Key.Space, 
                    ModifierKeys.None));

            this.AssociatedObject.InputBindings.Add(
                new KeyBinding(
                    ApplicationCommands.NotACommand, 
                    Key.Space, 
                    ModifierKeys.Shift));

            this.AssociatedObject.LostFocus += this.OnLostFocus;

            // IMEモードを無効化
            InputMethod.SetIsInputMethodEnabled(
                this.AssociatedObject, 
                false);

            // クリップボード処理
            this.AssociatedObject.CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.Paste,
                    this.OnPasteCommand));

            // コンテキストメニュー無効化
            this.AssociatedObject.ContextMenu = null;
        }

        /// <summary>
        /// イベント解除
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            // テキスト取得時の処理イベントの解除
            this.AssociatedObject.PreviewTextInput -= this.OnPreviewTextInput;

            this.AssociatedObject.LostFocus -= this.OnLostFocus;

            // 入力Bindingの初期化
            this.AssociatedObject.InputBindings.Clear();

            // クリップボード処理の初期化
            this.AssociatedObject.CommandBindings.Clear();
        }

        /// <summary>
        /// テキスト取得時の処理イベント
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">入力内容データ</param>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 整数値に変換できない場合は処理をキャンセル
            e.Handled = !this.IsIntegerText(
                sender, 
                e.Text);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var result = default(int);
            if (!string.IsNullOrWhiteSpace(this.AssociatedObject.Text))
                result = int.Parse(this.AssociatedObject.Text);

            this.AssociatedObject.Text = result.ToString();
        }

        /// <summary>
        /// クリップボード貼り付け時の処理イベント
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">実行イベントデータ</param>
        private void OnPasteCommand(object sender, ExecutedRoutedEventArgs e)
        {

            // 整数値に変換できる場合は貼り付け処理を実行
            if (this.IsIntegerText(
                sender, 
                Clipboard.GetText()))
                AssociatedObject.Paste();
        }

        /// <summary>
        /// 入力した内容で数値となるかチェック
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="addText">追加文字</param>
        /// <returns>
        /// true : 数値OK
        /// false : 数値NG
        /// </returns>
        private bool IsIntegerText(
            object sender, 
            string addText)
        {
            if (sender is not TextBox)
                return false;

            var textBox = (TextBox)sender;

            // カーソル位置より前の文字列
            var part1 = textBox.Text[..textBox.SelectionStart];

            // カーソル位置＋選択文字より後の文字列
            var part2 = textBox.Text[(textBox.SelectionStart + textBox.SelectionLength)..];

            // part1とpart2の間に入力された文字を追加
            var text = part1 + addText + part2;

            // 作成した文字列が整数に変換できるか
            return int.TryParse(
                text, 
                out _);
        }
    }
}
