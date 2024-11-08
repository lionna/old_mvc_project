using CarServiceApp.AuthManagement.Service;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CarServiceApp.AuthManagement.Tests.Service
{
    public class RoleServiceTests
    {
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;

        public RoleServiceTests()
        {
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void GetRoles_ReturnsCorrectRoles()
        {
            // Arrange
            var roles = new List<IdentityRole> { new() { Id = "1", Name = "Admin" }, new() { Id = "2", Name = "User" } }.AsQueryable();
            _mockRoleManager.Setup(rm => rm.Roles).Returns(roles);
            var roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);
            // Act
            var result = roleService.GetRoles();
            // Assert
            Assert.Equal(roles.ToDictionary(r => r.Id, r => r.Name), result);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("2", false)]
        public async Task GetByIdAsync_ReturnsCorrectRole(string roleId, bool exists)
        {
            // Arrange
            var role = exists ? new IdentityRole { Id = roleId, Name = "Admin" } : null;
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).Returns(Task.FromResult(role));

            var roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);

            // Act
            var result = await roleService.GetByIdAsync(roleId);

            // Assert
            if (exists)
            {
                Assert.Equal(role, result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task CreateAsync_CreatesRoleSuccessfully()
        {
            // Arrange
            var roleName = "Admin";
            var roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);

            _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await roleService.CreateAsync(roleName);

            // Assert
            Assert.True(result);
            _mockRoleManager.Verify(
                rm => rm.CreateAsync(It.Is<IdentityRole>(r => r.Name == roleName)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalseIfRoleHasUsers()
        {
            // Arrange
            var roleId = "1";
            var roleName = "Admin";
            var role = new IdentityRole { Id = roleId, Name = roleName };

            var users = new List<IdentityUser>
            {
                new() { Id = "1", UserName = "user1" },
                new() { Id = "2", UserName = "user2" }
            };

            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).Returns(Task.FromResult(role));
            _mockUserManager.Setup(um => um.GetUsersInRoleAsync(roleName)).Returns(Task.FromResult<IList<IdentityUser>>(users));

            var roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);

            // Act
            var result = await roleService.DeleteAsync(roleId);

            // Assert
            Assert.False(result);
            _mockRoleManager.Verify(rm => rm.DeleteAsync(role), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrueIfRoleUpdatedSuccessfully()
        {
            // Arrange
            var roleId = "1";
            var roleName = "Admin";
            var newRoleName = "SuperAdmin";
            var role = new IdentityRole { Id = roleId, Name = roleName };

            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).Returns(Task.FromResult(role));
            _mockRoleManager.Setup(rm => rm.UpdateAsync(role)).Returns(Task.FromResult(IdentityResult.Success));

            var roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);

            // Act
            var result = await roleService.UpdateAsync(roleId, newRoleName);

            // Assert
            Assert.True(result);
            Assert.Equal(newRoleName, role.Name);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalseIfRoleNotFound()
        {
            // Arrange
            var roleId = "1";
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).Returns(Task.FromResult<IdentityRole>(null));

            var roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);

            // Act
            var result = await roleService.DeleteAsync(roleId);

            // Assert
            Assert.False(result);
            _mockRoleManager.Verify(rm => rm.DeleteAsync(It.IsAny<IdentityRole>()), Times.Never);
        }
    }
}