using StocksApp.IServices;
using StocksApp.Services;
using StocksApp.ViewModels;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddTransient<IFinnhubService, FinnhubService>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
