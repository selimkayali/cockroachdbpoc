using CockroachDb.Data;
using CockroachDb.Models;
using Microsoft.EntityFrameworkCore;

namespace CockroachDbPoc.Services;

public class ProductService(DatabaseContext context)
{
    public async Task Create(Product product)
    {
        try
        {
            if (product.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating product: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Product>> GetAll()
    {
        try
        {
            return await context.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving products: {ex.Message}");
            return [];
        }
    }

    public async Task<Product?> GetById(long id)  // Changed from int to long
    {
        try
        {
            return await context.Products.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving product: {ex.Message}");
            return null;
        }
    }

    public async Task Update(Product product)
    {
        try
        {
            if (product.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product: {ex.Message}");
        }
    }

    public async Task<bool> Delete(long id)  // Changed from int to long
    {
        try
        {
            var product = await context.Products.FindAsync(id);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product: {ex.Message}");
            return false;
        }
    }
}