# SAFEr.Template [![NuGet](https://img.shields.io/nuget/v/SAFEr.Template.svg?style=flat-square)](https://www.nuget.org/packages/SAFEr.Template/)

Strongly opinionated modification of amazing [SAFE Stack Template](https://safe-stack.github.io/) for full-stack development in F#. 🔥 Now with Azure Functions (isolated) backend support!

## Installation

Install SAFEr Template:

    dotnet new --install SAFEr.Template

Create new directory for your kick-ass full-stack next-unicorn app:

    mkdir NextUnicornApp
    cd NextUnicornApp

Bootstrap your application using Giraffe 🦒:

    dotnet new SAFEr

Or have fully serverless backend using Azure Functions ⚡:

    dotnet new SAFEr --serverless


Restore dotnet tools:

    dotnet tool restore

And start it in development mode:

    dotnet run

Your application is now running on:

    http://localhost:8080 // fable frontend
    http://localhost:5000 // backend API for Giraffe
    http://localhost:7071 // backend API for Azure Functions


## Disclaimer

I really love [SAFE Stack Template](https://safe-stack.github.io/) template - it's a great thing for devs to start work on F# full-stack apps. However for me, the minimal template is too minimal and the default template is too different from my preferences, so I always struggle with the need to delete/restructure many things. This template makes it right for me and my projects from the very beginning. Feel free to use it. And one more thing... This template can change every time I decide to take a different approach to F# full-stack, so stay sharp. :)

## Key differences from SAFE Stack template

### Folder structure

- Project folders contains names of application [AppName].Client, [AppName].Server, ...

### Client

- Fable 4 as dotnet tool
- Feliz + Feliz.DaisyUI as default
- Feliz.Router for secured routing (including fallback to default page when navigating to non-existent page)
- Feliz.UseElmish instead of full Elmish
- TailwindCSS JIT as npm packages
- SharedView module for helper functions to navigate to strongly typed pages
- Public content in `public` folder
- Vite.js instead of webpack
- Femto pre-installed
- Yarn instead of npm used

### Server

- 🔥 Now with support for Azure Functions instead of Giraffe!
- Giraffe instead of Saturn as default
- Startup class used for ASP.NET setup
- Application split into more files with separate `WebApp` module

### Shared

- Remoting definition in `API` module

### GitHub Actions

- Continuous Integration pipeline prepared in `.github/workflows/CI.yml`