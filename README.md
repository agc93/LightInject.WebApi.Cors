# LightInject.WebApi.Cors

## *Integrate CORS in your API the easy way*

---

## Summary

This library is designed to seamlessly integrate complex CORS policies in ASP.NET Web API, using LightInject to take the pain out of integration. No more mucking about with attributes, no more refactoring woes. Simply define a custom `ICorsPolicyProvider` implementation, add the two calls to your startup code, and you're good to go.

## Installation

Install this package from the NuGet Package Manager or by running the following in the Package Manager Console

```
Install-Package LightInject.WebApi.Cors
```


## Getting started

To register your CORS policy, just add a call to `RegisterCorsPolicies()` and a call to `EnableCors()` and you're good to go.

```csharp
using LightInject.WebApi.Cors; //important!

protected void Application_Start()
{
    var container = new ServiceContainer();
    container.RegisterApiControllers();
    container.RegisterCorsPolicies();
    //register other services
    container.EnableCors(GlobalConfiguration.Configuration);
    container.EnableWebApi(GlobalConfiguration.Configuration);              
}
```
And you're done! LightInject will now be configured as the default factory for CORS policies, which will be resolved from the chosen service container.

See the [full documentation](https://agc93.github.io/LightInject.WebApi.Cors) for more details and an OWIN example.

## Limitations

Currently, this provider is a "dumb resolver" and will just use LightInject's default resolution behaviour to return a policy provider to the ASP.NET runtime.  In a future update, you will be able to customise the resolution behaviour.