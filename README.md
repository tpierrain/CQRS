# Book a room (*a simple CQRS project*)

Simple project made to explain CQRS during a live coding session at MS experiences'16

The project is a dotnet core ASP.NET web site (in order to be containerized), allowing users:
1. To consult and search for available rooms (READ model)
2. To book a room (WRITE model)

Of course, booking a room (write model) will impact the read model accordingly.
