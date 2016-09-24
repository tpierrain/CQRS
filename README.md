![BookARoom](https://github.com/tpierrain/cqrs/blob/master/images/bookARoom.gif?raw=true)

BookARoom is a simple project __to explain CQRS__ during a live coding session at MS experiences'16

The project is a __dotnet core__ ASP.NET web site (in order to be containerized in the next session), allowing users:

1. To consult and search for available rooms (READ model)
2. To book a room (WRITE model)

Of course, booking a room (write model) will impact the read model accordingly.

---
![disclaimer](https://github.com/tpierrain/cqrs/blob/master/images/disclaimer.gif?raw=true)

This project is not a real one nor a prod-ready code. The intent here is to illustrate the CQRS pattern during a 45 minutes session. Thus, some trade-offs have been taken in that direction (e.g. the usage of *Command* and *Queries* terminology instead of domain specific names).

#### Remaining tasks
1. To update the read model when the write model changes (use a fake bus?)
2. To plug the web app
3. To fight against the current anemic model ;-(
4. To identify which use case will be usefull to live-code at MS event (to zoom on CQRS)



---

#### Highlights of the talk

1. How __Outside-in__ TDD works
2. How Hexagonal Architecture can help us to focus on __Domain first__, before tackling the __infra (ASP.NET) in a second time__
3. __CQRS WITHOUT Event sourcing (ES)__:
    - why CQRS?
    - pattern origin
    - how read and write models articulate
    - Eventual consistency
    - CQRS ![loves](https://github.com/tpierrain/cqrs/blob/master/images/heart.png?raw=true) Event sourcing, but __CQRS != Event sourcing__ (clarifying what Event sourcing is)
4. What is __dotnet core__ and how it articulates with the new version of ASP.NET

---

#### Projects & Dependencies
- __BookARoom.Domain__:  containing all the domain logic of the solution (for both read and write models). __(has no dependency)__

- __BookARoom.Infra__: ASP.NET core project hosting the infrastructure code (i.e. non-domain one like adapters) for both read and write models. __(depends on both Domain and IntegrationModel projects and is also an ASP.NET dotnet core project)__

- __BookARoom.Tests__: containing tests for all projects. __(depends on all the other BookARoom projects)__

- __BookARoom.IntegrationModel__: command-line project to generate integration json files for hotel (from code). __(has no dependency)__

---

#### Tips and tricks

##### How to run the tests

Note: resharper and ncrunch don't support yet dotnet core; you can only run them via Visual Studio test runner (e.g. Ctrl-R, A) or by executing:

     dotnet test 

within the test project directory.

...
