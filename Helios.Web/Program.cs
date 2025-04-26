using Avatara;
using Avatara.Figure;
using Helios.Game;
using Helios.Storage;
using Helios.Web.Util;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace Helios.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 40));

            builder.Services.AddDbContext<StorageContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion));

            #region Http Session

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1); // TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = false;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.IsEssential = true;
            });

            #endregion

            #region Recompile views on file change (development mode)

            var mvcBuilder = builder.Services.AddRazorPages();

            builder.Services.AddControllersWithViews();

            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            #endregion

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            builder.Services.AddScoped<ValueManager>();
            builder.Services.AddScoped<PermissionsManager>();

            // View bag filter used for global variables
            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalControllerFilter));
            });

            var app = builder.Build();

            app.UseForwardedHeaders();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            // https://stackoverflow.com/questions/59304132/net-core-how-to-handle-route-with-extra-leading-slash
            app.Use((context, next) =>
            {
                if (context.Request.Path.HasValue &&
                    context.Request.Path.Value.StartsWith("//"))
                {
                    context.Request.Path = new PathString(context.Request.Path.Value.Replace("//", "/"));
                }
                return next();
            });

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.MapControllers();

            LoadCaptcha();
            LoadFigureAssets();

            app.Run();
        }

        private static void LoadCaptcha()
        {
            Console.WriteLine("Loading captcha words...");

            CaptchaUtil.Instance.Load();

            Console.WriteLine($"{CaptchaUtil.Instance.Words.Count} words loaded");
        }

        private static void LoadFigureAssets()
        {
            Console.WriteLine("Loading flash assets...");

            FlashExtractor.Instance.Load();

            Console.WriteLine($"{FlashExtractor.Instance.Parts.Count} flash assets loaded");

            Console.WriteLine("Loading figure data...");

            FiguredataReader.Instance.Load();
            Console.WriteLine($"{FiguredataReader.Instance.FigureSets.Count} figure sets loaded");
            Console.WriteLine($"{FiguredataReader.Instance.FigureSetTypes.Count} figure set types loaded");
            Console.WriteLine($"{FiguredataReader.Instance.FigurePalettes.Count} figure palettes loaded");
        }
    }
}