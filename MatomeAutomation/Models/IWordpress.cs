using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    public interface IWordpress : IXmlRpcProxy
    {
        /// <summary>
        /// Wordpressへ記事の投稿を行う
        /// </summary>
        /// <param name="blog_id">ブログID（基本的に1）</param>
        /// <param name="username">ログインID</param>
        /// <param name="password">ログインパスワード</param>
        /// <param name="content">記事</param>
        /// <returns></returns>
        [XmlRpcMethod("wp.newPost")]
        string NewPost(int blog_id, string username, string password, WpContent content);

        /// <summary>
        /// WordpressからTermを取得する
        /// </summary>
        /// <param name="blog_id">ブログID（基本的に1）</param>
        /// <param name="username">ログインID</param>
        /// <param name="password">ログインパスワード</param>
        /// <param name="taxonomy">タクソノミー(TagとかCategoryとか)</param>
        /// <returns></returns>
        [XmlRpcMethod("wp.getTerms")]
        WpTerm[] GetTerms(int blog_id, string username, string password, string taxonomy);

        /// <summary>
        /// Wordpressへファイルのアップロードを行う
        /// </summary>
        /// <param name="blog_id">ブログID（基本的に1）</param>
        /// <param name="username">ログインID</param>
        /// <param name="password">ログインパスワード</param>
        /// <param name="data">アップロードファイル</param>
        /// <returns></returns>
        [XmlRpcMethod("wp.uploadFile")]
        WpUploadFileResult UploadFile(int blog_id, string username, string password, WpUploadFileData data);

        /// <summary>
        /// Wordpressからメディア情報を取得する
        /// </summary>
        /// <param name="blog_id">ブログID（基本的に1）</param>
        /// <param name="username">ログインID</param>
        /// <param name="password">ログインパスワード</param>
        /// <param name="attachment_id">メディアID</param>
        /// <returns></returns>
        [XmlRpcMethod("wp.getMediaItem")]
        MediaItem GetMediaItem(int blog_id, string username, string password, int attachment_id);
    }
}