# REST API VERSIONING

- Cover how you can use MicroSofts  elegant API versioning Nuget package to perform all the methods of handlings API versioning in an ASP.Net core WebAPI.
- Without further I do lets dive right in the code.
- Controller with a single endpoint that gets a string from an the route and returns the weather information.
- 3 years in the future we want to make a breaking change and not affect existing users but support multiple users in a parallel manner

## Install nuget and register the service

- Install nuget package package
- Register it in the `ConfigureServices` method of `startup.cs` using `services.AddApiVersioning()`.
- With this we have some form of API versioning in our project.

```C#
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning();
    }
```

## Test Insomnia

- Returns error (API version not specified). Can be fixed by Adding the query string `?api-version=1.0`
- However this is not ideal because if you make this mandatory, your previous consumers may break.

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
- Duplicate controller. Change class names of namespaces. Add breaking changes to the new V2 class.
- Now add the attribute `[ApiVersion("1.0")]` and `[ApiVersion("2.0")]` to the controller class and `[MapToApiVersion("2.0")]` to the method.

## Test in Insomnia

- Run request without query string `?api-version=1.0` to still get V1.0 and `?api-version=2.0` for V2


## Supporting URL based Versioning
- Very good way of versioning application
- At the controller change the route attribute to
- You can use major and minor versions v1 or v1.0 and it work in the query string
```C#
    [Route("api/v{version:apiVersion}/weather)"]
```

## Passing the Version number in different way from besides a query parameter

- Use the Accept header/Content-type
```C#
    options.ApiVersionReader = new MediaTypeApiVersionReader("version");
```

## Test in Insomnia

- Go to `Header`. Add or Use `Accept`. Fix value `application/json;version=2.0`

## My Own header parameter to handle version

```C#
    options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
```

## Test in Insomnia

- Go to `Header`. Add value `X-Default`. Fix value `2.0`
- Note Remove `version=2.0` form `Accept` header and leave only `application/json`


## It doesn't stop there, You can combine both

```C#
    options.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("version")
    );
```

## Reporting API version to consumers is good practice

- The would want to know which one they are using
- Are there any minor version they could upgrade to and so forth
```C#
    options.ReportApiVersions = true;
```

## Deprecating a version after we have informed our consumers

At the controller set `[ApiVersion("1.0", Deprecated = true)]`

