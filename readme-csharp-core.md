Start with Docker:
```
docker run -it --rm -p 5000:5000 -v ${PWD}:/app microsoft/dotnet:2.0.5-sdk-2.1.4
```

You should be logged in Docker
```
cd /app
dotnet build
```

Run tests: 

* For Domain: `dotnet test .\Mixter.Domain.Tests\`
* For Infrastructure: `dotnet test .\Mixter.Infrastructure.Tests\`

Launch Kestrel server: `dotnet run --project .\Mixter.Web\`

Access it via REST client at `http://localhost:5000` (see CoreApi and IdentityApi for available routes)
