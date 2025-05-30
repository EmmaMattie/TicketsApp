using Microsoft.AspNetCore.Mvc;
using TicketsApp.Models;
using System.Linq;

namespace TicketsApp.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ShoppingCartItem>>(CartSessionKey) ?? new List<ShoppingCartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int eventId, string eventName, decimal price, int quantity)
        {
            if (quantity < 1)
            {
                TempData["Error"] = "Quantity must be at least 1.";
                return RedirectToAction("Index", "Browse");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<ShoppingCartItem>>(CartSessionKey) ?? new List<ShoppingCartItem>();

            var existingItem = cart.FirstOrDefault(i => i.EventId == eventId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new ShoppingCartItem
                {
                    EventId = eventId,
                    EventName = eventName,
                    Price = price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ShoppingCartItem>>(CartSessionKey) ?? new List<ShoppingCartItem>();

            if (!cart.Any())
            {
                TempData["Error"] = "Your cart is empty. Add items before checking out.";
                return RedirectToAction("Index");
            }

            ViewBag.Total = cart.Sum(i => i.Price * i.Quantity);
            return View(cart);
        }

        [HttpPost]
        public IActionResult ConfirmCheckout()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return View("Confirmation");
        }
    }
}
