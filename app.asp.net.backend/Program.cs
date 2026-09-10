var builder = WebApplication.CreateBuilder(args);

////////////////////SERVICES////////////////////
// DEFAULT Add services to the container.

builder.Services.AddControllers();
// DEFAULT Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Uses the AddCors ASP.NET service to create a new, strict policy that would allow request from a different origin.
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        //Allows request using any method and any headers from the React app origin
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

////////////////////BUILDERS////////////////////
var app = builder.Build();

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
