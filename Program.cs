using CockroachDb.Data;
using CockroachDb.Models;
using CockroachDbPoc.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

try
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
    optionsBuilder.UseNpgsql(connectionString);

    using var dbContext = new DatabaseContext(optionsBuilder.Options);
    var productService = new ProductService(dbContext);

    while (true)
    {
        Console.WriteLine("\n1. Create Product");
        Console.WriteLine("2. Read All Products");
        Console.WriteLine("3. Read Product by ID");
        Console.WriteLine("4. Update Product");
        Console.WriteLine("5. Delete Product");
        Console.WriteLine("6. Exit");
        Console.Write("\nSelect an option: ");

        var choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    Console.Write("Enter product name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter product price: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    {
                        var product = new Product { Name = name!, Price = price };
                        await productService.Create(product);
                        Console.WriteLine("Product created successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid price format!");
                    }
                    break;

                case "2":
                    var products = await productService.GetAll();
                    if (products.Any())
                    {
                        foreach (var p in products)
                        {
                            Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Price: {p.Price:C}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found.");
                    }
                    break;

                // In the switch case for reading by ID:
                case "3":
                    Console.Write("Enter product ID: ");
                    if (long.TryParse(Console.ReadLine(), out long id))  // Changed from int to long
                    {
                        var foundProduct = await productService.GetById(id);
                        if (foundProduct != null)
                        {
                            Console.WriteLine($"ID: {foundProduct.Id}, Name: {foundProduct.Name}, Price: {foundProduct.Price:C}");
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format!");
                    }
                    break;

                // In the switch case for updating:
                case "4":
                    Console.Write("Enter product ID to update: ");
                    if (long.TryParse(Console.ReadLine(), out long updateId))  // Changed from int to long
                    {
                        var foundProduct = await productService.GetById(updateId);
                        if (foundProduct != null)
                        {
                            Console.WriteLine($"ID: {foundProduct.Id}, Name: {foundProduct.Name}, Price: {foundProduct.Price:C}");
                            Console.Write("Enter new product name: ");
                            var newName = Console.ReadLine();
                            Console.Write("Enter new product price: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                            {
                                foundProduct.Name = newName ?? foundProduct.Name;
                                foundProduct.Price = newPrice;
                                productService.Update(foundProduct);
                                Console.WriteLine("Product updated successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Invalid price format!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                    }
                    break;

                // In the switch case for deleting:
                case "5":
                    Console.Write("Enter product ID to delete: ");
                    if (long.TryParse(Console.ReadLine(), out long deleteId))  // Changed from int to long
                    {
                        productService.Delete(deleteId);
                    }
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Application error: {ex.Message}");
}