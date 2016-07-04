# LightInject.WebApi.Cors

## *Integrate CORS in your API the easy way*

---

## Summary

This library is designed to seamlessly integrate complex CORS policies in ASP.NET Web API, using LightInject to take the pain out of integration. No more mucking about with attributes, no more refactoring woes. Simply define a custom `ICorsPolicyProvider` implementation, add the two calls to your startup code, and you're good to go.

## Using this documentation:

Click the tabs at the top of the page to navigate:
- **Documentation**: General documentation
- **Reference**: Full API documentation


## Getting started

```csharp
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

See the [introduction](doc/intro.md) for more details and an OWIN example.

## Limitations