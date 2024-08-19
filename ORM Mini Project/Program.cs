using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.OrderDetailDTOs;
using ORM_Mini_Project.DTOs.OrderDTOs;
using ORM_Mini_Project.DTOs.ProductDTOs;
using ORM_Mini_Project.DTOs.UserDTOs;
using ORM_Mini_Project.Enums;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Implementations;
using ORM_Mini_Project.Services.Interfaces;
using System.Xml;


UserService userService = new UserService();
OrderService orderService = new OrderService();
ProductService productService = new ProductService();
PaymentService paymentService = new PaymentService();
OrderDetailService OrderDetailServiceL = new OrderDetailService();

User? loggedInUser = null;

while (true)
{
    Console.WriteLine("Menyu:");
    Console.WriteLine("1. Qeydiyyat");
    Console.WriteLine("2. Daxil ol");
    Console.WriteLine("3. Servisler");
    Console.WriteLine("4. cıxış");
    string command = Console.ReadLine();

    try
    {
        switch (command)
        {
            case "1":
                await RegisterUser(userService);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("İstifadeci qeydiyyatı uğurlu oldu.");
                Console.ResetColor();
                break;
            case "2":
                loggedInUser = await LoginUser(userService);
                if (loggedInUser != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Daxilolma uğurlu oldu.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Daxilolma uğursuz oldu.");
                    Console.ResetColor();
                }
                break;
            case "3":
                if (loggedInUser == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("evvelce daxil olun.");
                    Console.ResetColor();

                    loggedInUser = await LoginUser(userService);

                
                    if (loggedInUser == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Daxil olma uğursuz oldu, esas menyuya qaytarılır.");
                        Console.ResetColor();
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Daxilolma uğurlu oldu.");
                        Console.ResetColor();
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Servislere giriş uğurludur.");
                Console.ResetColor();

                await ShowServiceMenu(orderService, productService, paymentService, OrderDetailServiceL,loggedInUser);
                break;
            case "4":
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Yanlış secim, yeniden cehd edin.");
                Console.ResetColor();
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        Console.ResetColor();
    }

}

static async Task RegisterUser(UserService userService)
{
    try
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Yeni istifadeci qeydiyyatdan kecirin:");
        Console.ResetColor();

    FirstName:
        Console.Write("Tam ad daxil edin: ");
        var fullName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Tam ad en azı 3 simvoldan ibaret olmalıdır.");
            Console.ResetColor();
            goto FirstName;
        }

        fullName = char.ToUpper(fullName[0]) + fullName.Substring(1);

        var existingUser = await userService.GetUserByFullNameAsync(fullName);
        if (existingUser != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bu adla istifadeci artıq mövcuddur.");
            Console.ResetColor();
            goto FirstName;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
    FirstEmail:
        Console.Write("E-poct ünvanı daxil edin: ");
        var email = Console.ReadLine();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Yanlış e-poct ünvanı,e-poct ünvanı boş ola bilmez ve @ işaresi olmalıdır");
            Console.ResetColor();
            goto FirstEmail;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
    FirstPassword:
        Console.Write("Şifre daxil edin: ");
        var password = Console.ReadLine();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Şifre en azı 6 simvoldan ibaret olmalıdır.");
            Console.ResetColor();
            goto FirstPassword;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        FirstMap:
        Console.Write("Ünvan daxil edin: ");
        var address = Console.ReadLine();
        Console.ResetColor();
        if (string.IsNullOrWhiteSpace(address))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ünvan daxil edilmelidir.");
            Console.ResetColor();
            goto FirstMap;
        }

        var registerUserDto = new RegisterUserDto
        {
            FullName = fullName,
            Email = email,
            Password = password,
            Address = address
        };

        await userService.RegisterUserAsync(registerUserDto);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("İstifadeci uğurla qeydiyyatdan kecdi.");
        Console.ResetColor();
    }
    catch (InvalidUserInformationException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Qeydiyyat uğursuz oldu: {ex.Message}");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Xeta baş verdi: {ex.Message}");
        Console.ResetColor();
    }
}


