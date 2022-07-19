using Framework.Validations.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Text;
using System.Text.Json;
using WebApplication1.Controllers;

namespace TestProject1
{
    public class UnitTest1
    {
        public class Response
        {
            public string? Type { get; set; }
            public string? Title { get; set; }
            public int Status { get; set; }
            public string? TraceId { get; set; }
            public Dictionary<string, List<string>>? Errors { get; set; }
        }

        [Fact]
        public async Task MappingDoesNotWork1()
        {
            var application = new WebApplicationFactory<WebApplication1.Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                        services.AddMvcCore()
                            .AddDataAnnotationsLocalization(options => 
                                options.DataAnnotationLocalizerProvider = (type, factory) =>
                                        factory.Create(typeof(SharedResource1))
                            ));
                });

            var client = application.CreateClient();
            var body = new ExampleClass { Id = null };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://www.test.com/Example?culture=es", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<Response>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            Assert.NotNull(responseModel);
            var error = Assert.Single(responseModel!.Errors);
            Assert.Equal(nameof(ExampleClass.Id), error.Key);
            var errorMessage = Assert.Single(error.Value);

            Assert.Equal("El campo de Id es obligatorio.", errorMessage);
        }

        [Fact]
        public async Task MappingDoesNotWork2()
        {
            var application = new WebApplicationFactory<WebApplication1.Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                        services.AddMvcCore()
                            .AddDataAnnotationsLocalization(options =>
                                options.DataAnnotationLocalizerProvider = (type, factory) =>
                                        factory.Create(typeof(SharedResource2))
                            ));
                });

            var client = application.CreateClient();
            var body = new ExampleClass { Id = null };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://www.test.com/Example?culture=es", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<Response>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            Assert.NotNull(responseModel);
            var error = Assert.Single(responseModel!.Errors);
            Assert.Equal(nameof(ExampleClass.Id), error.Key);
            var errorMessage = Assert.Single(error.Value);

            Assert.Equal("El campo de Id es obligatorio.", errorMessage);
        }
    }
}