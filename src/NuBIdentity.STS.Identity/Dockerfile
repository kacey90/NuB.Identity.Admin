FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/NuBIdentity.STS.Identity/NuBIdentity.STS.Identity.csproj", "src/NuBIdentity.STS.Identity/"]
COPY ["src/NuBIdentity.Admin.EntityFramework.Shared/NuBIdentity.Admin.EntityFramework.Shared.csproj", "src/NuBIdentity.Admin.EntityFramework.Shared/"]
COPY ["src/NuBIdentity.Admin.EntityFramework.SqlServer/NuBIdentity.Admin.EntityFramework.SqlServer.csproj", "src/NuBIdentity.Admin.EntityFramework.SqlServer/"]
COPY ["src/NuBIdentity.Admin.EntityFramework.PostgreSQL/NuBIdentity.Admin.EntityFramework.PostgreSQL.csproj", "src/NuBIdentity.Admin.EntityFramework.PostgreSQL/"]
COPY ["src/NuBIdentity.Admin.EntityFramework.MySql/NuBIdentity.Admin.EntityFramework.MySql.csproj", "src/NuBIdentity.Admin.EntityFramework.MySql/"]
RUN dotnet restore "src/NuBIdentity.STS.Identity/NuBIdentity.STS.Identity.csproj"
COPY . .
WORKDIR "/src/src/NuBIdentity.STS.Identity"
RUN dotnet build "NuBIdentity.STS.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NuBIdentity.STS.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "NuBIdentity.STS.Identity.dll"]