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
 - __BookARoom.Domain__ has no dependency
 - __BookARoom.Integration__ has no dependency
 - __BookARoom.Infra__ depends on both Domain and Integration projects and is also an ASP.NET dotnet core project
 - __BookARoom.Tests__ depends on all the other BookARoom projects.

 

...



