﻿# SAFEr.Template [![NuGet](https://img.shields.io/nuget/v/SAFEr.Template.svg?style=flat-square)](https://www.nuget.org/packages/SAFEr.Template/)

Strongly opinionated modification of amazing [SAFE Stack Template](https://safe-stack.github.io/) for full-stack development in F#.

## Installation

Install SAFEr Template:

    dotnet new --install SAFEr.Template

Create new directory for your kick-ass full-stack next-unicorn app:

    mkdir NextUnicornApp
    cd NextUnicornApp

Bootstrap your application:

    dotnet new SAFEr

Restore dotnet tools:

    dotnet tool restore

And start it in development mode:

    dotnet run

Your application is now running on:

    http://localhost:8080 // fable frontend
    http://localhost:5000 // backend API


## Disclaimer

I really love [SAFE Stack Template](https://safe-stack.github.io/) template - it's a great thing for devs to start work on F# full-stack apps. However for me, the minimal template is too minimal and the default template is too different from my preferences, so I always struggle with the need to delete/restructure many things. This template makes it right for me and my projects from the very beginning. Feel free to use it.

## Key differences from SAFE Stack template

### Folder structure

- Project folders contains names of application [AppName].Client, [AppName].Server, ...

### Client

- Fable 3 as dotnet tool
- Feliz + Feliz.Bulma as default
- Feliz.Router for secured routing (including fallback to default page when navigating to non-existent page)
- Feliz.UseElmish on page level
- Elmish for wrapper level
- Bulma + Font Awesome as npm packages
- SharedView module for helper functions to navigate to strongly typed pages
- Public content in `public` folder
- Webpack pre-configured to support SCSS files (from `styles/styles.css`)
- Webpack pre-configured to correct SPA routing
- Femto pre-installed
- Yarn instead of npm used

### Server

- Giraffe instead of Saturn as default
- Startup class used for ASP.NET setup
- Application split into more files with separate `WebApp` module

### Shared

- Remoting definition in `API` module

### Deploy

- Farmer specified in separated F# script in `Infrastructure.fsx` as CLI
- Environment definition including AppInsights in `Deployments`

### GitHub Actions

- Continuous Integration / Deployment pipeline prepared in `.github/workflows/CI.yml`