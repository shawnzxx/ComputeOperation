FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["src/Services/Audit/Audit.Application/Audit.Application.csproj", "src/Services/Audit/Audit.Application/"]
COPY ["src/Services/Audit/Audit.Infrastructure/Audit.Infrastructure.csproj", "src/Services/Audit/Audit.Infrastructure/"]
COPY ["src/Services/Audit/Audit.Domain/Audit.Domain.csproj", "src/Services/Audit/Audit.Domain/"]
RUN dotnet restore "src/Services/Audit/Audit.Application/Audit.Application.csproj"
COPY . .
WORKDIR "/src/src/Services/Audit/Audit.Application"
RUN dotnet build "Audit.Application.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Audit.Application.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Audit.Application.dll"]