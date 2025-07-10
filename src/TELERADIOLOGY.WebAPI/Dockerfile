
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["src/TELERADIOLOGY.WebAPI/TELERADIOLOGY.WebAPI.csproj", "src/TELERADIOLOGY.WebAPI/"]
COPY ["src/TELERADIOLOGY.Application/TELERADIOLOGY.Application.csproj", "src/TELERADIOLOGY.Application/"]
COPY ["src/TELERADIOLOGY.Domain/TELERADIOLOGY.Domain.csproj", "src/TELERADIOLOGY.Domain/"]
COPY ["src/TELERADIOLOGY.Infrastructure/TELERADIOLOGY.Infrastructure.csproj", "src/TELERADIOLOGY.Infrastructure/"]
RUN dotnet restore "src/TELERADIOLOGY.WebAPI/TELERADIOLOGY.WebAPI.csproj"

COPY . .
WORKDIR /app/src/TELERADIOLOGY.WebAPI
RUN dotnet publish -c Release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "TELERADIOLOGY.WebAPI.dll"]