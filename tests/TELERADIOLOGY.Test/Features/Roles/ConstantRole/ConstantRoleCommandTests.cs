public class ConstantsRolesTests
{
    [Fact]
    public void GetRoles_ShouldReturnAllPredefinedRoles()
    {
        var roles = ConstantsRoles.GetRoles();
        Assert.NotNull(roles);
        Assert.True(roles.Count >= 6);  
        var adminRole = roles.FirstOrDefault(r => r.Name == "Admin");
        Assert.NotNull(adminRole);
        Assert.Equal("ADMIN", adminRole.NormalizedName);
    }
}
