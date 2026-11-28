using Microsoft.AspNetCore.Identity;

namespace _31241026329_TranHuuTuanBang.Services;

public static class AdminSetup
{
    public const string Role = "Admin";

    public static async Task ConfigureAsync(IServiceProvider services, IConfiguration config)
    {
        var email = config["Admin:Email"];
        var password = config["Admin:Password"];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        using var scope = services.CreateScope();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        if (!await roles.RoleExistsAsync(Role))
            RequireSuccess(await roles.CreateAsync(new IdentityRole(Role)));

        var user = await users.FindByEmailAsync(email);
        if (user == null)
        {
            user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            RequireSuccess(await users.CreateAsync(user, password));
        }

        if (await users.IsInRoleAsync(user, Role))
            return;

        if (!await users.CheckPasswordAsync(user, password))
            throw new InvalidOperationException("Mật khẩu cấu hình không khớp tài khoản quản trị.");

        RequireSuccess(await users.AddToRoleAsync(user, Role));
    }

    private static void RequireSuccess(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(" ", result.Errors.Select(error => error.Description)));
    }
}
