using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductAPI.DTOs;
using VShop.ProductAPI.Services;
using VShop.Web.Roles;

namespace VShop.ProductAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Get uma lista de todas categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categoriesDto = await _categoryService.GetCategories();

        if(categoriesDto == null)
            return NotFound("Categories not found");

        return Ok(categoriesDto);
    }

    // Get uma lista de todas categorias e produtos
    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
    {
        var categoriesDto = await _categoryService.GetCategoriesProducts();

        if (categoriesDto == null)
            return NotFound("Categories not found");

        return Ok(categoriesDto);
    }

    // Get uma categoria pelo id
    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id);

        if (categoryDto == null)
            return NotFound("Category not found");

        return Ok(categoryDto);
    }

    // Post(posta/cria) uma categoria
    [HttpPost]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Post([FromBody] CategoryDTO categoryDto)
    {
        if (categoryDto == null)
            return BadRequest("Invalid Data");

        await _categoryService.AddCategory(categoryDto);

        return new CreatedAtRouteResult("GetCategory", new {id = categoryDto.CategoryId}, 
            categoryDto);
    }

    // Put(coloca/atribui/atualiza) uma Categoria
    [HttpPut("{id:int}")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Put(int id, [FromBody] CategoryDTO categoryDto)
    {
        if(id != categoryDto.CategoryId)
            return BadRequest("Data invalid");

        if (categoryDto == null)
            return BadRequest("Data invalid");

        await _categoryService.UpdateCategory(categoryDto);

        return Ok(categoryDto);
    }

    // Delete uma Categoria
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Delete(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id);

        if (categoryDto == null)
            return NotFound("Category not found");

        await _categoryService.DeleteCategory(id);

        return Ok(categoryDto);
    }
}
