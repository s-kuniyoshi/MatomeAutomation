using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace MatomeAutomation.Models
{
    public class MyProcessThread
    {

        [Required]
        [Display(Name = "2chURL")]
        public string Html { get; set; }

        [Required]
        [Display(Name = "抽出レス数")]
        public int ExtractRes { get; set; }

        public List<Res> ResList { get; set; }
        public byte Thumbnail { get; set; }
        public string Subject { get; set; }
        public string url { get; set; }

        public MyProcessThread()
        {
            ResList = new List<Res>();
        }


        /// <summary>
        /// レスを取得し、加工後、string型レスリストを返却
        /// </summary>
        /// <param name="url">取得対象URL</param>
        /// <returns>レスリスト</returns>
        public string GetResList(string url)
        {
            //加工結果
            string result = "";
            //取得したHTMLソース
            string soruce = "";
            //レスを一つ格納
            string line = "";
            //生のレスリスト
            string filtrationSoruce = "";

            WebClient client = new WebClient();
            //HTMLコードを取得
            soruce = client.DownloadString(url);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(soruce);
            //dlタグ内の要素を格納
            HtmlAgilityPack.HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//dl");

            //変数内の要素を取得※各ddの閉じタグが無いから自動で最終行に要素数分付加される
            foreach (HtmlAgilityPack.HtmlNode node in nodes)
            {
                filtrationSoruce += node.InnerHtml;
            }


            //1行ずつ抽出し、レスオブジェクトに格納しListに格納
            System.IO.StringReader sr = new System.IO.StringReader(filtrationSoruce);
            line = sr.ReadLine();
            //最初の1つは空文字なので読み次の行を読み込む
            if (line == "")
            {
                line = sr.ReadLine();
            }
            int i = 0;

            while (line != null)
            {
                if (line != null)
                {
                    //元のソースのddタグに閉じタグが無いので付加
                    line += "</dd>";
                    ResList.Add(new Res(line, i));
                    line = sr.ReadLine();
                    i++;
                }
            }

            ResList.RemoveAt(ResList.Count() - 1);

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


            string singleRes = "";

            //レスリストを文字列として格納
            foreach (Res item in ResList)
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
            result = singleRes;


            return result;
        }

        /// <summary>
        /// ソート
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Res> SortResList(List<Res> list)
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
        public List<Res> ColoringResList(List<Res> list)
        {
            List<Res> result = new List<Res>(list);
            List<Res> opeList = new List<Res>(list);

            var q = result.OrderByDescending(t => t.AnchorCount);
            opeList = q.ToList<Res>();

            //スレ主の文字を加工


            for (int i = 1; i < opeList.Count(); i++)
            {
                string style = "";
                if (i < 5)
                {
                    switch (i)
                    {
                        case 1:
                            style = "color:red;font-size:24px;";
                            break;
                        case 2:
                            style = "color:green;font-size:24px;";
                            break;
                        case 3:
                            style = "color:blue;font-size:24px;";
                            break;
                        case 4:
                            style = "color:orange;font-size:24px;";
                            break;
                    }

                    foreach (Res item in list)
                    {
                        if (item.ResNo == opeList[i].ResNo)
                        {
                            item.Style += style;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}