namespace Back_End.Models;

public record UserUpdateModel(string Username, string Email, string Password, byte[] Image);
