namespace realworldProject.Models
{
    public record UserRequestEnv<T>(T user);
    public record UserResponseEnv<T>(T user);

    public record UserLoginRequest(string Email, string Password);

    public record UserCreateRequest(string Email, string Password, string username, string? bio, string? image);
    public record UserUpdateRequest(string Email);

    public record UserResponse(string Email, string Token, string username, string? bio, string? image);
    public record UserUpdateInfo(string? Email, string? username, string? password, string? bio, string? image);
    public record profile(string? username, string? bio, string? image, bool following);
}
