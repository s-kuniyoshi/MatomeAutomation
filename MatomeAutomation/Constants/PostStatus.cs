﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MatomeAutomation.Constants
{
    /// <summary>
    /// 記事の投稿タイプを定義
    /// </summary>
    public static class PostStatus
    {
        public static readonly string Any = "any";
        public static readonly string Published = "published";
        public static readonly string Pending = "pending";
        public static readonly string Draft = "draft";
        public static readonly string AutoDraft = "autodraft";
        public static readonly string Future = "future";
        public static readonly string Private = "private";
        public static readonly string Inherit = "inherit";
        public static readonly string Trash = "trash";
    }
}