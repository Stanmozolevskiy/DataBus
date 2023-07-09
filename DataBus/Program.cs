WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Handling the environments here (Heroku or local Json)
IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


builder.Services.AddControllers()
    .AddXmlSerializerFormatters()
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();


public static class IConfigurationExtensions
{
    public static string GetVariableByEnvironment(this IConfiguration configuration, string variable)
    {
        string environment = configuration["ENVIRONMENT"];

        // Return production connection string from Heroku variables
        if (environment == "production")
        {
            Console.WriteLine(environment, Environment.GetEnvironmentVariable(configuration[variable]));
            return Environment.GetEnvironmentVariable(configuration[variable]);
        }

        // Return development connection string from the appsettings.json
        else
        {
            Console.WriteLine(environment, configuration[variable]);
            return configuration[variable];
        }
        
    }
}