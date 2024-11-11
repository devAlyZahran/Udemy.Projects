using SocialMediaLinks.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SocialMediaLinksOptions>(builder.Configuration.GetSection("SocialMediaLinks"));
builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.Services..Configure<SocialMediaLinksOptions>(builder.Configuration.GetSection("SocialMediaLinks"));

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();
