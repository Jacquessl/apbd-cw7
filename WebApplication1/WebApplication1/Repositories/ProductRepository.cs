using Microsoft.Data.SqlClient;

namespace WebApplication1.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;
    
    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public bool DoesProductExist(int idProduct)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT 1 FROM Product WHERE IdProduct = @idProduct";
        command.Parameters.AddWithValue("@idProduct", idProduct);
        var reader = command.ExecuteReader();
        return reader is null;
    }
    public bool DoesWarehouseExist(int idWarehouse)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @idWarehouse";
        command.Parameters.AddWithValue("@idProduct", idWarehouse);
        var reader = command.ExecuteReader();
        return reader is null;
    }

    public bool WasProductOrdered(int idProduct, int amount)
    {
        throw new NotImplementedException();
    }

    public bool CheckIfMistake(int idOrder)
    {
        throw new NotImplementedException();
    }
}