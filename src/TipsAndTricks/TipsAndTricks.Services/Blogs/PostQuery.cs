﻿using TipsAndTricks.Core.Contracts;

namespace TipsAndTricks.Services {
    public class PostQuery : IPostQuery {
        public int AuthorId { get; set; } = -1;
        public int CategoryId { get; set; } = -1;
        public string CategorySlug { get; set; } = "";
        public string AuthorSlug { get; set; } = "";
        public string TagSlug { get; set; } = "";
        public int PostedYear { get; set; } = -1;
        public int PostedMonth { get; set; } = -1;
        public bool Published { get; set; }
        public bool Unpublished { get; set; }
        public string Keyword { get; set; } = "";
    }
}
