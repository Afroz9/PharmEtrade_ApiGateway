# Use the official .NET image as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the official build image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore any dependencies
COPY ["PharmEtrade_ApiGateway/PharmEtrade_ApiGateway.csproj", "PharmEtrade_ApiGateway/"]
RUN dotnet restore "PharmEtrade_ApiGateway/PharmEtrade_ApiGateway.csproj"

# Copy the rest of the application source code
COPY . .

WORKDIR "/src/PharmEtrade_ApiGateway"
RUN dotnet build "PharmEtrade_ApiGateway.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "PharmEtrade_ApiGateway.csproj" -c Release -o /app/publish

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "PharmEtrade_ApiGateway.dll"]
