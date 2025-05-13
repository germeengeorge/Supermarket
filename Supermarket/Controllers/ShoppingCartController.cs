using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Supermarket.Data.IRepository;
using Supermarket.Models;
using Supermarket.RolesAndEmailSender;
using Supermarket.ViewModel;

namespace Supermarket.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.Carts.GetShoppingCartListByUserId(userId),
                OrderHeader = new()
            };


            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (item.Product.price * item.quantity);
                item.price = item.Product.price;
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.Carts.GetShoppingCartListByUserId(userId),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.Phone = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAdress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
 

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (item.Product.price * item.quantity);
                item.price = item.Product.price;
            }
            return View(ShoppingCartVM);
        }


        [HttpPost]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.Carts.GetShoppingCartListByUserId(userId);

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;


            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            //shoppingCartvm.OrderHeader.ApplicationUser.Name = shoppingCartvm.OrderHeader.Name;
            //shoppingCartvm.OrderHeader.ApplicationUser.PhoneNumber = shoppingCartvm.OrderHeader.PhoneNumber;
            //shoppingCartvm.OrderHeader.ApplicationUser.StreetAdress = shoppingCartvm.OrderHeader.StreetAddress;
            //shoppingCartvm.OrderHeader.ApplicationUser.City = shoppingCartvm.OrderHeader.City;
            //shoppingCartvm.OrderHeader.ApplicationUser. = shoppingCartvm.OrderHeader.;



            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (item.Product.price * item.quantity);
                item.price = item.Product.price;
            }


            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.productId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.price,
                    Count = cart.quantity
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            var domain = "https://localhost:44311/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"ShoppingCart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"ShoppingCart/Index",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };


            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.quantity
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

            //this is an order by customer

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
            }
            //HttpContext.Session.Clear();

            List<ShoppingCart> shoppingCarts = _unitOfWork.Carts
                .GetShoppingCartListByUserId(orderHeader.ApplicationUserId).ToList();

            _unitOfWork.Carts.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }


        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.Carts.Get(u => u.Id == cartId);
            cartFromDb.quantity += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.Carts.Get(u => u.Id == cartId);
            if (cartFromDb.quantity <= 1)
            {
                //remove that from cart

                _unitOfWork.Carts.Remove(cartFromDb);
                //HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                //    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            }
            else
            {
                cartFromDb.quantity -= 1;
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.Carts.Get(u => u.Id == cartId);

            _unitOfWork.Carts.Remove(cartFromDb);

            //HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
            //  .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


    }
}