FROM microsoft/aspnetcore AS base
WORKDIR /app
EXPOSE 5302

FROM microsoft/aspnetcore-build AS build
WORKDIR /src
COPY . .
RUN dotnet restore
WORKDIR /src/Services/microCommerce.CheckoutApi
RUN dotnet build --no-restore -o /app

FROM build AS publish
RUN dotnet publish --no-restore -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "microCommerce.CheckoutApi.dll"]