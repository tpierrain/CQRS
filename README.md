# Book a room (*a simple CQRS project*)

Simple project to explain CQRS during a live coding session at MS experiences'16

The project is a dotnet core ASP.NET web site (in order to be containerized), allowing users:

1. To consult and search for available rooms (READ model)
2. To book a room (WRITE model)


Of course, booking a room (write model) will impact the read model accordingly.

## Highlights

1. How __Outside-in__ TDD works
2. How Hexagonal Architecture can help us to focus on __Domain first__, before tackling the __infra (ASP.NET) in a second time__
3. __CQRS without Event sourcing__:
    - why CQRS?
    - pattern origin
    - how read and write models articulate
    - Eventual consistency
    - CQRS loves Event sourcing, but __CQRS != Event sourcing__ (clarifying what Event sourcing is)
4. ASP.NET with __dotnet core__

---


### Projects & Dependencies
- __BookARoom.Domain__ Project containing all the domain logic of the solution (for both read and write models). __(has no dependency)__

- __BookARoom.Infra__ ASP.NET core project hosting the infrastructure code (i.e. non-domain one) for both read and write models. __(depends on both Domain and IntegrationModel projects and is also an ASP.NET dotnet core project)__

- __BookARoom.Tests__ Tests for all projects. __(depends on all the other BookARoom projects)__

- __BookARoom.IntegrationModel__ A command-line project to generate integration json files for hotel (from code). __(has no dependency)__


...
