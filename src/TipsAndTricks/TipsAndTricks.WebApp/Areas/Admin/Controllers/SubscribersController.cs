using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services.Blogs;
using TipsAndTricks.Services.FilterParams;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Areas.Admin.Controllers {
    public class SubscribersController : Controller {

        private readonly ILogger<SubscribersController> _logger;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMapper _mapper;

        public SubscribersController(ILogger<SubscribersController> logger, ISubscriberRepository subscriberRepository, IMapper mapper) {
            _logger = logger;
            _subscriberRepository = subscriberRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            SubscriberFilterModel model,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "page-size")] int pageSize = 10) {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var subscriberQuery = _mapper.Map<SubscriberQuery>(model);

            _logger.LogInformation("Lấy danh sách người theo dõi từ CSDL");

            var pagingParams = new PagingParams() {
                PageNumber = page,
                PageSize = pageSize
            };

            ViewBag.SubscribersList = await _subscriberRepository.GetPagedSubscribersByQueryAsync(subscriberQuery, pagingParams);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Ban(int id) {
            var subscriber = await _subscriberRepository.GetSubscriberByIdAsync(id);
            SubscriberBanModel model = new SubscriberBanModel() {
                Id = subscriber.Id,
                Reason = subscriber.Reason,
                Notes = subscriber.Notes,
                IsBanned = subscriber.SubscribeState == SubscribeState.Banned
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(SubscriberBanModel model) {
            var subscriber = await _subscriberRepository.GetSubscriberByIdAsync(model.Id);

            if (model.IsBanned) {
                await _subscriberRepository.BanSubscriberAsync(subscriber.Id, model.Notes);
            } else {
                await _subscriberRepository.UnbanSubscriberAsync(subscriber.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteSubscriber(int id) {
            await _subscriberRepository.DeleteSubscriberByIdAsync(id);

            return RedirectToAction("Index");
        }
    }
}
