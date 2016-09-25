![BookARoom](https://github.com/tpierrain/cqrs/blob/master/images/bookARoom.gif?raw=true)

BookARoom is a simple project __to explain CQRS__ during a live coding session at MS experiences'16

The project is a __dotnet core__ ASP.NET web site (in order to be containerized in the next session), allowing users:

1. To consult and search for available rooms (READ model)
2. To book a room (WRITE model)

Of course, booking a room (write model) will impact the read model accordingly.

---
![disclaimer](https://github.com/tpierrain/cqrs/blob/master/images/disclaimer.gif?raw=true)

This project is not a real one nor a prod-ready code. The intent here is __to illustrate the CQRS pattern during a 40 minutes session__. Thus, some __trade-offs have been taken__ in that direction (e.g. the usage of *Command* and *Queries* terminology instead of domain specific names that I would have used otherwise).

##### CQRS without Event Sourcing?!?
Yes, since the timing will be short for __[this *MS experiences'16* session in Paris](https://experiences.microsoft.fr/Event/speaker/thomas-pierrain/e11c8e2e-f572-e611-80c3-000d3a2229a6)__ (no more than 30 minutes of live-coding), I've decided to focus only on CQRS pattern, WITHOUT Event Sourcing (ES). Indeed, ES is often a mental dam for people's understanding. 
I also find important that people understand that __CQRS ![loves](https://github.com/tpierrain/cqrs/blob/master/images/heart.png?raw=true) Event sourcing__, but __CQRS != Event sourcing__.

---

### Remaining tasks before the D-day
1. Update the read model when the write model changes (use a fake bus?)
2. Plug the ASP.NET web app
3. Fight against the current anemic model (mainly because I never worked on that topic and that I don't have any expert available to help me ;-( and embrace more the ubiquitous language of this domain.
4. Identify which use case(s) will be usefull to live-code at MS event (to zoom on CQRS) and to find the best possible workflow for it.



---

### Highlights of the talk

1. How __Outside-in TDD__ works.
2. How __Hexagonal Architecture__ can help us to focus on __Domain first__, before tackling the infra code (ASP.NET) in a second time.
3. __CQRS (WITHOUT Event Sourcing)__:
    - why CQRS?
    - Pattern origin
    - How read and write models articulate
    - Eventual consistency
    - Short explanation about why __CQRS ![loves](https://github.com/tpierrain/cqrs/blob/master/images/heart.png?raw=true) Event sourcing__
4. What is __dotnet core__ and how it articulates with the new version of ASP.NET

---

### Projects & Dependencies
- ![directory](https://github.com/tpierrain/cqrs/blob/master/images/directory.png?raw=true) __BookARoom.Domain__:  containing all the domain logic of the solution (for both read and write models). __(has no dependency)__

- ![directory](https://github.com/tpierrain/cqrs/blob/master/images/directory.png?raw=true) __BookARoom.Infra__: ASP.NET core project hosting the infrastructure code (i.e. non-domain one like adapters) for both read and write models. __(depends on both Domain and IntegrationModel projects and is also an ASP.NET dotnet core project)__

- ![directory](https://github.com/tpierrain/cqrs/blob/master/images/directory.png?raw=true) __BookARoom.Tests__: containing tests for all projects. __(depends on all the other BookARoom projects)__

- ![directory](https://github.com/tpierrain/cqrs/blob/master/images/directory.png?raw=true) __BookARoom.IntegrationModel__: command-line project to generate integration json files for hotel (from code). __(has no dependency)__

---

### Tips and tricks

##### How to run the tests

Note: resharper and ncrunch don't support yet dotnet core; you can only run them via Visual Studio test runner (e.g. Ctrl-R, A) or by executing:

     dotnet test 

within the BookARoom.Tests project directory.

...
