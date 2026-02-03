# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Copy and build Server only (frontend deployed separately on Vercel)
COPY server ./server
WORKDIR /src/server/Tracker.API
RUN dotnet publish Tracker.API.csproj -c Release -o /app

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Tracker.API.dll"]
