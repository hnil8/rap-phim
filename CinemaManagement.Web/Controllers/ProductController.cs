using System;
using CinemaManagement.BLL.Services;
using CinemaManagement.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var model = new ProductCatalogViewModel
        {
            Combos = await _productService.GetAllCombosAsync(),
            Groups = await _productService.GetAllNhomWithSanPhamAsync()
        };

        return View(model);
    }
}
