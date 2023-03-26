using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger,
            IProductService productService,
            ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllProducts(string.Empty);

            if (result == null)
                return View("Error");

            return View(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProductViewModel>> ProductDetails(int id) 
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var product = await _productService.FindProductById(id, token);

            if (product == null) return View("Error");

            return View(product);
        }

        [HttpPost]
        [ActionName("ProductDetails")]
        [Authorize]
        public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(ProductViewModel productVM)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            CartViewModel cart = new() 
            { 
                CartHeader = new CartHeaderViewModel
                {
                    UserId = User.Claims.Where(p => p.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartItemViewModel cartItem = new()
            {
                Quantity = productVM.Quantity,
                ProductId = productVM.Id,
                Product = await _productService.FindProductById(productVM.Id, token)
            };

            List<CartItemViewModel> cartItemVM = new List<CartItemViewModel>();
            cartItemVM.Add(cartItem);
            cart.CartItems = cartItemVM;

            var result = await _cartService.AddItemToCartAsync(cart, token);

            if(result != null)
                return RedirectToAction(nameof(Index));

            return View(productVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            var acessToken = await HttpContext.GetTokenAsync("oidc", "access_token");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}