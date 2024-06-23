# Start with a base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory to /app or /hng-task1 (choose based on your project structure)
WORKDIR /hng-task1

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Start a new stage for the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory to /app or /hng-task1 in the runtime image
WORKDIR /hng-task1

# Copy the published output from the build stage to the runtime image
COPY --from=build /hng-task1/out .

# Expose port and define entry point, assuming your application listens on this port
EXPOSE 8080

# Optionally set environment variables for PostgreSQL connection string
ENV ConnectionStrings__DefaultConnection="Hostname=dpg-cps45188fa8c73914g40-a;Database=hng_task1_db;Username=hng_task1_db_user;Password=lHlqFMT8zHNZKD0AKcuzC9P9vGkaKUBc;Port=5432"

ENTRYPOINT ["dotnet", "hng-task1.dll"]
