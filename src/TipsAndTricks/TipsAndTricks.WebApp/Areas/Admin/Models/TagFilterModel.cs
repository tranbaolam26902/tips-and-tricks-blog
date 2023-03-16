using System.ComponentModel;

namespace TipsAndTricks.WebApp.Areas.Admin.Models {
    public class TagFilterModel {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
    }
}
