﻿using TipsAndTricks.WebApi.Models.Authors;
using TipsAndTricks.WebApi.Models.Categories;
using TipsAndTricks.WebApi.Models.Tags;

namespace TipsAndTricks.WebApi.Models.Posts {
    public class PostDetail {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Meta { get; set; }
        public string ImageUrl { get; set; }
        public int ViewCount { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CategoryDTO Category { get; set; }
        public AuthorDTO Author { get; set; }
        public IList<TagDTO> Tags { get; set; }
    }
}
