# Estágio 1: Build (para compilar e publicar)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de solução e de projeto para restaurar dependências
COPY ["TesteAtak.csproj", "TesteAtak/"]
RUN dotnet restore "TesteAtak/TesteAtak.csproj"

# Copia todo o restante do código
COPY . .

# Publica a aplicação
WORKDIR "/src/TesteAtak"
RUN dotnet publish "TesteAtak.csproj" -c Release -o /app/publish

# Estágio 2: Final (para rodar a aplicação, imagem menor e mais segura)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080 # O Railway usa a porta 8080 por padrão

# Copia os arquivos publicados do estágio de build
COPY --from=build /app/publish .

# Define o comando de inicialização
ENTRYPOINT ["dotnet", "TesteAtak.dll"]
