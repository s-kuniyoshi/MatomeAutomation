using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    public class MediaItem
    {
        [XmlRpcMember("attachment_id")]
        public string Id { get; set; }

        [XmlRpcMember("date_created_gmt")]
        public DateTime DateCreated { get; set; }

        [XmlRpcMember("parent")]
        public int Parent { get; set; }

        [XmlRpcMember("link")]
        public string Link { get; set; }

        [XmlRpcMember("title")]
        public string Title { get; set; }

        [XmlRpcMember("caption")]
        public string Caption { get; set; }

        [XmlRpcMember("description")]
        public string Description { get; set; }

        [XmlRpcMember("metadata")]
        public MediaItemMetadata Metadata { get; set; }

        [XmlRpcMember("thumbnail")]
        public string Thumbnail { get; set; }
    }
}