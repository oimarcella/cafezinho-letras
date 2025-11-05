# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
#COPY . .

# Copia o restante do código e faz o build
#COPY . .
#RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false
#RUN dotnet publish ./CafezinhoELivrosApi/CafezinhoELivrosApi.csproj -c Release -o /app/publish /p:UseAppHost=false
# Copia tudo da raiz (csproj, Program.cs, etc)
COPY . .

# Restaura dependências
RUN dotnet restore ./CafezinhoELivrosApi.csproj

# Publica
RUN dotnet publish ./CafezinhoELivrosApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Etapa de runtime
#FROM mcr.microsoft.com/dotnet/aspnet:9.0
#WORKDIR /app
#COPY --from=build /app/publish .

# Define porta exposta
EXPOSE 5027

# Copia o script para dentro do container
COPY wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

# Comando de inicialização
# ENTRYPOINT ["dotnet", "CafezinhoELivrosApi.dll"]
ENTRYPOINT ["/wait-for-it.sh", "db:3306", "--", "dotnet", "CafezinhoELivrosApi.dll"]