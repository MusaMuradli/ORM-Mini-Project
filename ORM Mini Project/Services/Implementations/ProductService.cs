using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.ProductDTOs;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;

namespace ORM_Mini_Project.Services.Implementations
{

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService()
        {
            _context = new AppDbContext();
        }
        public async Task CreateProductAsync(ProductCreateDto newProduct)
        {
            if (string.IsNullOrWhiteSpace(newProduct.Name))
            {
                throw new InvalidProductException("Mehsulun adı boş ola bilmez.");
            }
            if (newProduct.Price <= 0)
            {
                throw new InvalidProductException("Mehsulun qiymeti 0-dan böyük olmalıdır.");
            }
            if (newProduct.Stock < 0)
            {
                throw new InvalidProductException("Mehsulun stok miqdarı menfi ola bilmez.");
            }
            if (!string.IsNullOrWhiteSpace(newProduct.Description) && newProduct.Description.Length < 10)
            {
                throw new InvalidProductException("Mehsulun tesviri en azı 10 simvol uzunluğunda olmalıdır.");
            }


            Product product = new Product
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                Description = newProduct.Description,
                Stock = newProduct.Stock,
                CreatedDate= DateTime.UtcNow,
                UpdatedDate= DateTime.UtcNow
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x=>x.Id==id);
            if (product == null)
            {
                throw new NotFoundException($"Id-si {id} olan məhsul tapılmadı.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ProductGetDTO>> GetAllProductAsync()
        {
            try
            {
                var products = await _context.Products.Select(product => new ProductGetDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    Stock = product.Stock
                })
                    .ToListAsync();
                if (products == null || !products.Any())
                {
                    throw new NotFoundException("Heç bir mehsul tapılmadı.");
                }
                return products;
            }
            catch (Exception ex)
            {

                throw new Exception("Mehsullar elde edilerken bir xeta baş verdi.", ex);
            }
        }
        public async Task<ProductGetDTO> GetProductById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product is null)
                throw new NotFoundException($"Bu {id} id-li mehsul tapilmadi");

            ProductGetDTO dto = new()
            {Id=product.Id,
                 Name= product.Name,
                 Price= product.Price,
                 Description= product.Description,
                 Stock = product.Stock
            };

            return dto;
        }
        public async Task UpdateProductAsync(ProductUpdateDto  productUpdateDto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productUpdateDto.Id);
            if (product == null)
            {
                throw new NotFoundException($"ID-si {productUpdateDto.Id} olan mehsul tapılmadı.");
            }
            product.Name = productUpdateDto.Name;
            product.Price = productUpdateDto.Price;
            product.Stock = productUpdateDto.Stock;
            product.Description = productUpdateDto.Description;
            product.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

        }
        public async Task<List<ProductGetDTO>> GetProductByNameAsync(string name)
        {
            var products = await _context.Products
                .Where(x => x.Name.Contains(name.ToLower()))
                .ToListAsync();

            List<ProductGetDTO> dtos = new List<ProductGetDTO>();

            foreach (var product in products)
            {
                ProductGetDTO dto = new ProductGetDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    Description = product.Description
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        internal async Task UpdateProductAsync(ProductGetDTO selectedProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == selectedProduct.Id);
            if (product == null)
            {
                throw new NotFoundException($"ID-si {selectedProduct.Id} olan mehsul tapılmadı.");
            }
            product.Name = selectedProduct.Name;
            product.Price = selectedProduct.Price;
            product.Stock = selectedProduct.Stock;
            product.Description = selectedProduct.Description;
            product.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

        }
    }
}
