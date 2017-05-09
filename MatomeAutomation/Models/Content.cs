using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    /// <summary>
    /// 汎用投稿クラス
    /// </summary>
    public class Content
    {
        //投稿情報
        public string Title { get; set; }
        public string Description { get; set; }
        public string PostType { get; set; }
        public string PostStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PostDate { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Content()
        {
            Title = "";
            Description = "";
            PostType = Constants.PostType.Post;
            PostStatus = Constants.PostStatus.Future;
            CreatedDate = DateTime.Now;
            PostDate = DateTime.Now.AddHours(1);
            Categories = new List<string>();
            Tags= new List<string>();
            Slug = "";
            Body = "";           

        }

    }
}