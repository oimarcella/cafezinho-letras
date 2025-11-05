using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;

public static class SeedAdmin
{
    public static async Task CreateAdmin(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        // Definindo dados do admin
        string adminUser = "admin";
        string adminSenha = "Admin@2025"; // sempre use uma senha segura

        var admin = await userManager.FindByEmailAsync(adminUser);
        if (admin == null)
        {
            admin = new User
            {
                UserName = "admin",
                Email = "marcella.amorimsa@gmail.com",
                Name = "Administrador"
            };

            var resultado = await userManager.CreateAsync(admin, adminSenha);
            if (resultado.Succeeded)
            {
                // Garante que a role Admin exista
                if (!await roleManager.RoleExistsAsync(ERoles.Administrador.ToString()))
                {
                    await roleManager.CreateAsync(new Role { Name = ERoles.Administrador.ToString() });
                }

                // Adiciona o usuário à role Admin
                await userManager.AddToRoleAsync(admin, ERoles.Administrador.ToString());
            }
        }
    }
}
