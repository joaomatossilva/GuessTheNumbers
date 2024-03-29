using GuessTheNumbers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton(new Evaluator());
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
//uilder.Services.AddTransient<ConfigMiddleware>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.UseMiddleware<ConfigMiddleware>();

app.Run();
