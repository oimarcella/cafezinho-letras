using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CafezinhoELivrosAPI.Data.Seeds;
public static class SeedData
{
    public static async Task CreateAdmin(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger logger)
    {
        // Definindo dados do admin
        string adminEmail = "admin@cafezinho.com";
        string adminPassword= "Admin@2025"; // sempre use uma senha segura


        // Tentando criar ou ver se existe a role Admin antes
        var roleName = ERoles.Administrador.ToString();
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new Role { Name = roleName };
            await roleManager.CreateAsync(role);
        }

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new User
            {
                UserName = "admin",
                Email = adminEmail,
                Name = "Administrador",
                RoleId = role.Id,
                BirthDate = new DateTime(1997, 10, 17),
            };

            var resultado = await userManager.CreateAsync(admin, adminPassword);
            if (resultado.Succeeded)
            {
                logger.LogInformation("\n✅ Seed CreateAdmin executou com sucesso! \n");
                // Garante que a role Admin exista
                if (!await roleManager.RoleExistsAsync(ERoles.Administrador.ToString()))
                {
                    await roleManager.CreateAsync(new Role { Name = ERoles.Administrador.ToString() });
                }

                // Adiciona o usuário à role Admin
                await userManager.AddToRoleAsync(admin, ERoles.Administrador.ToString());
            }
            else if (!resultado.Succeeded)
            {
                logger.LogInformation("\n✅ Seed CreateAdmin deu errado\n");
                foreach (var error in resultado.Errors)
                    Console.WriteLine(error.Description);
            }
        }
    }

    public static async Task CreateRoles(RoleManager<Role> roleManager, ILogger logger)
    {
        foreach (var roleName in Enum.GetNames(typeof(ERoles)))
        {
            Console.WriteLine($"Verificando Role : ...'{roleName}'");

            if (!string.IsNullOrWhiteSpace(roleName) && !await roleManager.RoleExistsAsync(roleName))
            {
                Console.WriteLine($"Criando'{roleName}'...");

                var result = await roleManager.CreateAsync(new Role { Name = roleName });
                if (!result.Succeeded)
                {
                    logger.LogInformation("✅ Seed CreateRoles falhou");
                    foreach (var error in result.Errors)
                        Console.WriteLine(error.Description);
                }
            }
        }
    }
}
