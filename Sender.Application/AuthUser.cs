using Sender.Application.Interfaces;
using Sender.Domain;

namespace Sender.Application
{
    public class AuthUser
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthUser(IPasswordHasher passwordHasher, IUserRepository userRepository, ITokenService tokenService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        public async Task<Result<string>> RegisterAsync(string username, string email, string password)
        {
            var userExists = await _userRepository.ExistsAsync(email);
            if (userExists)
            {
                return Result<string>.Failure("Користувач з таким email вже існує.", 409);
            }

            var hashedPassword = _passwordHasher.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddAsync(user);

            string token = _tokenService.CreateToken(new Token { Id = user.Id, Username = user.Username, Email = user.Email });
            return Result<string>.Success(token, 201);
        }

        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return Result<string>.Failure("Логін або пароль не вірний", 401);
            }

            string token = _tokenService.CreateToken(new Token { Id = user.Id, Username = user.Username, Email = user.Email });

            return Result<string>.Success(token);
        }
    }
}
