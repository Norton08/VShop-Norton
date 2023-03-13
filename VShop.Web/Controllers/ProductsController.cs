using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Roles;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

[Authorize (Roles = Role.Admin)]
public class ProductsController : Controller
{
    private readonly IProductService _productService;

    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService,
                              ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var result = await _productService.GetAllProducts(await GetAcessToken());

        if (result == null)
            return View("Error");

        return View(result);
    }

    

    [HttpGet]
    public async Task<ActionResult> CreateProduct()
    {
        ViewBag.CategoryId = new SelectList(await 
            _categoryService.GetAllCategories(await GetAcessToken()), "CategoryId", "Name");

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.CreateProduct(model, await GetAcessToken());

            if(result != null)
                return RedirectToAction(nameof(Index));
        }
        else 
        {
            ViewBag.CategoryId = new SelectList(await
                                            _categoryService.GetAllCategories(await GetAcessToken()), "CategoryId", "Name");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(await
                                            _categoryService.GetAllCategories(await GetAcessToken()), "CategoryId", "Name");

        var result = await _productService.FindProductById(id, await GetAcessToken());

        if(result == null) return View("Error");

        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateProduct(ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(model, await GetAcessToken());

            if (result != null)
                return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
    {
        var result = await _productService.FindProductById(id, await GetAcessToken());

            if (result == null)
                return View("Error");

        return View(result);
    }

    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        var result = await _productService.DeleteProductById(id, await GetAcessToken());
        
            if (!result)
                return View("Error");

        return RedirectToAction("Index");
    }

    private async Task<string> GetAcessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

}
