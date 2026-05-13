using DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Features.Product;

[Authorize]
public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Permission("Product.View")]
    public IActionResult Index([FromQuery] ProductListRequest request)
    {
        var response = _productService.GetProducts(request);
        return View(response);
    }

    [HttpGet]
    [Permission("Product.Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Permission("Product.Create")]
    public IActionResult Create(ProductCreateRequest request)
    {
        _productService.CreateProduct(request);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Permission("Product.Update")]
    public IActionResult Edit(int id)
    {
        var products = _productService.GetProducts(new ProductListRequest { PageSize = 1000 });
        var product = products.Products.FirstOrDefault(x => x.Id == id);
        if (product == null) return NotFound();

        var request = new ProductUpdateRequest
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description
        };
        return View(request);
    }

    [HttpPost]
    [Permission("Product.Update")]
    public IActionResult Edit(int id, ProductUpdateRequest request)
    {
        _productService.UpdateProduct(id, request);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Permission("Product.Delete")]
    public IActionResult Delete(int id)
    {
        _productService.DeleteProduct(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminOnly()
    {
        return View(new AdminOnlyResponse { Message = "Only users with Admin role can access this page." });
    }
}
