namespace CSharpPromptSnippets
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method, which is the entry point of the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer(); // discover endpoints through metadata
            builder.Services.AddSwaggerGen();           // generate Swagger docs

            builder.Services.AddLocalization();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "de-DE" };
                options.SetDefaultCulture("en-US")
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();  //makes content available at /swagger/v1/swagger.json
                app.UseSwaggerUI(c => {  //serves the Swagger UI as per route prefix (default is /swagger)
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); 
                    c.RoutePrefix = string.Empty; 
                });
            }

            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.MapControllers();

            app.Run();
        }
    }
}
