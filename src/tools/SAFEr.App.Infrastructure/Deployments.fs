module SAFEr.App.Infrastructure.Deployments

open Farmer
open Farmer.Builders

let getDefault (appPath:string) =

    // appinsights
    let insights = appInsights {
        name "appinsights-SAFEr.App"
    }

    // web app
    let web = webApp {
        name "web-SAFEr.App"
        service_plan_name "SAFEr.App-serviceplan"
        sku WebApp.Sku.B1
        always_on
        link_to_app_insights insights.Name
        zip_deploy appPath
    }

    arm {
        location Location.WestEurope
        add_resource insights
        add_resource web
    }