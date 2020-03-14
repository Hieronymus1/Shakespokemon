
# Instructions

**Run with Docker:**

```
$ docker build -t api-host .
$ docker run -d -p 8080:80 api-host
```

The service will listen on [http://localhost:8080](http://localhost:8080).

**Run with dotnet CLI:**

```
$ dotnet run --project Api/Host/Api.Host.csproj
```

The service will listen on [http://localhost:5000](http://localhost:5000) or the port numbers assigned to the ASPNETCORE_URLS environment variable.

# Architecture

The solution contains 2 processes for the distinct data-flows including the API that is kept in isolation of the [ETL](https://en.wikipedia.org/wiki/Extract,_transform,_load) dependencies that are required to extract, transform and load the baseline data from external services, namely [PokéApi](https://pokeapi.co/), [pokemon.com/pokedex](https://www.pokemon.com/us/pokedex/) and [funtranslation.com/shakespeare](https://funtranslations.com/shakespeare).

## API

The API namespace only contains the dependencies required to run the service including:

* Api.DataAccess.csproj to implement the [repository](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) to access the database.
* Api.Host.csproj to start the [WebHost](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-3.1).

## ETL

The ETL namespace only contains the dependencies to extract, transform and load the baseline data including:

* Etl.Core.csproj to keep the application model agnostic of infrastructure dependencies.
* Etl.DataAccess to implement the [repository](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) to extract the source data with HttpClients.
* Etl.Host.csproj to run the console process.

This separation of concerns in different [vertical-slices](http://www.dhrubo.net/2017/04/microservices-vertical-slicing.html) was done to:

1. Avoid external depedencies because the required data is quite static and no real-time updates are required (new Pokémons are not released every day) in order to:
    * Provide better API response time with preloaded data.
    * Make less requests on the external data-sources on which daily calls are limited by IP.
    * Avoid potential failures if external data-sources are not available (could even run without an internet connection).
    * Keep the ownership of the database to make sure changes on the external data-sources won't impact the API.

5. Implement distinct [bounded-contexts](https://xebia.com/blog/microservices-architecture-principle-3-small-bounded-contexts-over-one-comprehensive-model/) that will be easier to maintain independently.

2. Allow releases in isolation (only the API is packaged in the Docker container).

3. Allow ad hoc runs of the ETL process whenever new Pokémons are published.

4. Apply the [Interface Segregation Principle](https://en.wikipedia.org/wiki/Interface_segregation_principle) that states that no client should be forced to depend on methods it does not use.

Layers were used with the [Dependency Inversion Principle](https://en.wikipedia.org/wiki/Dependency_inversion_principle) to implement an [Onion Architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/) where the model (Core projects) is kept agnostic of infrastructure dependencies (Api.Host, Api.DataAccess, Etl.Console and Etl.DataAccess) without any dependencies from the model (Core projects) to low level implementations. A good example in the .NET for instance is with System<span>.</span>Core that won't pull references to System<span>.</span>Web or System<span>.</span>IO.

This way multiple infrastructure dependencies can be plugged in, interchanged or mocked independently of the application models. In the past 20 years we have seen a sucession of communication protocols (COM+, SOAP, WCF, gRPC and etc), a plethora of UI frameworks (WinForms, WebForms, WPF, Xamarin, Blazor and etc) as well as many types of databases. Pushing those infrastructure concerns outside of the application models with references pointing from the outside toward the core keeps the models easier to maintain and/or reused without pulling extra dependencies. Infrastructure concerns can then also be changed in isolation as described in the [Hexagonal Architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software)) or [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) styles that all share the same intention.

## Exception handling

No exception handling was required because the API can only return OK 200 or Not Found 404 unless the database file is deleted, which couldn't happen with the usage of an isolated container.

No exception handling was required in the ETL process either because it would not be possible to load any data if an external data-source is not available. In this case, an uhandled exception should be bubbled up to wait until the external data-sources are up again or else the data-access layer implementation would need to change to extract the data from alternative sources.

Exception handling is [a bad practice to drive the execution flow](https://docs.microsoft.com/en-us/visualstudio/profiling/da0007-avoid-using-exceptions-for-control-flow?view=vs-2017), because [thrown exceptions can drastically degrade  performances](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/exceptions-and-performance) so the code was rather made more robust to be resilient and more performant.
