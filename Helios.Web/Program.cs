using Avatara.Figure;
using Avatara;
using Helios.Web.Util;
using Microsoft.EntityFrameworkCore;
using Helios.Storage;
using Helios.Game;
using Microsoft.Extensions.FileProviders;

namespace Helios.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<StorageContext>(options =>
                options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();

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

            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
            else
            {
                builder.Services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
            }

			#endregion

			builder.Services.AddScoped<ValueManager>();

            // View bag filter used for global variables
            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalControllerFilter));
            });

            var app = builder.Build();

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