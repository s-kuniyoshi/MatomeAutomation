using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class MediaItemMetadata
    {
        [XmlRpcMember("width")]
        public int Width { get; set; }

        [XmlRpcMember("height")]
        public int Height { get; set; }

        [XmlRpcMember("file")]
        public string File { get; set; }

        [XmlRpcMember("sizes")]
        public MediaItemSizes Sizes { get; set; }
    }
}