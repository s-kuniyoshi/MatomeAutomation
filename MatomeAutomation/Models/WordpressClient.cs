using CookComputing.XmlRpc;
using MatomeAutomation.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace MatomeAutomation.Models
{
	public class WordpressClient : IXmlRpcClient, IDisposable
	{
		public SiteConfig SiteConfig { get; set; }

		public Object Proxy { get; set; }

		public HashSet<string> ImageUrls { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="siteConfig"></param>
		public WordpressClient(SiteConfig siteConfig)
		{
			this.ImageUrls = new HashSet<string>();
			this.SiteConfig = siteConfig;

			var tmpProxy = (IWordpress)XmlRpcProxyGen.Create(typeof(IWordpress));
			tmpProxy.Url = this.SiteConfig.FullUrl;
			Proxy = tmpProxy;

		}

		/// <summary>
		/// Wordpressに投稿する
		/// </summary>
		/// <param name="content">汎用投稿クラス</param>
		/// <returns>投稿ID</returns>
		public string NewPost(Content content)
		{

			//Wordpress用の投稿クラスを生成
			WpContent wpContent = new WpContent(content.Title, content.Body, content.Categories, content.Tags, content.PostStatus, content.PostDate);

			return ((IWordpress)Proxy).NewPost(SiteConfig.BlogId, SiteConfig.Username, SiteConfig.Password, wpContent);
		}

		/// <summary>
		/// 対象となるWordpressのカテゴリ一覧を取得する
		/// </summary>
		/// <returns>カテゴリ一覧</returns>
		public List<string> GetCategories()
		{
			List<string> categories = new List<string>();
			WpTerm[] terms = ((IWordpress)Proxy).GetTerms(SiteConfig.BlogId, SiteConfig.Username, SiteConfig.Password, Constants.TaxonomyType.Category);
			foreach (WpTerm term in terms)
			{
				categories.Add(term.name);
			}

			return categories;
		}

		/// <summary>
		/// Wordpressに画像ファイルをアップロードする
		/// </summary>
		/// <param name="url">画像URL</param>
		/// <returns>対象の画像URL</returns>
		public string UploadFile(string url)
		{
			string retVal = "";

			if (ImageUrls.Add(url))
			{
				//画像URLがリストに存在しない場合

				//画像を保存するパスを取得
				string getFileDirPass = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\";


				//URLからファイル名.拡張子(例:hogehoge.png)を取得
				Regex fileName = new Regex(@"([^\/]+?)([\?#].*)?$");
				Match fn = fileName.Match(url);

				//URLからファイル形式を取得
				Regex fileType = new Regex(@"[^.]+$");
				Match ft = fileType.Match(url);

				try
				{

					System.Net.WebClient wc = new System.Net.WebClient();
					wc.DownloadFile(url, getFileDirPass + fn);

					//画像ファイルのデータを格納
					WpUploadFileData data = new WpUploadFileData()
					{
						name = fn.Value,
						type = "image/" + ft.Value,
						bits = File.ReadAllBytes(getFileDirPass + fn),
						overwrite = true
					};

					//開放
					wc.Dispose();
					WpUploadFileResult wpResult = ((IWordpress)Proxy).UploadFile(SiteConfig.BlogId, SiteConfig.Username, SiteConfig.Password, data);
					retVal = wpResult.url;
				}
				catch (Exception)
				{

					retVal = null;
				}
			}

			//アップロード
			return retVal;
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		public void Dispose()
		{
			Proxy = null;
		}
	}
}