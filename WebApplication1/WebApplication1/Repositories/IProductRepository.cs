namespace WebApplication1.Repositories;

public interface IProductRepository
{
    public bool DoesProductExist(int idProduct);
    public bool DoesWarehouseExist(int idWarehouse);
    public bool WasProductOrdered(int idProduct, int amount);
    public bool CheckIfMistake(int idOrder);
    
}