using Microsoft.Data.SqlClient;
using WebApplication1.Models.DTO_s;

namespace WebApplication1.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;
    
    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> DoesProductExist(AddProduct addProduct)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT 1 FROM Product WHERE IdProduct = @idProduct";
        command.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        await connection.OpenAsync();
        var reader = await command.ExecuteScalarAsync();
        return reader is not null;
    }
    public async Task<bool> DoesWarehouseExist(AddProduct addProduct)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @idWarehouse";
        command.Parameters.AddWithValue("@idWarehouse", addProduct.IdWarehouse);
        await connection.OpenAsync();
        var reader = await command.ExecuteScalarAsync();
        return reader is not null;
    }

    public async Task<bool> WasProductOrdered(AddProduct addProduct)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT 1 FROM \"Order\" WHERE IdProduct = @idProduct AND amount = @amount AND CreatedAt <= GETDATE()";
        command.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        command.Parameters.AddWithValue("@amount", addProduct.Amount);
        await connection.OpenAsync();
        var reader = await command.ExecuteScalarAsync();
        return reader is not null;
    }

    public async Task<bool> CheckIfMistake(AddProduct addProduct)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = (SELECT IdOrder FROM \"Order\" WHERE IdProduct = @idProduct AND amount = @amount)";
        command.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        command.Parameters.AddWithValue("@amount", addProduct.Amount);
        await connection.OpenAsync();
        var reader = await command.ExecuteScalarAsync();
        return reader is not null;
    }

    public async Task UpdateFullfilledAt(AddProduct addProduct)
    {
        var query = "UPDATE \"ORDER\" SET FulfilledAt = GETDATE() WHERE IdProduct = @idProduct AND amount = @amount";

        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        command.Parameters.AddWithValue("@amount", addProduct.Amount);
        
        await connection.OpenAsync();
        
        await command.ExecuteNonQueryAsync();
    }

    public async Task InputRecord(AddProduct addProduct)
    {
        var query = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)" +
                    "VALUES (@idWarehouse, @idProduct, (SELECT IdOrder FROM \"Order\" WHERE IdProduct = @idProduct2 AND amount = @amount2), @amount, @price, GETDATE())";

        //var query2 = "SELECT IdOrder FROM \"Order\" WHERE IdProduct = @idProduct AND amount = @amount";
        var query3 = "SELECT Price FROM Product WHERE IdProduct = @idProduct";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
       // using SqlCommand command2 = new SqlCommand();
        using SqlCommand command3 = new SqlCommand();
        await connection.OpenAsync();

        // command2.Connection = connection;
        // command2.CommandText = query2;
        // command2.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        // command2.Parameters.AddWithValue("@amount", addProduct.Amount);
        command3.Connection = connection;
        command3.CommandText = query3;
        command3.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        //var idOrder = await command2.ExecuteScalarAsync();
        var price = await command3.ExecuteScalarAsync();
        float.TryParse(price.ToString(), out float floatValue);
        //int.TryParse(idOrder.ToString(), out int idOrderInt);
        //Console.WriteLine(idOrderInt);
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        command.Parameters.AddWithValue("@idProduct2", addProduct.IdProduct);
        command.Parameters.AddWithValue("@amount", addProduct.Amount);
        command.Parameters.AddWithValue("@amount2", addProduct.Amount);
        command.Parameters.AddWithValue("@idWarehouse", addProduct.IdWarehouse);
        //command.Parameters.AddWithValue("@idOrder", idOrderInt);
        command.Parameters.AddWithValue("@price", floatValue*addProduct.Amount);
        
        
        await command.ExecuteNonQueryAsync();
    }

    public async Task<int> GetId(AddProduct addProduct)
    {
        var query = "SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdOrder = (SELECT IdOrder FROM \"Order\" WHERE IdProduct = @idProduct AND amount = @amount)";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        connection.Open();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProduct", addProduct.IdProduct);
        command.Parameters.AddWithValue("@amount", addProduct.Amount);
        var reader = command.ExecuteScalar();
        int.TryParse(reader.ToString(), out int id);
        return id;
    }
}