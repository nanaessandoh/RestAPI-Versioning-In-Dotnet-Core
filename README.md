# REST API VERSIONING

- Cover how you can use MicroSofts  elegant API versioning Nuget package to perform all the methods of handlings API versioning in an ASP.Net core WebAPI.
- Without further I do lets dive right in the code.
- Controller with a single endpoint that gets a string from an the route and returns the weather information.
- 3 years in the future we want to make a breaking change and not affect existing users but support multiple users in a parallel manner

## Install nuget and register the service

```C#
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning();
    }
```

## Add Default Version to Startup.cs

```C#
services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultVersion = new ApiVersion(1,0);  // ApiVersion.Default;
});
```

## Add Second Version to the controller

- Fine practice to create sub-controllers in directories V1 and V2 and have route attributes for the different versions.
```C#
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]

    [MapToApiVersion("2.0")]
```

## Test in Insomnia

- Run request without query string `?api-version=1.0` to still get V1.0 and `?api-version=2.0` for V2


## Passing the Version number in different way from besides a query parameter

- Use the Accept header/Content-type
```C#
    options.ApiVersionReader = new MediaTypeApiVersionReader("version"); // Accept header field
    options.ApiVersionReader = new HeaderApiVersionReader("X-Version"); // Custom header field

    options.ApiVersionReader = ApiVersionReader.Combine(   // Supporting both versions
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("version")
    );
```

## Test in Insomnia

- Go to `Header`. Add or Use `Accept`. Fix value `application/json;version=2.0`

## Test in Insomnia

- Go to `Header`. Add value `X-Default`. Fix value `2.0`
- Note Remove `version=2.0` form `Accept` header and leave only `application/json`

## Reporting API version to consumers is good practice

- The would want to know which one they are using
- Are there any minor version they could upgrade to and so forth

```C#
    options.ReportApiVersions = true;
```

## Deprecating a version after we have informed our consumers

At the controller set `[ApiVersion("1.0", Deprecated = true)]`


## Supporting URL based Versioning
- Very good way of versioning application
- At the controller change the route attribute to
- You can use major and minor versions v1 or v1.0 and it work in the query string

```C#
    [Route("api/v{version:apiVersion}/weather)"]
```