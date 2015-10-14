Start with Docker:
```
docker build -t mixter-cs .
docker run -it --rm -p 5000:5000 -v ${PWD}:/app mixter-cs
```

You should be logged in Docker
```
dotnet build
```

Run tests: 

* For Domain: `dotnet test .\Mixter.Domain.Tests\`
* For Infrastructure: `dotnet test .\Mixter.Infrastructure.Tests\`

Launch server: `dotnet run --project .\Mixter.Web\`

Access it via REST client at `http://localhost:5000` (see BoundedContexts/* for available routes)
