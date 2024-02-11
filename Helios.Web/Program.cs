using Avatara.Figure;
using Avatara;
using Helios.Web.Util;
using Microsoft.EntityFrameworkCore;
using Helios.Web.Storage;

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
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.HttpOnly = true;
                // options.Cookie.IsEssential = true;
            });

            #endregion

            #region Recompile views on file change (development mode)

            var mvcBuilder = builder.Services.AddRazorPages();

            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            #endregion

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