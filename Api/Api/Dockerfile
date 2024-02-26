# Use the .NET 5 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the container
COPY . .

# Restore dependencies and build the application
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the built application from the 'build' stage
COPY --from=build /app/publish .

# Expose port 80 and 443 to the outside world
EXPOSE 80
EXPOSE 443

# Set environment variables for HTTPS redirection
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_HTTPS_PORT=443

# Command to run the application
ENTRYPOINT ["dotnet", "Api.dll"]

