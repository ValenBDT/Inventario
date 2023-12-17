
namespace Inventory.DTOs.Auth
{
    public record UserToRegisterDTO(string Email, string Password, string Name);

    public record UserToLoginDTO(string Email, string Password);

    public record UserToListDTO(int Id, string Email, string Name, string Token);
    

    
}