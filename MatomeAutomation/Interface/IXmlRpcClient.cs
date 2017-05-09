using MatomeAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Interface
{
	interface IXmlRpcClient
	{
		/// <summary>
		/// サイト情報
		/// </summary>
		SiteConfig SiteConfig { get; set; }

		/// <summary>
		/// XMLRPCオブジェクト
		/// </summary>
		Object Proxy { get; set; }

		/// <summary>
		/// 画像重複管理リスト
		/// </summary>
		HashSet<string> ImageUrls { get; set; }

		/// <summary>
		/// XMLRPCを介して記事投稿を行う
		/// </summary>
		/// <param name="Content">記事</param>
		/// <returns>投稿成功した記事番号</returns>
		string NewPost(Content content);

		/// <summary>
		/// XMLRPCを介してカテゴリ情報を取得する
		/// </summary>
		/// <returns>カテゴリ情報</returns>
		List<string> GetCategories();

		/// <summary>
		/// XMLRPCを介してメディアの投稿を行う
		/// </summary>
		/// <returns>成功:True 失敗:False</returns>
		string UploadFile(string url);

	}
}