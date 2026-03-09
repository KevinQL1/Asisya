# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["Asisya.Api/Asisya.Api.csproj", "Asisya.Api/"]
COPY ["Asisya.Services/Asisya.Services.csproj", "Asisya.Services/"]
COPY ["Asisya.Data/Asisya.Data.csproj", "Asisya.Data/"]
COPY ["Asisya.Entity/Asisya.Entity.csproj", "Asisya.Entity/"]
RUN dotnet restore "Asisya.Api/Asisya.Api.csproj"

# Copiar todo el código y compilar
COPY . .
WORKDIR "/src/Asisya.Api"
RUN dotnet build "Asisya.Api.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "Asisya.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Asisya.Api.dll"]