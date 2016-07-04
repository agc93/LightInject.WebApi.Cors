# Getting Started

## Installation

Install this package from the NuGet Package Manager or by running the following in the Package Manager Console

```
Install-Package LightInject.WebApi.Cors
```

This package requires both `LightInject.WebApi` and `Microsoft.AspNet.WebApi.Cors`, which will automatically be installed if they're not already. 

> Note that because of the dependency on `Microsoft.AspNet.WebApi.Cors`, this library requires .NET 4.5 or up.

## Usage

Simply add a call to `RegisterCorsPolicies()` and `EnableCors()` alongside your other code. So, for example:

```
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

Or for OWIN hosting:

```
public void Configuration(IAppBuilder app)
{                        
    // Configure Web API for self-host. 
    var config = new HttpConfiguration();
    var container = new ServiceContainer();
    container.RegisterApiControllers();
    container.RegisterCorsPolicies();
    container.EnableCors(config);
    // you can also call config.EnableCors(container);
    container.EnableWebApi(config);
    app.UseWebApi(config); 
}
```

This will automatically discover implementations of `ICorsPolicyProvider` in the current (or specified) assembly, and register them with LightInject. It will then use the provided container to resolve the policy when required.