using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameFile.Constants
{
    /// <summary>
    /// ファイル名のテンプレート
    /// </summary>
    public static class FileNameTemplate
    {
        /// <summary>
        /// シーケンステンプレート
        /// </summary>
        public static readonly string SEQUENCE_TEMPLATE = @"[%SEQUENCE%]";

        /// <summary>
        /// 日時テンプレート
        /// </summary>
        public static readonly string DATETIME_TEMPLATE = @"[%TIMESTAMP%]";

        /// <summary>
        /// 親ディレクトリ名のテンプレート
        /// </summary>
        public static readonly string PARENT_DIRECTORY_NAME_TEMPLATE = @"[%PARENT%]";

        /// <summary>
        /// 一意値のテンプレート
        /// </summary>
        public static readonly string UNIQUE_IDENTIFIER_TEMPLATE = @"[%UID%]";

        /// <summary>
        /// 拡張子のテンプレート
        /// </summary>
        public static readonly string SOURCE_EXTENSION_TEMPLATE = @"[%EXTENSION%]";
    }
}
