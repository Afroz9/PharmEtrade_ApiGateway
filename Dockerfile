# Use the official ASP.NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /src

# Copy the project file and restore dependencies
COPY PharmEtrade_ApiGateway.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./
RUN dotnet publish -c Release -o /app

# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# Port No
EXPORT 5000

# Set the entry point for the application
ENTRYPOINT ["dotnet", "PharmEtrade_ApiGateway.dll"]
