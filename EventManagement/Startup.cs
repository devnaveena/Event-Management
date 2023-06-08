using System.Data.SqlClient;
using EventManagement.Repository;
using EventManagement.Service;
using EventManagement.Contracts;

namespace EventManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services) // Configure services 
        {
            services.AddSwaggerGen();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventServices, EventService>();
            services.AddControllers();
            var connectionString = Configuration.GetConnectionString("MyConnectionString");
            var sqlConnection = new SqlConnection(connectionString);

            //To import user data using csv file
            var user = new Userdata(connectionString);
            services.AddSingleton(user);
            user.importUserData();

            // Register SqlConnection
            services.AddSingleton(sqlConnection);


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs");
                });
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}