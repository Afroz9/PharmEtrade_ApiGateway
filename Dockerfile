# Use the official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY PharmEtrade_ApiGateway.csproj ./
RUN dotnet restore PharmEtrade_ApiGateway.csproj

# Copy the entire project and publish the release build
COPY . .
RUN dotnet publish PharmEtrade_ApiGateway.csproj -c Release -o /app

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Set the working directory for the runtime
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /app .
# Expose ports (optional: adjust if needed)
EXPOSE 5000

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "PharmEtrade_ApiGateway.dll"]
