using Microsoft.AspNetCore.Identity;
using UserComponent;

namespace Tests.UserComponent;

public class UserServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!); // UserManager constructor has 9 parameters
        _tokenServiceMock = new Mock<ITokenService>();
        _userService = new UserService(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnSuccessResult_WhenUserIsCreated()
    {
        // Arrange
        var registerRequest = new RegisterRequest { Email = "test@example.com", Password = "Password123!" };
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.RegisterUserAsync(registerRequest);

        // Assert
        Assert.True(result.Succeeded);
        _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<User>(), "User"), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
        var user = new User { UserName = loginRequest.Email, Email = loginRequest.Email };
        var roles = new List<string> { "User" };
        _userManagerMock.Setup(um => um.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);
        _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginRequest.Password))
            .ReturnsAsync(true);
        _userManagerMock.Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(roles);
        _tokenServiceMock.Setup(ts => ts.GenerateJwtToken(user, roles.Single()))
            .Returns("mocked_token");

        // Act
        var token = await _userService.AuthenticateUserAsync(loginRequest);

        // Assert
        Assert.NotNull(token);
        Assert.Equal("mocked_token", token);
        _userManagerMock.Verify(um => um.FindByEmailAsync(loginRequest.Email), Times.Once);
        _userManagerMock.Verify(um => um.CheckPasswordAsync(user, loginRequest.Password), Times.Once);
        _userManagerMock.Verify(um => um.GetRolesAsync(user), Times.Once);
        _tokenServiceMock.Verify(ts => ts.GenerateJwtToken(user, roles.Single()), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "InvalidPassword" };
        _userManagerMock.Setup(um => um.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync((User)null);

        // Act
        var token = await _userService.AuthenticateUserAsync(loginRequest);

        // Assert
        Assert.Null(token);
        _userManagerMock.Verify(um => um.FindByEmailAsync(loginRequest.Email), Times.Once);
        _userManagerMock.Verify(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _userManagerMock.Verify(um => um.GetRolesAsync(It.IsAny<User>()), Times.Never);
        _tokenServiceMock.Verify(ts => ts.GenerateJwtToken(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }
}