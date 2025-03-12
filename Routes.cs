namespace Portfolio
{
    public static class Routes
    {
        public static IEndpointRouteBuilder MapCustomRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "admin",
                pattern: "admin",
                defaults: new { area = "Admin", controller = "Dashboard", action = "Index" });

            endpoints.MapControllerRoute(
                name: "admin_login",
                pattern: "admin/login",
                defaults: new { area = "Admin", controller = "Account", action = "Login" });

            return endpoints;
        }
    }
}
