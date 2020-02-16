# Seeding, Weeding Out, and Destroying Startup

## Description

Seeds are dependency injection (DI) aware. They support injection of services which they need to create their yield. E.g. IRepositories in the case of the repository pattern.

Same is with Weed Outs and potentially also with the Destroyer.

In addition, they all might need access to configuration e.g. to configure a DbContext.

Services used in .NET Core application will very likely depend on ILogger.

We need a mechanism to configure all of these, similar to one found in the ASP.NET Core Startup class.

For the sake of brevity, in the below text *seeding*, *weeding out* and *destroying* will be called *execution*.

In this lab we are trying to find answer to the following questions:

- Can we fully reuse the ASP.NET Core seeding mechanism and the semantics of the Startup class?
- Does the concept of environments (develpment, staging, production, &lt;custom&gt;]) makes sense for NSeed execution?
- Can possible NSeed execution "environments" (development, integration tests, unit test, &lt;custom&gt;) be seen as .NET Core environments?
- Should we support configuring logging like in .NET Core?
- Can we fully take over the [generic host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1) approach?
- If the .NET Core approach is not applicable to our case, how the wiring of services etc., should look like?

When we are talking about taking over the .NET Core infrustructrue we are talking about libraries that are available as .NET Standard libraries.

The solution has to cover the following boundary conditions:

- Defining the wiring must be trivial, intuitive and as close to existing .NET Core patterns as possible.
- It must be possible to define separate wirings for seeding, weeding out, and destroying.
- It must be possible to define separate wirings for different use cases like development, integration tests, etc.
- The wiring has to work when execution is done via NSeed Cli and in the unit test runners.
- The user can select a specific wiring when when execution is done via NSeed Cli and in the unit test runners.
- In the case of unit test runners, it should be possible to require seeding via an attribute on a test method or fixture.
- In the case of unit test runners, it should be possible to require seeding in the test method code.

Useful articles to read:

- [App startup in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1)
- [Use multiple environments in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-3.1#environment-based-startup-class-and-methods)
- [Handle errors in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1#startup-exception-handling)
- [Use hosting startup assemblies in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1)
- [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1)
- [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows)
- [IConfigurationBuilder Interface (Microsoft.Extensions.Configuration)](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationbuilder?view=dotnet-plat-ext-3.1)
- [Logging in .NET Core and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1)
- [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1)

## Running the Experiment


## Results

In the lab we have considered the following approaches:

- [Having a Dedicated ISeedingStartup Interface](having-a-dedicated-iseedingstartup-interface)
- [Completely Reusing the .NET Core Startup Mechanism](completely-reusing-the-net-core-startup-mechanism)
- [Base SeedBucketStartup Class with Startup Scope Attributes](base-seedBucketStartup-class-with-startup-scope-attributes)

### Having a Dedicated ISeedingStartup Interface

This was the original approach used in the beginning when the requirements and boundary conditions were not fully understood.

The idea was to have a dedicated interface called `ISeedingStartup` containing the methods like `ConfigureServices()` and `ConfigureConfiguration()`.

The major issue is that all the implementations should implement all the methods (we cannot use default interface implementations, because of the support of .NET Classic) even if some are not needed.
The question was also how to distinguish between seeding, weeding out, and destroying startups. Should we have a dedicated interface for them?

### Completely Reusing the .NET Core Startup Mechanism

The premise was that seeding Startup and .NET Core Startup are, in their essence, exactly the same thing - initial wiring of the application. Also, the tempting idea was to reuse already existing Startup classes to wire future NSeed executions.

After giving it a proper thought the following became clear:

- Startup classes will never be reused as-is. E.g. during the seeding we will want different implementation of services then during Web development. Also, a lot of wiring in Web Sartups are Web related and have nothing to do with seeding. Moreover, at the projects where we utilize seeding via simple console application the approach was always to extract common wirings to a dedicated common project used in Web and Seeds.
- Startup classes configure the app's pipline, something completely unrelated to seeding.
- Startup mechanism is very mighty with lot of estension points and variations that are not relevant to seeding. Supporting it would only caused confusion.

### Base SeedBucketStartup Class with Startup Scope Attributes

This approach takes the best of both previous approaches. A dedicated abstract class called `SeedBucketStartup` containing the methods like `ConfigureServices()` and `ConfigureConfiguration()` that follow the same approach as their counterparts in the .NET Core Startup. All the methods provide standard implementations.

The concrete implementations can optionally be marked with three different attributes:

- SeedingStartup
- WeedingOutStartup
- DestroyingStartup

If a concrete implementation does not have any of those attributes set it is considered that it can be used for all three kind of executions.