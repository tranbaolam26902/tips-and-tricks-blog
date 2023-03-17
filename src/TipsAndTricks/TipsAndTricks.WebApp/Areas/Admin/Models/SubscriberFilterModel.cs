using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;
using TipsAndTricks.Core.Entities;

namespace TipsAndTricks.WebApp.Areas.Admin.Models {
    public class SubscriberFilterModel {
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }

        [DisplayName("Trạng thái")]
        public SubscribeState? SubscribeState { get; set; }

        [DisplayName("Tháng đăng ký")]
        public int? LastSubscribedMonth { get; set; }

        [DisplayName("Năm đăng ký")]
        public int? LastSubscribedYear { get; set; }

        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> SubscribeStateList { get; set; }

        public SubscriberFilterModel() {
            MonthList = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem() {
                    Value = m.ToString(),
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
                })
                .ToList();
            var subscribe = new SelectListItem() {
                Value = "0",
                Text = "Theo dõi",
            };
            var unsubscribe = new SelectListItem() {
                Value = "1",
                Text = "Huỷ theo dõi",
            };
            var banned = new SelectListItem() {
                Value = "2",
                Text = "Bị chặn",
            };
            IList<SelectListItem> states = new List<SelectListItem>() {
                subscribe, unsubscribe, banned
            };
            SubscribeStateList = states;
        }
    }
}
