using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    public class WpUploadFileData
    {
        public string name;     // ファイル名
        public string type;     // MIMEタイプ
        public byte[] bits;     // バイナリデータ
        public bool overwrite;  // 上書き
        public int post_id;     // 関連付ける記事のID？
    }
}