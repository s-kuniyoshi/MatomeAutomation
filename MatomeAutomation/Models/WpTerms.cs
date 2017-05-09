using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Models
{
    public struct WpTerm
    {
        public string term_id;
        public string name;
        public string slug;
        public string term_group;
        public string term_taxonomy_id;
        public string taxonomy;
        public string description;
        public string parent;
        public int count;
    }
}