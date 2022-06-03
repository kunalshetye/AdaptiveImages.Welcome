## appsettings.json
```
"EPiServer": {
    "Welcome": {
        "Api": {
            "ClientId": "xxxx-xxxx-xxxx-xxxx-xxxx",
            "ClientSecret" : "xxxxxxxxxx"
        }
    }
}
```
## Startup.cs
```
 services.AddWelcomeAdaptiveImageProvider(
    _configuration.GetValue<string>("EPiServer:Welcome:Api:ClientId"),
    _configuration.GetValue<string>("EPiServer:Welcome:Api:ClientSecret"));
```

In case _configuration is missing for you, try this:
```
private readonly IWebHostEnvironment _webHostingEnvironment;
private readonly IConfiguration _configuration;

public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
{
    _webHostingEnvironment = webHostingEnvironment;
    _configuration = configuration;
}
```