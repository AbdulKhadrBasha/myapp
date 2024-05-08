FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build-env
WORKDIR /App
EXPOSE 8080
EXPOSE 8081
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "./myapp.csproj"
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "myapp.dll"]