using WebApplication1.Models.DTO_s;

namespace WebApplication1.Repositories;

public interface IProductRepository
{
    Task<bool> DoesProductExist(AddProduct addProduct);
    Task<bool> DoesWarehouseExist(AddProduct addProduct);
    Task<bool> WasProductOrdered(AddProduct addProduct);
    Task<bool> CheckIfMistake(AddProduct addProduct);
    Task UpdateFullfilledAt(AddProduct addProduct);

    Task InputRecord(AddProduct addProduct);
    Task<int> GetId(AddProduct addProduct);
    Task<int> AddProductProcedure(AddProduct addProduct);
    Task<String> AddProductException(AddProduct addProduct);
}