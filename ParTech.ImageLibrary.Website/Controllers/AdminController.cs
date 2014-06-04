using System.Web.Mvc;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.ViewModels.Byer;

namespace ParTech.ImageLibrary.Website.Controllers
{
    public class AdminController : Controller
    {
        public ILogger Logger { get; set; }

        private readonly IImageRepository _imageRepository;

        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserRepository _userRepository;

        public AdminController(IImageRepository imageRepository, IObjectRepository objectRepository, 
            IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _imageRepository = imageRepository;
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        //
        // GET: /Admin/ShowInactiveUsers

        [Authorize(Roles = "Admin")]
        public ActionResult ShowInactiveUsers()
        {
            if (TempData != null && TempData["Message"] != null)
            {
                switch ((MessageIdEnum)TempData["Message"])
                {
                    default:
                        TempData["StatusMessage"] = string.Empty;
                        TempData["StatusMessageClass"] = string.Empty;
                        break;
                }
            }

            var model = _userRepository.GetUserProfiles(false);

            return View(model);
        }
    }
}
