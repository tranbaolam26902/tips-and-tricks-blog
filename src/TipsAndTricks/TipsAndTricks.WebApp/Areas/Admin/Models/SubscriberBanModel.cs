using System.ComponentModel;

namespace TipsAndTricks.WebApp.Areas.Admin.Models {
    public class SubscriberBanModel {
        public int Id { get; set; }

        [DisplayName("Lý do")]
        public string? Reason { get; set; }

        [DisplayName("Ghi chú")]
        public string? Notes { get; set; }

        [DisplayName("Chặn")]
        public bool IsBanned { get; set; }
    }
}
