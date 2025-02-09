FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /out /app

VOLUME ["/app-keys"]

EXPOSE 3939

ENTRYPOINT ["dotnet", "pWallet.dll"]
