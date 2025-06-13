# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto primeiro
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaura pacotes
RUN dotnet restore "WebApi/WebApi.csproj"

# Copia o restante dos arquivos
COPY . .

# Define o diretório de trabalho
WORKDIR "/src/WebApi"

# Faz o build, tentativa de correção com restore
RUN dotnet build "WebApi.csproj" -c Release -o /app/build --restore

# Etapa publish
FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]