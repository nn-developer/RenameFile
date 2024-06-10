using RenameFile.Constants;
using RenameFile.DataSources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RenameFile.Models
{
    public class RenameFileModel
    {
        /// <summary>
        /// リネームファイルのデータソース
        /// </summary>
        public RenameFileDataSource DataSource { get; init; }

        /// <summary>
        /// ファイル名のシーケンス値
        /// </summary>
        public int CurrentSequence { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataSource">リネームファイルのデータソース</param>
        public RenameFileModel(RenameFileDataSource dataSource) => this.DataSource = dataSource;

        /// <summary>
        /// ファイル名生成用の文字列操作クラス
        /// </summary>
        private StringBuilder _fileNameBuilder = new();

        /// <summary>
        /// ファイル名を評価
        /// </summary>
        /// <returns>評価結果で取得したファイル名</returns>
        public string EvaluateFileName(string filePath)
        {
            // ファイル名生成用の文字列操作クラスを初期化
            this._fileNameBuilder.Clear();

            // ファイル名を取得
            var fileName = Path.GetFileName(filePath);

            // ファイル名生成用のパターン文字列を成形
            this._fileNameBuilder.Append(this.DataSource.FileNamePattern);
            var extension = Path.GetExtension(filePath).ToLower();
            this._fileNameBuilder.Replace(
                $".{FileNameTemplate.SOURCE_EXTENSION_TEMPLATE}",
                extension);

            // リネーム対象の親ディレクトリ名の置換
            if (this.DataSource.ContainsParentDirectoryNameTemplate)
            {
                var parentDirectoryPath = Path.GetDirectoryName(filePath);
                var parentDirectoryName = Path.GetFileName(parentDirectoryPath);
                this._fileNameBuilder.Replace(
                    FileNameTemplate.PARENT_DIRECTORY_NAME_TEMPLATE,
                    parentDirectoryName);
            }

            // リネーム対象のシーケンス値の置換
            if (this.DataSource.ContainsSequenceTemplate)
            {
                var sequence = this.CurrentSequence++;
                var sequenceString = sequence.ToString(this.DataSource.SequenceFormat);
                this._fileNameBuilder.Replace(
                    FileNameTemplate.SEQUENCE_TEMPLATE,
                    sequenceString);
            }

            // リネーム対象の日時の置換
            if (this.DataSource.ContainsFimeNameDatetimeTemplate)
            {
                var dateTimeString = this.DataSource.FimeNameDatetime.ToString(this.DataSource.FimeNameDatetimeFormat);
                this._fileNameBuilder.Replace(
                    FileNameTemplate.DATETIME_TEMPLATE,
                    dateTimeString);
            }

            // リネーム対象の一意値の置換
            if (this.DataSource.ContainsUniqueIdentifierTemplate)
            {
                var uniqueIdentifierString = Guid.NewGuid().ToString(this.DataSource.UniqueIdentifierFormat);
                this._fileNameBuilder.Replace(
                    FileNameTemplate.UNIQUE_IDENTIFIER_TEMPLATE,
                    uniqueIdentifierString);
            }

            return this._fileNameBuilder.ToString();
        }

        /// <summary>
        /// リネーム処理
        /// </summary>
        public void Rename()
        {
            // 対象の全ファイルパスを取得
            var allFilePaths = Directory.GetFiles(
                this.DataSource.RootDirectoryPath,
                "*",
                SearchOption.AllDirectories);

            // 拡張子毎にファイルパスを取得
            var targetFilePath = new List<string>();
            foreach (var targetExtention in this.DataSource.TargetFileExtensions.Select(x => x?.ToLower() ?? string.Empty))
            {
                // 拡張子が取得できない場合は処理をスキップ
                if (string.IsNullOrEmpty(targetExtention))
                    continue;

                // 対象の拡張子事に抽出
                var filePaths = allFilePaths.Where(x => Path.GetExtension(x).ToLower() == targetExtention);

                // ディレクトリ名及びファイル名でソート
                var orderedFilePaths = filePaths.OrderBy(x => x).ToList();
                targetFilePath.AddRange(orderedFilePaths);
            }

            // ファイルをリネーム
            this.RenameFileName(targetFilePath);
        }

        /// <summary>
        /// ファイルをリネーム
        /// </summary>
        /// <param name="filePaths">リネーム対象のファイルパス</param>
        private void RenameFileName(IEnumerable<string> filePaths)
        {
            // ディレクトリが変更されたかを検知するための値
            var currentDirectoryPath = string.Empty;

            // ファイル重複抑止のため仮ファイル名でリネーム
            var tmpFilePaths = new List<(string TmpFilePath, string RenameFilePath)>();
            foreach (var filePath in filePaths)
            {
                // ディレクトリが変更になった際にシーケンス値を初期化
                var directoryPath = Path.GetDirectoryName(filePath) ?? string.Empty;
                if (this.DataSource.ShouldInitializeSequence &&
                    currentDirectoryPath != directoryPath)
                {
                    this.CurrentSequence = this.DataSource.InitialSequence;
                    currentDirectoryPath = directoryPath;
                }

                // リネームファイルパスを取得
                var directoryName = Path.GetFileName(directoryPath);
                var fileExtension = Path.GetExtension(filePath);
                var renameFileName = this.EvaluateFileName(filePath);
                var renameFilePath = Path.Combine(
                    directoryPath,
                    renameFileName);

                // 仮ファイルパスを取得
                var tmpFilePath = Path.Combine(
                    directoryPath,
                    $"{Guid.NewGuid()}.tmp");

                // 仮ファイルパスへ退避
                File.Move(filePath,
                    tmpFilePath);

                // 仮ファイルパスとリネームファイルパスを設定
                tmpFilePaths.Add(new(tmpFilePath, renameFilePath));
            }

            // 退避した仮ファイルをリネーム
            foreach (var tmpFilePath in tmpFilePaths)
                File.Move(
                    tmpFilePath.TmpFilePath,
                    tmpFilePath.RenameFilePath);
        }
    }
}
