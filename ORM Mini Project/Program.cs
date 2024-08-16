using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.OrderDTOs;
using ORM_Mini_Project.DTOs.ProductDTOs;
using ORM_Mini_Project.DTOs.UserDTOs;
using ORM_Mini_Project.Enums;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Implementations;
using System.Xml;

UserService userService = new UserService();
OrderService orderService = new OrderService();
ProductService productService = new ProductService();
PaymentService paymentService = new PaymentService();
OrderDetailServiceL OrderDetailServiceL = new OrderDetailServiceL();

User? loggedInUser = null;

while (true)
{
    Console.WriteLine("Menyu:");
    Console.WriteLine("1. Qeydiyyat");
    Console.WriteLine("2. Daxil ol");
    Console.WriteLine("3. Servisler");
    Console.WriteLine("4. Çıxış");
    string command = Console.ReadLine();

    try
    {
        switch (command)
        {
            case "1":
                await RegisterUser(userService);
                break;
            case "2":
                loggedInUser = await LoginUser(userService);
                break;
            case "3":
                if (loggedInUser == null)
                {
                    Console.WriteLine("evvelce daxil olun.");
                    loggedInUser = await LoginUser(userService);
                    if (loggedInUser == null)
                    {
                        Console.WriteLine("Daxil olma uğursuz oldu, esas menyuya qaytarılır.");
                        break;  
                    }
                }

                Console.WriteLine("Servislere giriş uğurludur.");
                await ShowServiceMenu(orderService, productService, paymentService, OrderDetailServiceL);
                break;
            case "4":
                return;
            default:
                Console.WriteLine("Yanlış seçim, yeniden cehd edin.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Xeta baş verdi: {ex.Message}");
    }
}

static async Task RegisterUser(UserService userService)
{
    try
    {
        Console.WriteLine("Yeni istifadeçi qeydiyyatdan keçirin:");
        Console.Write("Tam ad daxil edin: ");
        var fullName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
        {
            Console.WriteLine("Tam ad en azı 3 simvoldan ibaret olmalıdır.");
            return;
        }

        Console.Write("E-poçt ünvanı daxil edin: ");
        var email = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
        {
            Console.WriteLine("Yanlış e-poçt ünvanı.");
            return;
        }

        Console.Write("Şifre daxil edin: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            Console.WriteLine("Şifre en azı 6 simvoldan ibaret olmalıdır.");
            return;
        }

        Console.Write("Ünvan daxil edin: ");
        var address = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("Ünvan daxil edilmelidir.");
            return;
        }

        var registerUserDto = new RegisterUserDto
        {
            FullName = fullName,
            Email = email,
            Password = password,
            Address = address
        };

        await userService.RegisterUserAsync(registerUserDto);
        Console.WriteLine("İstifadeçi uğurla qeydiyyatdan keçdi.");
    }
    catch (InvalidUserInformationException ex)
    {
        Console.WriteLine($"Qeydiyyat uğursuz oldu: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Xeta baş verdi: {ex.Message}");
    }
}

static async Task<User?> LoginUser(UserService userService)
{
    try
    {
        Console.WriteLine("Daxil olun:");
        Console.Write("E-poçt ünvanı daxil edin: ");
        var email = Console.ReadLine();

        Console.Write("Şifre daxil edin: ");
        var password = Console.ReadLine();

        var loginUserDto = new LoginUserDto
        {
            Email = email,
            Password = password
        };

        var user = await userService.LoginUserAsync(loginUserDto);
        Console.WriteLine($"Daxil oldunuz: {user.FullName}");
        return user;
    }
    catch (UserAuthenticationException ex)
    {
        Console.WriteLine($"Daxil olma uğursuz oldu: {ex.Message}");
        return null;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        return null;
    }
}

