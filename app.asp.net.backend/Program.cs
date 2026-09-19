using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaData;

var builder = WebApplication.CreateBuilder(args);

// Register the services that the controllers need to handle API requests.
builder.Services.AddDbContext<TriviaDBContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// During development, the React dev server is a separate origin from the API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed the database once at startup so a fresh checkout has quiz content.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TriviaDBContext>();
    DbInitializer.Seed(context, app.Environment.ContentRootPath);
}

////////////////////PIPELINE////////////////////
// DEFAULT Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

////////////////////MIDDLEWARE////////////////////
//Allows http to https redirection
app.UseHttpsRedirection();
//Adds the Cross Reference Origin Source (CORS) policy created earlier
app.UseCors("ReactDev");
app.UseAuthorization();
app.MapControllers();


////////////////////API Initialization////////////////////
app.Run();