static async Task<User?> LoginUser(UserService userService)
{
    try
    {
        Console.WriteLine("Daxil olun:");
        Console.Write("E-poct ünvanı daxil edin: ");
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

static async Task ShowServiceMenu(OrderService orderService, ProductService productService, PaymentService paymentService, OrderDetailService orderDetailService,User LogedUser)
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
                await ManageOrders(orderService, productService,LogedUser);
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
                await ManageOrderDetails(orderDetailService, orderService);
                Console.WriteLine("Sifariş Detalları servisi.");
                break;
            case "5":
                return;
            default:
                Console.WriteLine("Yanlış secim, yeniden cehd edin.");
                break;
        }
    }
}

static async Task ManageOrders(OrderService orderService, ProductService productService,User LogedUser)
{
    while (true)
    {
        Console.WriteLine("Sifarişler Menyusu:");
        Console.WriteLine("1. Sifariş yarat");
        Console.WriteLine("2. Sifarişleri siyahıya al");
        Console.WriteLine("3. Sifarişi sil");
        Console.WriteLine("4. Sifarişi tamamla");
        Console.WriteLine("5. Sifarişi leğv et");
        Console.WriteLine("7.Add OrderDetail");
        Console.WriteLine("6. Geri qayıt");

        string orderCommand = Console.ReadLine();

        switch (orderCommand)
        {
            case "1":
                await CreateOrder(orderService, productService,LogedUser);
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
                Console.WriteLine("Yanlış secim, yeniden cehd edin.");
                break;
        }
    }

    static async Task CreateOrder(OrderService orderService, ProductService productService,User LogedUser)
    {
        try
        {
            Console.WriteLine("Yeni sifariş yaratmaq ücün melumatları daxil edin:");
            int userId = LogedUser.Id;

            var products = await productService.GetAllProductAsync();
            Console.WriteLine("Mövcud Mehsullar:");
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Ad: {product.Name}, Qiymet: {product.Price}, Stok: {product.Stock}");
            }

            Console.Write("Mehsul ID-sini secin: ");
            int productId = int.Parse(Console.ReadLine());

            var selectedProduct = products.FirstOrDefault(p => p.Id == productId);
            if (selectedProduct == null)
            {
                throw new NotFoundException("Secilen mehsul tapılmadı.");
            }

            Console.Write("Miqdarı daxil edin: ");
            int quantity = int.Parse(Console.ReadLine());

            if (quantity > selectedProduct.Stock)
            {
                throw new InvalidOrderException("Stokda kifayet qeder mehsul yoxdur.");
            }

            var order = new OrderDto
            {
                UserId = userId,
                Id = productId,
                TotalAmount = quantity,
                OrderDate = DateTime.UtcNow,
                ProductId=selectedProduct.Id,
                 Quantity=quantity
            };

            await orderService.CreateOrderAsync(order);

            selectedProduct.Stock -= quantity;
            await productService.UpdateProductAsync(selectedProduct);

            Console.WriteLine("Sifariş uğurla yaradıldı.");
        }
        catch (InvalidOrderException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Sifariş yaratmaq uğursuz oldu: {ex.Message}");
            Console.ResetColor();
        }
        catch (NotFoundException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Xeta: {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Xeta baş verdi: {ex.Message}");
            Console.ResetColor();
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
                Console.WriteLine($"İstifadeci ID: {order.UserId}, Mebleğ: {order.TotalAmount}, Status: {order.Status}");
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

            Console.WriteLine("Sifarişi silmek ücün sifariş ID-sini daxil edin:");
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
            Console.WriteLine("Sifarişi tamamlamaq ücün sifariş ID-sini daxil edin:");
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
            Console.WriteLine("Sifarişi leğv etmek ücün sifariş ID-sini daxil edin:");
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
                Console.WriteLine("Yanlış secim, yeniden cehd edin.");
                break;
        }
    }
    static async Task CreateProduct(ProductService productService)
    {
        try
        {
            var products = await productService.GetAllProductAsync();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Yeni mehsul yaratmaq ücün melumatları daxil edin:");
            Console.ResetColor();
        CreateProductName:
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Mehsulun adını daxil edin: ");
            Console.ResetColor();
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Adsiz mehsul yaratmaq mumkun deil");
                Console.ResetColor();
                goto CreateProductName;
            }

            var productWithSameName = products.FirstOrDefault(p => p.Name == name);
            if (productWithSameName != null)
            {
                throw new Exception("Eyni adla artıq başqa bir mehsul mövcuddur.");
            }

            Console.Write("Mehsulun qiymetini daxil edin: ");
            decimal price = decimal.Parse(Console.ReadLine());
            if (price == 0 && price < 0)
            {
                Console.WriteLine("Qiymet 0 ve ya menfi ola bilmez");
            }
            Console.Write("Mehsulun stok miqdarını daxil edin: ");
            int stock = int.Parse(Console.ReadLine());
            if (stock == 0 && stock < 0)
            {
                Console.WriteLine("Stok 0 ve ya menfi ola bilmez");
            }
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
            var products = await productService.GetAllProductAsync();
            foreach (var item in products)
            {
                Console.WriteLine($"Product ID: {item.Id}, Product Name: {item.Name}, Product Price {item.Price}, Product Desc: {item.Description}");
            }

            Console.WriteLine("Yenilenecek mehsulun ID-sini daxil edin:");
            int id = int.Parse(Console.ReadLine());

            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"ID-si {id} olan mehsul tapılmadı.");
            }

            Console.WriteLine("Mehsulun yeni adını daxil edin:");
            string name = Console.ReadLine();
            var productWithSameName = products.FirstOrDefault(p => p.Name == name);
            if (productWithSameName != null)
            {
                throw new Exception("Eyni adla artıq başqa bir mehsul mövcuddur.");
            }
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
                Console.WriteLine("Hec bir mehsul tapılmadı.");
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
            Console.WriteLine("Hec bir mehsul tapılmadı.");
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
                Console.WriteLine("Yanlış secim, yeniden cehd edin.");
                break;
        }
    }

    static async Task MakePayment(PaymentService paymentService)
    {
        AppDbContext _context = new AppDbContext();
        try
        {
            Console.WriteLine("Ödeniş etmek ücün melumatları daxil edin:");
            Console.Write("Sifariş ID-sini daxil edin: ");
            int orderId = int.Parse(Console.ReadLine());

            Console.Write("Ödeniş mebleğini daxil edin: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            foreach (var item in _context.Products)
            {
                if (item.Id == orderId)
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
static async Task ManageOrderDetails(OrderDetailService orderDetailService, OrderService orderService)
{
    Console.WriteLine("Sifariş Detalları Menyu:");
    Console.WriteLine("1. Sifariş Detalı elave Et");
    Console.WriteLine("2. Sifariş Detallarını Göster");
    Console.WriteLine("3. Geri dön");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await AddOrderDetail(orderDetailService, orderService);
            break;
        case "2":
            await ShowOrderDetails(orderDetailService);
            break;
        case "3":
            return;
        default:
            Console.WriteLine("Yanlış secim, yeniden cehd edin.");
            break;
    }
    static async Task AddOrderDetail(OrderDetailService orderDetailService, OrderService orderService)
    {

        Console.Write("Sifariş ID daxil edin: ");
        var orders = await orderService.GetOrders(); 

        Console.WriteLine("Mövcud Sifarişler:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Sifariş ID: {order.Id}, Sifariş Tarixi: {order.OrderDate}, Status: {order.Status}");
        }

        int orderId = int.Parse(Console.ReadLine());

        Console.Write("Mehsul ID daxil edin: ");
        int productId = int.Parse(Console.ReadLine());

        Console.Write("Miqdarı daxil edin: ");
        int quantity = int.Parse(Console.ReadLine());

        Console.Write("Mehsulun birim qiymetini daxil edin: ");
        decimal pricePerItem = decimal.Parse(Console.ReadLine());

        var orderDetailDto = new OrderDetailDto
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            PricePerItem = pricePerItem
        };

        try
        {
            await orderDetailService.AddOrderDetailAsync(orderDetailDto);
            Console.WriteLine("Sifariş detalı uğurla elave olundu.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta: {ex.Message}");
        }
    }

    static async Task ShowOrderDetails(OrderDetailService orderDetailService)
    {
        Console.Write("Sifariş ID daxil edin: ");
        int orderId = int.Parse(Console.ReadLine());

        try
        {
            var orderDetails = await orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
            if (orderDetails.Count == 0)
            {
                Console.WriteLine("Bu sifarişe aid hec bir detal tapılmadı.");
            }
            else
            {
                foreach (var detail in orderDetails)
                {
                    Console.WriteLine($"Mehsul ID: {detail.ProductId}, Miqdar: {detail.Quantity}, Birim Qiymet: {detail.PricePerItem}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Xeta: {ex.Message}");
        }
    }
}







