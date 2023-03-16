﻿using System.ComponentModel;

namespace TipsAndTricks.WebApp.Areas.Admin.Models {
    public class CategoryFilterModel {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
        [DisplayName("Hiển thị trên menu")]
        public bool ShowOnMenu { get; set; }
    }
}