static async Task ShowServiceMenu(OrderService orderService, ProductService productService, PaymentService paymentService, OrderDetailServiceL OrderDetailServiceL)
{
    while (true)
    {
        Console.WriteLine("Servisler Menyusu:");
        Console.WriteLine("1. Sifarişler");
        Console.WriteLine("2. Mehsullar");
        Console.WriteLine("3. Ödenişler");
        Console.WriteLine("4. Sifariş Detalları");
        Console.WriteLine("5. Geri qayıt");

        string serviceCommand = Console.ReadLine();

        switch (serviceCommand)
        {
            case "1":
                await ManageOrders(orderService, productService);
                break;
            case "2":
                await ManageProducts(productService);
                Console.WriteLine("Mehsullar servisi.");
                break;
            case "3":
                await ManagePayments(paymentService);
                Console.WriteLine("Ödenişler servisi.");
                break;
            case "4":
               
                Console.WriteLine("Sifariş Detalları servisi.");
                break;
            case "5":
                return; 
            default:
                Console.WriteLine("Yanlış seçim, yeniden cehd edin.");
                break;
        }
    }
}

static async Task ManageOrders(OrderService orderService,ProductService productService)
{
    while (true)
    {
        Console.WriteLine("Sifarişler Menyusu:");
        Console.WriteLine("1. Sifariş yarat");
        Console.WriteLine("2. Sifarişleri siyahıya al");
        Console.WriteLine("3. Sifarişi sil");
        Console.WriteLine("4. Sifarişi tamamla");
        Console.WriteLine("5. Sifarişi leğv et");
        Console.WriteLine("6. Geri qayıt");

        string orderCommand = Console.ReadLine();

        switch (orderCommand)
        {
            case "1":
                await CreateOrder(orderService, productService);
                break;
            case "2":
                await ListOrders(orderService);
                break;
            case "3":
                await DeleteOrder(orderService);
                break;
            case "4":
                await CompleteOrder(orderService);
                break;
            case "5":
                await CancelOrder(orderService);
                break;
            case "6":
                return;
            default:
                Console.WriteLine("Yanlış seçim, yeniden cehd edin.");
                break;
        }
    }

    static async Task CreateOrder(OrderService orderService, ProductService productService)
    {
        try
        {
            Console.WriteLine("Yeni sifariş yaratmaq üçün melumatları daxil edin:");
            Console.Write("İstifadeçi ID-sini daxil edin: ");
            int userId = int.Parse(Console.ReadLine());

            var products = await productService.GetAllProductAsync();
            Console.WriteLine("Mövcud Mehsullar:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Ad: {product.Name}, Qiymet: {product.Price}, Stok: {product.Stock}");
            }

            Console.Write("Mehsul ID-sini seçin: ");
            int productId = int.Parse(Console.ReadLine());

            var selectedProduct = products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct == null)
            {
                throw new NotFoundException("Seçilen mehsul tapılmadı.");
            }

            Console.Write("Miqdarı daxil edin: ");
            int quantity = int.Parse(Console.ReadLine());

            if (quantity > selectedProduct.Stock)
            {
                throw new InvalidOrderException("Stokda kifayet qeder mehsul yoxdur.");
            }

            var order = new Order
            {
                UserId = userId,
                Id = productId,
                TotalAmount = quantity,
                OrderDate = DateTime.UtcNow
            };

            await orderService.CreateOrderAsync(userId);

            selectedProduct.Stock -= quantity;
            await productService.UpdateProductAsync(selectedProduct);

            Console.WriteLine("Sifariş uğurla yaradıldı.");
        }
        catch (InvalidOrderException ex)
        {
            Console.WriteLine($"Sifariş yaratmaq uğursuz oldu: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Xeta: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }

    static async Task ListOrders(OrderService orderService)
    {
        try
        {
            var orders = await orderService.GetOrders();
            Console.WriteLine("Sifarişler:");
            foreach (var order in orders)
            {
                Console.WriteLine($"İstifadeçi ID: {order.UserId}, Mebleğ: {order.TotalAmount}, Status: {order.Status}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sifarişleri siyahıya almaq mümkün olmadı: {ex.Message}");
        }
    }

    static async Task DeleteOrder(OrderService orderService)
    {
        try
        {

            Console.WriteLine("Sifarişi silmek üçün sifariş ID-sini daxil edin:");
            int orderId = int.Parse(Console.ReadLine());

            await orderService.DeleteOrderAsync(orderId);
            Console.WriteLine("Sifariş uğurla silindi.");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Sifarişi silmek uğursuz oldu: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }
    static async Task CompleteOrder(OrderService orderService)
    {
        try
        {
            Console.WriteLine("Sifarişi tamamlamaq üçün sifariş ID-sini daxil edin:");
            int orderId = int.Parse(Console.ReadLine());

            await orderService.CompleteOrderAsync(orderId);
            Console.WriteLine("Sifariş uğurla tamamlandı.");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Sifarişi tamamlamaq uğursuz oldu: {ex.Message}");
        }
        catch (OrderAlreadyCancelledException ex)
        {
            Console.WriteLine($"Sifarişi tamamlamaq uğursuz oldu: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }

    static async Task CancelOrder(OrderService orderService)
    {
        try
        {
            Console.WriteLine("Sifarişi leğv etmek üçün sifariş ID-sini daxil edin:");
            int orderId = int.Parse(Console.ReadLine());

            await orderService.CancelOrderAsync(orderId);
            Console.WriteLine("Sifariş uğurla leğv edildi.");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Sifarişi leğv etmek uğursuz oldu: {ex.Message}");
        }
        catch (OrderAlreadyCancelledException ex)
        {
            Console.WriteLine($"Sifarişi leğv etmek uğursuz oldu: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }
}

static async Task ManageProducts(ProductService productService)
{
    while (true)
    {
        Console.WriteLine("Mehsullar Menyusu:");
        Console.WriteLine("1. Mehsul yarat");
        Console.WriteLine("2. Mehsulları siyahıya al");
        Console.WriteLine("3. Mehsulu yenile");
        Console.WriteLine("4. Mehsulu sil");
        Console.WriteLine("5. Mehsul axtar (adı ile)");
        Console.WriteLine("6. Mehsul axtar (Id ile)");
        Console.WriteLine("7. Geri qayıt");

        string productCommand = Console.ReadLine();

        switch (productCommand)
        {
            case "1":
                await CreateProduct(productService);
                break;
            case "2":
                await ListProducts(productService);
                break;
            case "3":
                await UpdateProduct(productService);
                break;
            case "4":
                await DeleteProduct(productService);
                break;
            case "5":
                await SearchProductByName(productService);
                break;
            case "6":
                await SearchProductById(productService);
                break;
            case "7":
                return;
            default:
                Console.WriteLine("Yanlış seçim, yeniden cehd edin.");
                break;
        }
    }
    static async Task CreateProduct(ProductService productService)
    {
        try
        {
            Console.WriteLine("Yeni mehsul yaratmaq üçün melumatları daxil edin:");
            Console.Write("Mehsulun adını daxil edin: ");
            string name = Console.ReadLine();

            Console.Write("Mehsulun qiymetini daxil edin: ");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.Write("Mehsulun stok miqdarını daxil edin: ");
            int stock = int.Parse(Console.ReadLine());

            Console.Write("Mehsulun tesvirini daxil edin (boş buraxmaq olar): ");
            string description = Console.ReadLine();

            var newProduct = new ProductCreateDto
            {
                Name = name,
                Price = price,
                Stock = stock,
                Description = description
            };

            await productService.CreateProductAsync(newProduct);
            Console.WriteLine("Mehsul uğurla yaradıldı.");
        }
        catch (InvalidProductException ex)
        {
            Console.WriteLine($"Mehsul yaratmaq uğursuz oldu: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }

    static async Task ListProducts(ProductService productService)
    {
        try
        {
            var products = await productService.GetAllProductAsync();
            Console.WriteLine("Mehsullar:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Ad: {product.Name}, Qiymet: {product.Price}, Stok: {product.Stock}, Tesvir: {product.Description}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mehsulları siyahıya almaq mümkün olmadı: {ex.Message}");
        }
    }

    static async Task UpdateProduct(ProductService productService)
    {
        try
        {
            Console.WriteLine("Yenilenecek mehsulun ID-sini daxil edin:");
            int id = int.Parse(Console.ReadLine());

            Console.WriteLine("Mehsulun yeni adını daxil edin:");
            string name = Console.ReadLine();

            Console.WriteLine("Mehsulun yeni qiymetini daxil edin:");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Mehsulun yeni stok miqdarını daxil edin:");
            int stock = int.Parse(Console.ReadLine());

            Console.WriteLine("Mehsulun yeni tesvirini daxil edin (boş buraxmaq olar):");
            string description = Console.ReadLine();

            var updateProduct = new ProductUpdateDto
            {
                Id = id,
                Name = name,
                Price = price,
                Stock = stock,
                Description = description
            };

            await productService.UpdateProductAsync(updateProduct);
            Console.WriteLine("Mehsul uğurla yenilendi.");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Mehsulu yenilemek uğursuz oldu: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }

    static async Task DeleteProduct(ProductService productService)
    {
        try
        {
            Console.WriteLine("Silinecek mehsulun ID-sini daxil edin:");
            int id = int.Parse(Console.ReadLine());

            await productService.DeleteProductAsync(id);
            Console.WriteLine("Mehsul uğurla silindi.");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Mehsulu silmek uğursuz oldu: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }

    static async Task SearchProductByName(ProductService productService)
    {
        try
        {
            Console.WriteLine("Axtarmaq istediyiniz mehsulun adını daxil edin:");
            string name = Console.ReadLine();

            var products = await productService.GetProductByNameAsync(name);
            if (products.Any())
            {
                Console.WriteLine("Tapılan mehsullar:");
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}, Ad: {product.Name}, Qiymet: {product.Price}, Stok: {product.Stock}, Tesvir: {product.Description}");
                }
            }
            else
            {
                Console.WriteLine("Heç bir mehsul tapılmadı.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mehsulu axtarmaq uğursuz oldu: {ex.Message}");
        }
    }

    static async Task SearchProductById(ProductService productService)
    {
        var products = await productService.GetAllProductAsync();

        if (products.Count == 0)
        {
            Console.WriteLine("Heç bir mehsul tapılmadı.");
            return;
        }

        Console.WriteLine("Mövcud mehsullar:");
        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.Id}, Ad: {product.Name}, Qiymet: {product.Price}, Stok: {product.Stock}");
        }
        Console.WriteLine("Axtarmaq istediyiniz mehsulun ID-sini daxil edin:");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Yanlış ID formatı, yeniden cehd edin.");
            return;
        }
        try
        {
            var product = await productService.GetProductById(id);
            Console.WriteLine($"Tapılan mehsul: ID: {product.Id}, Ad: {product.Name}, Qiymet: {product.Price}, Stok: {product.Stock}, Tesvir: {product.Description}");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}
static async Task ManagePayments(PaymentService paymentService)
{
    while (true)
    {
        Console.WriteLine("Ödenişler Menyusu:");
        Console.WriteLine("1. Ödeniş et");
        Console.WriteLine("2. Bütün ödenişleri siyahıya al");
        Console.WriteLine("3. Geri qayıt");

        string paymentCommand = Console.ReadLine();

        switch (paymentCommand)
        {
            case "1":
                await MakePayment(paymentService);
                break;
            case "2":
                await ListPayments(paymentService);
                break;
            case "3":
                return; 
            default:
                Console.WriteLine("Yanlış seçim, yeniden cehd edin.");
                break;
        }
    }

    static async Task MakePayment(PaymentService paymentService)
    {
        AppDbContext _context = new AppDbContext();
        try
        {
            Console.WriteLine("Ödeniş etmek üçün melumatları daxil edin:");
            Console.Write("Sifariş ID-sini daxil edin: ");
            int orderId = int.Parse(Console.ReadLine());

            Console.Write("Ödeniş mebleğini daxil edin: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            foreach (var item in _context.Products )
            {
                if (item.Id==orderId)
                {
                    Console.WriteLine(true);
                }
            }

            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow
            };

            await paymentService.MakePayment(payment);
            Console.WriteLine("Ödeniş uğurla edildi.");
        }
        catch (InvalidPaymentException ex)
        {
            Console.WriteLine($"Ödeniş etmek uğursuz oldu: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            Console.WriteLine($"Xeta: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        }
    }

    static async Task ListPayments(PaymentService paymentService)
    {
        try
        {
            var payments = await paymentService.GetAllPayment();
            Console.WriteLine("Bütün Ödenişler:");
            foreach (var payment in payments)
            {
                Console.WriteLine($"Sifariş ID: {payment.OrderId}, Mebleğ: {payment.Amount}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ödenişleri siyahıya almaq mümkün olmadı: {ex.Message}");
        }
    }

}


