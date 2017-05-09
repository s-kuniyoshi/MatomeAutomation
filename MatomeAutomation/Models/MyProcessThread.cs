using CookComputing.XmlRpc;
using NMeCab;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace MatomeAutomation.Models
{
    public class MyProcessThread
    {

        [Required]
        [Display(Name = "2chURL")]
        public string Url { get; set; }

        [Required]
        [Display(Name = "抽出レス数")]
        public int ExtractRes { get; set; }

        public List<Res> ResList { get; set; }
        public byte Thumbnail { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }

        public MyProcessThread()
        {
        }

        public MyProcessThread(string url, int extractRes)
        {
            this.Url = url;
            this.ExtractRes = extractRes;
            ResList = new List<Res>();

            //URLを元にHTMLを取得
            WebClient client = new WebClient();
            string source = client.DownloadString(this.Url);

            // TODO あとでクッションページのURL自体を外部化する
            //2chのクッションページをHTML中から除去
            string replaceSource = source.Replace("2ch.io/", "");

            //取得したHTMLから各種情報を設定
            SetTitle(replaceSource);
            SetResList(replaceSource);
        }

        /// <summary>
        /// タイトルを設定する
        /// </summary>
        private void SetTitle(string source)
        {

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            //タイトルを抽出
            HtmlAgilityPack.HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//head/title");

            //設定
            Title = titleNode.InnerText;
        }


        /// <summary>
        /// ResListを設定する
        /// </summary>
        /// <param name="source">対象HTML</param>
        private void SetResList(string source)
        {
            //DLタグの間の文字列をぶっこ抜く
            Regex dlTag = new Regex("<dl class=\"thread\"[^>]*>(\n|.)*?<\\/dl>");
            Match match = dlTag.Match(source);
            if (match.Success)
            {
                // dt要素をぶっこぬいてResListに格納
                Regex dtTags = new Regex("<dt>(\n|.)*?<br><br>");
                MatchCollection tagMatchs = dtTags.Matches(source);
                int i = 0;
                foreach (Match tagMatch in tagMatchs)
                {

                    //抽出レス数までループする
                    if (i > ExtractRes)
                    {
                        break;
                    }
                    else
                    {

                        ResList.Add(new Res(tagMatch.Groups[0].Value, i));
                        i++;
                    }
                }
            }


            //ソート順序を決める
            for (int j = 0; j < ResList.Count() - 1; j++)
            {
                if (ResList[j].Anchor != -1)
                {
                    int tmpAnchor = ResList[j].Anchor;
                    //アンカーが自レス番号以上に出ない時
                    if (tmpAnchor < j)
                    {
                        int tmpSortNo = ResList[tmpAnchor].SortNo;
                        ResList[j].SortNo = tmpSortNo;
                        //アンカー数をインクリメント
                        ResList[tmpAnchor].AnchorCount++;
                    }
                    else
                    {
                        ResList[j].SortNo = j;
                    }
                }
            }


            //ソート、文字色変更、サイズ変更を行う
            ResList = ColoringResList(SortResList(ResList));

            //HTMLプロパティに値を設定する
            SetProcessHTML(ResList);

        }

        /// <summary>
        /// ソート
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Res> SortResList(List<Res> list)
        {
            List<Res> result;

            //list.Sort((a, b) => a.SortNo - b.SortNo);
            //var q = list.OrderBy(t => t.ResNo).ThenBy(t => t.SortNo);
            var q = list.OrderBy(t => t.SortNo);
            result = q.ToList<Res>();

            return result;
        }



        /// <summary>
        /// アンカー数に応じて文字サイズ、色を付加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Res> ColoringResList(List<Res> list)
        {
            List<Res> result = new List<Res>(list);
            List<Res> opeList = new List<Res>(list);

            var q = result.OrderByDescending(t => t.AnchorCount);
            opeList = q.ToList<Res>();

            int cnt = 0;

            string style = "";

            for (int i = 0; i < opeList.Count(); i++)
            {

                //レスのIDがスレ主の場合
                if (opeList[i].Id == result[0].Id)
                {
                    style = "color:gray;font-size:24px;";
                }
                else
                {
                    if (cnt < 4)
                    {
                        switch (cnt)
                        {
                            case 0:
                                cnt++;

                                style = "color:red;font-size:24px;";
                                break;
                            case 1:
                                cnt++;

                                style = "color:green;font-size:24px;";
                                break;
                            case 2:
                                cnt++;

                                style = "color:blue;font-size:24px;";
                                break;
                            case 3:
                                cnt++;

                                style = "color:orange;font-size:24px;";
                                break;
                        }

                        //ID、レスNo.が一致したitemにSTYLEを格納
                        foreach (Res item in list)
                        {
                            if (item.ResNo == opeList[i].ResNo)
                            {
                                item.Style += style;
                                break;
                            }
                            else if (item.Id == opeList[i].Id)
                            {
                                item.Style += style;
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 加工されたHTMLを設定する
        /// </summary>
        private void SetProcessHTML(List<Res> resList)
        {

            string singleRes = "";

            //レスリストを文字列として格納
            foreach (Res item in resList)
            {

                //オブジェクトが保有しているStyleからddタグ変換用のHTMLを生成する
                if (String.IsNullOrWhiteSpace(item.Style) == false)
                {
                    string replaceHtml = "<dd style=\"";
                    replaceHtml += item.Style;
                    replaceHtml += "\">";
                    string convHtml = item.Html.Replace("<dd>", replaceHtml);
                    item.Html = convHtml;
                }

                singleRes += item.Html;
            }

            //2chのGIF画像をHTMLから削除する(2chの記事中でIMGタグが存在するのは唯一投稿最初のGIFのみという前提)
            string processHtml = Regex.Replace(singleRes, "<img\\s+[^>]*src=\"([^\"]*)\"[^>]*>", "");


            //DLタグを先頭と終端に追加
            processHtml = processHtml.Insert(0, "<dl>");
            processHtml += "</dl>";

            processHtml += "<p>引用元:" + this.Url + "</p>";
            this.Html = processHtml;
        }

        /// <summary>
        /// 形態素解析を行い、名詞を返却する
        /// </summary>
        /// <returns></returns>
        public List<string> GetTags()
        {
            List<string> retVal = new List<string>(); ;
            try
            {

                MeCabParam param = new MeCabParam();
                param.DicDir = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\ipadic";

                MeCabTagger tag = MeCabTagger.Create(param);
                MeCabNode node = tag.ParseToNode(Title);

                while (node != null)
                {
                    if (node.CharType == 2)
                    {
                        retVal.Add(node.Surface);
                    }
                    node = node.Next;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

    }
}