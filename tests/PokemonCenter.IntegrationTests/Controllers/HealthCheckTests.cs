using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_ReturnsStatusOk()
    {
        var json = await _client.GetFromJsonAsync<JsonObject>("/health");

        Assert.NotNull(json);
        Assert.Equal("ok", json!["status"]!.ToString());
    }
}
