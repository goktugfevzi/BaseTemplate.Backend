FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BaseTemplate.Presentation/BaseTemplate.Presentation.csproj", "BaseTemplate.Presentation/"]
COPY ["BaseTemplate.Business/BaseTemplate.Business.csproj", "BaseTemplate.Business/"]
COPY ["BaseTemplate.Repository/BaseTemplate.Repository.csproj", "BaseTemplate.Repository/"]
COPY ["BaseTemplate.Domain/BaseTemplate.Domain.csproj", "BaseTemplate.Domain/"]
COPY ["BaseTemplate.Shared/BaseTemplate.Shared.csproj", "BaseTemplate.Shared/"]
RUN dotnet restore "./BaseTemplate.Presentation/BaseTemplate.Presentation.csproj"
COPY . .
WORKDIR "/src/BaseTemplate.Presentation"
RUN dotnet build "./BaseTemplate.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BaseTemplate.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaseTemplate.Presentation.dll"]