FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Services/Compute/Compute.Application/Compute.Application.csproj src/Services/Compute/Compute.Application/
COPY src/Services/Compute/Compute.Infrastructure/Compute.Infrastructure.csproj src/Services/Compute/Compute.Infrastructure/
COPY src/Services/Compute/Compute.Domain/Compute.Domain.csproj src/Services/Compute/Compute.Domain/
RUN dotnet restore src/Services/Compute/Compute.Application/Compute.Application.csproj
COPY . .
WORKDIR /src/src/Services/Compute/Compute.Application
RUN dotnet build Compute.Application.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Compute.Application.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Compute.Application.dll"]
