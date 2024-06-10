using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RenameFile.DataSources
{
    /// <summary>
    /// リネームファイルのデータソース
    /// </summary>
    public record struct RenameFileDataSource
    {
        /// <summary>
        /// リネーム対象のルートディレクトリのパス
        /// </summary>
        public string RootDirectoryPath { get; set; }

        /// <summary>
        /// シーケンスを初期化すべきかの判定値
        /// </summary>
        public bool ShouldInitializeSequence { get; set; }

        /// <summary>
        /// 親ディレクトリ名テンプレートの存在有無
        /// </summary>
        public bool ContainsParentDirectoryNameTemplate { get; set; }

        /// <summary>
        /// シーケンステンプレートの存在有無
        /// </summary>
        public bool ContainsSequenceTemplate { get; set; }

        /// <summary>
        /// シーケンスの初期値
        /// </summary>
        public int InitialSequence { get; set; }

        /// <summary>
        /// シーケンス書式
        /// </summary>
        public string SequenceFormat { get; set; }

        /// <summary>
        /// 日時テンプレートの存在有無
        /// </summary>
        public bool ContainsFimeNameDatetimeTemplate { get; set; }

        /// <summary>
        /// リネーム対象の日時
        /// </summary>
        public DateTime FimeNameDatetime { get; set; }

        /// <summary>
        /// リネーム対象の日時書式
        /// </summary>
        public string FimeNameDatetimeFormat { get; set; }

        /// <summary>
        /// 一意値のテンプレートの存在有無
        /// </summary>
        public bool ContainsUniqueIdentifierTemplate { get; set; }

        /// <summary>
        /// 一意値の書式
        /// </summary>
        public string UniqueIdentifierFormat { get; set; }

        /// <summary>
        /// リネームするファイル名のパターン
        /// </summary>
        public string FileNamePattern { get; set; }

        /// <summary>
        /// 対象の拡張子
        /// </summary>
        public List<string> TargetFileExtensions { get; set; }
    }
}
