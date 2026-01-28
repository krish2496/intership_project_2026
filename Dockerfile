# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs

# Copy and build Client
COPY client ./client
WORKDIR /src/client
RUN npm install
RUN npm run build

# Build Server
WORKDIR /src
COPY server ./server
WORKDIR /src/server/Tracker.API
RUN dotnet publish Tracker.API.csproj -c Release -o /app

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app .
# Copy the static frontend files to wwwroot
COPY --from=build /src/client/out ./wwwroot

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Tracker.API.dll"]
