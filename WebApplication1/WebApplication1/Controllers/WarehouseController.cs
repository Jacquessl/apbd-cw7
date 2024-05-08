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
    public async Task<IActionResult> PostProduct(AddProduct addProduct)
    {
        if (!await _productRepository.DoesProductExist(addProduct)
            || !await _productRepository.DoesWarehouseExist(addProduct))
            return NotFound("Product or Warehouse does not exist");
        if (!await _productRepository.WasProductOrdered(addProduct))
        {
            return NotFound("Product Was Not Ordered");
        }

        if (await _productRepository.CheckIfMistake(addProduct))
        {
            return Conflict("There's an order with this order ID");
        }
        try
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _productRepository.UpdateFullfilledAt(addProduct);
                await _productRepository.InputRecord(addProduct);
                
                scope.Complete();
            }
        }
        catch (TransactionAbortedException ex)
        {
            Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            throw;
        }
        return Ok(await _productRepository.GetId(addProduct));
    }
}