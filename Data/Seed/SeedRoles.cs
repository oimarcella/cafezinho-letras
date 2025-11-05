using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;

public static class SeedRoles
{
    public static async Task CreateRoles(RoleManager<Role> roleManager)
    {
        foreach (var roleName in Enum.GetNames(typeof(ERoles)))
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }
    }
}