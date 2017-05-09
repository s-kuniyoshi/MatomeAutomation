using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatomeAutomation.Models
{
    /// <summary>
    /// Wordpress向け投稿クラス
    /// </summary>
    public class WpContent
    {
        [XmlRpcMember("post_title")]
        public string PostTitle;

        [XmlRpcMember("post_content")]  
        public string PostContent;

        [XmlRpcMember("terms_names")]
        public XmlRpcStruct TermsNames;

        [XmlRpcMember("post_status")]
        public string PostStatus;

        [XmlRpcMember("post_date")] 
        public DateTime PostDate;          

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="postTitle">投稿タイトル</param>
        /// <param name="postContent">投稿内容</param>
        /// <param name="categories">カテゴリ</param>
        /// <param name="tags">タグ</param>
        /// <param name="postStatus">投稿ステータス</param>
        /// <param name="postDate">投稿日</param>
        public WpContent(string postTitle, string postContent, List<string> categories, List<string>tags, string postStatus, DateTime postDate)
        {
            this.PostTitle = postTitle;
            this.PostContent = postContent;
            this.TermsNames = new XmlRpcStruct();
            this.TermsNames.Add("category", categories.ToArray<string>());
            this.TermsNames.Add("post_tag", tags.ToArray<string>());
            this.PostStatus = postStatus;
            this.PostDate = postDate;

        }
    }
}
