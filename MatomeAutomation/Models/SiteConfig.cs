
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    public class SiteConfig
    {

        public int BlogId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string BaseUrl { get; set; }

        public string FullUrl { get { return string.Concat(BaseUrl, BaseUrl.EndsWith("/") ? "xmlrpc.php" : "/xmlrpc.php"); } }

        /// <summary>
        /// ブログタイプ 0:Wordpress 1:MovableType
        /// </summary>
        public int BlogType { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SiteConfig()
        {

            //TODO　色々初期処理を追加しておく

        }
    }
}