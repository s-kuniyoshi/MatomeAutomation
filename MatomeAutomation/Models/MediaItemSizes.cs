using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class MediaItemSizes
    {
        [XmlRpcMember("medium")]
        public MediaItemSize Medium { get; set; }

        [XmlRpcMember("large")]
        public MediaItemSize Large { get; set; }

        [XmlRpcMember("thumbnail")]
        public MediaItemSize Thumbnail { get; set; }

        [XmlRpcMember("post-thumbnail")]
        public MediaItemSize PostThumbnail { get; set; }

        [XmlRpcMember("listing")]
        public MediaItemSize Listing { get; set; }

        [XmlRpcMember("listing_small")]
        public MediaItemSize ListingSmall { get; set; }
    }
}