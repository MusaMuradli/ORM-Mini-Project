using ORM_Mini_Project.DTOs.ProductDTOs;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Services.Interfaces;

public interface  IProductService
{
    Task<List<ProductGetDTO>> GetAllProductAsync();
    Task <ProductGetDTO> GetProductById(int id);
    Task CreateProductAsync(ProductCreateDto newProduct);
    Task DeleteProductAsync(int id);
    Task UpdateProductAsync(ProductUpdateDto productUpdateDto);
    Task<List<ProductGetDTO>> GetProductByNameAsync(string name);
}
