using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TipsAndTricks.WebApp.Areas.Admin.Models {
    public class AuthorFilterModel {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
        [DisplayName("Tháng tham gia")]
        public int? JoinedMonth { get; set; }
        [DisplayName("Năm tham gia")]
        public int? JoinedYear { get; set; }

        public IEnumerable<SelectListItem> MonthList { get; set; }
        public AuthorFilterModel() {
            MonthList = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem() {
                    Value = m.ToString(),
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
                })
                .ToList();
        }
    }
}
