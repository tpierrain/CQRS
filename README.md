# Book a room (*a simple CQRS project*)

Simple project to explain CQRS during a live coding session at MS experiences'16

The project is a dotnet core ASP.NET web site (in order to be containerized), allowing users:

1. To consult and search for available rooms (READ model)
2. To book a room (WRITE model)


Of course, booking a room (write model) will impact the read model accordingly.

## Highlights

1. How *Outside-in* TDD works
2. How Hexagonal Architecture can help us to focus on *Domain first*, before tackling the *infra (ASP.NET) in a second time*
3. *CQRS without Event sourcing*:
    - why CQRS?
    - pattern origin
    - how read and write models articulate
    - Eventual consistency
    - CQRS loves Event sourcing, but CQRS != Event sourcing (clarifying what Event sourcing is)
4. ASP.NET with *dotnet core*



