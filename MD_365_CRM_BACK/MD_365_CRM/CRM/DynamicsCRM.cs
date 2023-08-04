namespace MD_365_CRM.CRM;

using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Text;
using System.Threading.Tasks;

public class DynamicsCRM
{
    private static string _accessToken = String.Empty;
    private static DateTimeOffset _accessTokenExpiration;
    private readonly IConfiguration _configuration;

    public DynamicsCRM(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> GetAccessTokenAsync()
    {
        if (_accessToken != null && DateTimeOffset.Now < _accessTokenExpiration)
        {
            // Return the existing access token if it's still valid
            return _accessToken; 
        }
        string? clientId = _configuration.GetValue<string>("DynamicsCrmSettings:ClientId");
        string? appKey = _configuration.GetValue<string>("DynamicsCrmSettings:AppKey");
        string? tenantId = _configuration.GetValue<string>("DynamicsCrmSettings:TenantId");
        string? scope = $"{_configuration.GetValue<string>("DynamicsCrmSettings:Scope")}/.default";

        var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .WithClientSecret(appKey)
            .Build();

        AuthenticationResult token = await app.AcquireTokenForClient(new[] { scope })
            .ExecuteAsync();

        _accessToken = token.AccessToken;
        _accessTokenExpiration = token.ExpiresOn;

        return _accessToken;
    }


    public async Task<HttpResponseMessage> CrmRequest(HttpMethod httpMethod, string accessToken, string requestUri, string body = "")
    {
        var client = new HttpClient();
        var msg = new HttpRequestMessage(httpMethod, requestUri);
        msg.Headers.Add("OData-MaxVersion", "4.0");
        msg.Headers.Add("OData-Version", "4.0");
        msg.Headers.Add("Prefer", "odata.include-annotations=\"*\"");

        // Passing AccessToken in Authentication header  
        msg.Headers.Add("Authorization", $"Bearer {accessToken}");

        if (body != "")
            msg.Content = new StringContent(body, UnicodeEncoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await client.SendAsync(msg);
        return httpResponseMessage;
    }
}

