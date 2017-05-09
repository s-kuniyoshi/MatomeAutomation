﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace MatomeAutomation.Models
{
    /// <summary>
    /// レスの構造体
    /// </summary>
    public class Res
    {
        public string Html { get; set; }
        public int Anchor { get; set; }
        public int ResNo { get; set; }
        public int SortNo { get; set; }
        public int AnchorCount { get; set; }
        public string Style { get; set; }
        public string Id { get; set; }
        public string NameStyle { get; set; }


        public Res(string html, int resNo)
        {
            this.Html = html;
            this.ResNo = resNo;
            this.SortNo = resNo;
            this.NameStyle = "color: green; font-weight: bold;";

            //名前や時刻などに余計なゴミが付いている可能性があるためそれらを除去する
            //Regex infoReg = new Regex("<b>(.*?)</b>");
            //Match match = infoReg.Match(this.Html);
            //if (match.Success)
            //{
            //    Regex replaceReg = new Regex("：(.*?)：");
            //    string placeHolder = string.Format("：<span style=\"{0}\">{1}</span>：", this.NameStyle, match.Groups[0].Value);
            //    string tmpHtml = replaceReg.Replace(this.Html, placeHolder);
            //    this.Html = tmpHtml;
            //}


            Regex infoReg = new Regex("：(.*?)：");
            Match match = infoReg.Match(this.Html);
            if (match.Success)
            {
                Regex replaceReg = new Regex("<[^>]*>");
                string result = replaceReg.Replace(match.Value, "");

                Regex addFontTag = new Regex("：(.*?)：");
                string placeHolder = string.Format("<span style=\"{0}\">{1}</span>", this.NameStyle, result);
                string tmpHtml = addFontTag.Replace(this.Html, placeHolder);
                this.Html = tmpHtml;

            }




            //2chIDを取得
            Regex idReg = new Regex("ID:(.*)<dd>");
            Match idMatch = idReg.Match(this.Html);
            if (idMatch.Success)
            {
                this.Id = idMatch.Value;
            }



            //アンカーがあればアンカー先をAnchorとSortNoに格納し、無ければAnchorに-1を格納し、resNoをSortNoに格納
            //パターンを指定
            Regex reg = new Regex("&gt;&gt;" + @"\d{1,3}");
            //マッチ判定した値の情報を格納
            Match m = reg.Match(html);
            if (m.Success)
            {
                //マッチしていた場合(アンカー有り)
                string macthWord = m.Value;
                Anchor = int.Parse(macthWord.Replace("&gt;&gt;", "")) - 1;
                this.SortNo = Anchor;

                //HTMLを編集してレスに対するレスであることを明確する
                this.Style = "background: #F5F5F5;";
                this.Style += "border: 1px solid #CCCCCC;";
                this.Style += "margin: 5px 0 0 20px;";
                this.Style += "padding: 10px;";


            }
            else
            {
                //マッチしていない場合
                this.SortNo = resNo;
                Anchor = -1;
            }
        }
    }
}