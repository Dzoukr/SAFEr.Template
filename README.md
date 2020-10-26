# SAFEr.Template [![NuGet](https://img.shields.io/nuget/v/SAFEr.Template.svg?style=flat)](https://www.nuget.org/packages/SAFEr.Template/)

Strongly opinionated modification of amazing [SAFE Stack Template](https://safe-stack.github.io/) for full-stack development in F#.

## Installation

Create new directory for your kick-ass full-stack next-unicorn app:

    mkdir NextUnicornApp
    cd NextUnicornApp
    
Install SAFEr Template:

    dotnet new --install SAFEr.Template

Bootstrap your application:

    dotnet new SAFEr

And start it in development mode:

    dotnet fake build

## Disclaimer

I really love [SAFE Stack Template](https://safe-stack.github.io/) template - it's a great thing for devs to start work on F# full-stack apps. However for me the minimal template is too minimal and default template is too different from my preferences, so I always struggle with need to delete / restructure many things. This template makes it right for me and my projects from the very beginning. Feel free to use it.

## Key differences from SAFE Stack template

### Folder structure

- Project folders contains names of application [AppName].Client, [AppName].Server, ...

### Client

- Feliz + Feliz.Bulma as default
- Feliz.Router for secured routing (including fallback to default page when navigating to non-existent page)
- Feliz.UseElmish for sub-pages
- Bulma + Font Awesome as npm packages
- Application split into Domain / State / View
- SharedView module for helper functions to navigate to strongly typed pages
- Public content in `public` folder (including `index.html`)
- Webpack pre-configured to support SCSS files (from `styles/styles.css`)
- Webpack pre-configured to correct SPA routing
- Femto pre-installed
- Yarn instead of npm used

### Server

- Giraffe instead of Saturn as default
- Startup class used for ASP.NET setup
- Application split into more files with separate `WebApp` module

### Shared

- Remoting definition in `Communication` module

### Deploy

- Farmer specified in separated console application in `tools/[AppName].Infrastructure` as CLI
- Environment definition including AppInsights in `Deployments`

### GitHub Actions

- Continuous Integration / Deployment pipeline prepared in `.github/workflows/CI.yml`