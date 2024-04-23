using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.DTO_s;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api")]
public class WarehouseController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public WarehouseController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    [HttpPost]
    public IActionResult PostProduct(AddProduct addProduct)
    {
        if (!_productRepository.DoesProductExist(addProduct.IdProduct)
            || !_productRepository.DoesWarehouseExist(addProduct.IdWarehouse))
        {
            return NotFound();
        }
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _productRepository.RemoveProcedures();
                    await _productRepository.
                }
            }
        return Ok();
    }
}