using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Models;

public class User:BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; } 
    public string Password { get; set; } 
    public string Address { get; set; }
}
