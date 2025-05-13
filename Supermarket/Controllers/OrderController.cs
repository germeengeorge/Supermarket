using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermarket.Data.IRepository;
using Supermarket.Models;
using Supermarket.RolesAndEmailSender;
using Supermarket.ViewModel;

namespace Supermarket.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<OrderHeader> orders = _unitOfWork.OrderHeader.GetAll().ToList();
            return View(orders);
        }
    }
}