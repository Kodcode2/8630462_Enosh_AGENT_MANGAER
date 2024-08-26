using AgentClient.Servise;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(); // Added Http Client
builder.Services.AddScoped<IAgentServis, AgentServis>(); // to add servis
builder.Services.AddScoped<IMissionsServis, MissionsServis>(); // to add servis
builder.Services.AddScoped<ITargetServis, TargetServis>(); // to add servis

// builder.Services.AddSingleton<Authentication>(); // AddSingleton -------------------


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
