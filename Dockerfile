# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
#COPY . .

# Copia o arquivo de projeto e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante do código e faz o build
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Define porta exposta
EXPOSE 5027

# Comando de inicialização
ENTRYPOINT ["dotnet", "CafezinhoELivrosApi.dll"]