# RoomsBooking project (steps to follow)

## Setup your dev environment

### 1. Install all the plugins and dependencies for ASP.NET dotnet core project
### 2. Configure your project.json to allow unit test execution in a dotnet core environment
(note: review what I've found after my own explorations: https://github.com/dotnet/core-docs/blob/master/docs/core/testing/unit-testing-with-dotnet-test.md )

1. Within the *project.json* file, comment:
    ````C#
     "frameworks": {
        //"netstandard1.6": {
        //  "imports": "dnxcore50"
        //}
    ````
2. Set (instead) the values specified below (source:  [https://github.com/nunit/dotnet-test-nunit/tree/release/3.4.0#projectjson](https://github.com/nunit/dotnet-test-nunit/tree/release/3.4.0#projectjson) )
    ````C#
    {
        "version": "1.0.0-*",
    
        "dependencies": {
            "NUnit": "3.4.1",
            "dotnet-test-nunit": "3.4.0-beta-1"
        },
    
        "testRunner": "nunit",
    
        "frameworks": {
            "netcoreapp1.0": {
                "imports": "portable-net45+win8",
                "dependencies": {
                    "Microsoft.NETCore.App": {
                        "version": "1.0.0-*",
                        "type": "platform"
                    }
                }
            }
        }
    }
    ````
3. Open a cmd window at the directory root of your project and execute:
    ````C#
    # Restore the NuGet packages
    dotnet restore

    # Run the unit tests in the current directory
    dotnet test
    ````


## Coding
