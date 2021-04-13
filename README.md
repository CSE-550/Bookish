# Bookish
A book review social media website


# Startup

- Clone the project
- Install dotnet sdk/runtime
- Install postgreSQL
- Run `dotnet tool install --global dotnet-ef`
- Update Context.cs to have connection string to database
- Update appsettings.json to have connection string to database
- Run `dotnet ef database update --project Data`
- Start project

# Deployment

## Note: This assumes you have heroku access

- Install heroku ci
- Install docker
- `heroku login`
- `heroku containger:login`
- `cd to Bookish top level`
- `heroku container:push web -a bookish-team2 --context-path={Path to top level repo}`
- `cd to Bookish Server`
- `heroku container:release web -a bookish-team2`
