# Tv Show Tracker

### Running mongo
```
docker run -d -p 27017:27107 db mongo
```

### Running backend
run the following command in `src/backend` directory

```
dotnet restore
dotnet build
dotnet run --project TvShowTracker.WebApi
```
and
```
dotnet test
```
to run tests

### Running fronted
run the following command in `src/frontend` directory

```
npm install
HTTPS=true npm start
```