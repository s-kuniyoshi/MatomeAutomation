using MatomeAutomation.Interface;
using MatomeAutomation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MatomeAutomation.Controllers
{
    public class HomeController : Controller
    {
        //TODO あとでけす
        private static string username = "root";
        private static string passWord = "@Since2014";
        private static string url = "http://gadget-news.jp/";
        private IXmlRpcClient rpcClient;

        public ActionResult Index()
        {
            return View();
        }

        // POST: /Home/Result
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MyProcessThread model)
        {
            if (ModelState.IsValid)
            {
                //TODO ここで設定ファイル作るのもなぁ・・・
                SiteConfig siteconfig = new SiteConfig();
                siteconfig.BlogId = 1;
                siteconfig.Username = username;
                siteconfig.Password = passWord;
                siteconfig.BaseUrl = url;
                siteconfig.BlogType = 0;

                //設定ファイルに応じてインスタンスを変化
                switch (siteconfig.BlogType)
                {

                    case 0: //Wordpressの場合
                        rpcClient = new WordpressClient(siteconfig);
                        break;

                    case 1: //MovableTypeの場合
                        //TOOD そのうち作る
                        break;
                }

                //投稿する記事を作成
                MyProcessThread tmpThread = new MyProcessThread(model.Url, model.ExtractRes);

                //汎用投稿クラスへオブジェクトをセット
                Content content = new Content();
                content.Title = tmpThread.Title;
                content.Tags = tmpThread.GetTags();
                content.Body = tmpThread.Html;

                //本文中に一致するカテゴリを格納
                List<string> categories = new List<string>();
                foreach (string category in rpcClient.GetCategories())
                {

                    if (content.Body.IndexOf(category) > 1)
                    {
                        categories.Add(category);
                    }

                }

                content.Categories = categories;

                //画像イメージを含むhrefタグを取得
                Regex hrefReg = new Regex(@"<a[^>]href[^>]*\.(jpg|jpeg|gif|png)[^>]*>.*?</a>");
                for (Match href = hrefReg.Match(content.Body); href.Success; href = href.NextMatch())
                {

                    //画像URLを取得
                    Regex imgReg = new Regex(@"(https?)(:\/\/[-_.!~*\'()a-zA-Z0-9;\/?:\@&=+\$,%#]+)\.(jpg|jpeg|gif|png)");
                    for (Match img = imgReg.Match(href.Value); img.Success; img = img.NextMatch())
                    {
                        string result = rpcClient.UploadFile(img.Value);
                        if (string.IsNullOrWhiteSpace(result) == false)
                        {
                            //Placeholderを定義
                            string placeHolder = "<img class=\"image pict\" src=\"{0}\" alt=\"no title\" border=\"0\" hspace=\"5\">";

                            string imgUrl = String.Format(placeHolder, result);

                            //アップロード成功時はHTMLを置換
                            content.Body = content.Body.Replace(href.Value, imgUrl);
                        }
                    }

                }

                //動画イメージを含むhrefタグを取得
                Regex movieReg = new Regex(@"<a[^>]href[^>]*(youtube|www\.youtube|youtu\.be)/([a-zA-Z0-9]*)[^>]*>.*?</a>");
                for (Match movie = movieReg.Match(content.Body); movie.Success; movie = movie.NextMatch())
                {
             
                    //Placeholderを定義
                    string placeHolder = @"<iframe src=""http://www.youtube.com/embed/{0}"" width=""420"" height=""315"" frameborder=""0"" allowfullscreen=""allowfullscreen""></iframe>";

                    //投稿に表示する画像はサムネイル版にする
                    string imgUrl = String.Format(placeHolder, movie.Groups[2].Value);

                    //アップロード成功時はHTMLを置換
                    content.Body = content.Body.Replace(movie.Value, imgUrl);

                }

                //記事の投稿
                rpcClient.NewPost(content);

                ViewBag.tags = content.Tags;
                ViewBag.postDate = content.PostDate;
                ViewBag.categories = content.Categories;
                ViewBag.result = true;

                return View();
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}