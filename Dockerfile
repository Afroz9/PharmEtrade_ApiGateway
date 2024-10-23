# Use official .NET image as the base image for build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the project files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the app source code
COPY . ./

# Publish the application to the /app directory
RUN dotnet publish -c Release -o /app

# Use a lightweight runtime image to reduce the final image size
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory for the runtime container
WORKDIR /app

# Copy the published files from the build container
COPY --from=build /app ./

# Expose the port that the application listens on
EXPOSE 5000
EXPOSE 5001

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "PharmEtrade_ApiGateway.dll"]
