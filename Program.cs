namespace Helios.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
			builder.Services.AddDistributedMemoryCache();

			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				//options.Cookie.HttpOnly = true;
				// options.Cookie.IsEssential = true;
			});

			var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
			app.UseSession();
			app.MapControllers();

            app.Run();
        }
    }
}