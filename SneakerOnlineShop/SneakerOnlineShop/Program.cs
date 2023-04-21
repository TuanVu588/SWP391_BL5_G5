using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

var builder = WebApplication.CreateBuilder(args);
// Bo sung kien truc cho ung dung web vao container cua web server
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddDbContext<SWP391_DBContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("swp391db"));
});
//builder.Services.AddScoped<SWP391_DBContext>();
builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromMinutes(30));
builder.Services.AddAutoMapper(typeof(SneakerOnlineShop.DTO.MapperProfile));
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.UseStaticFiles();
app.UseSession();
app.MapRazorPages();
//app.MapHub<SignalRServer>("/signalRServer");

app.Run();