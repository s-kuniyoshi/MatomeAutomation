using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class MediaItemSize
    {
        [XmlRpcMember("file")]
        public string File { get; set; }

        [XmlRpcMember("width")]
        public int Width { get; set; }

        [XmlRpcMember("height")]
        public int Height { get; set; }

        [XmlRpcMember("mime_type")]
        public string MimeType { get; set; }
    }
}