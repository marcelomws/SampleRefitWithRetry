FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SamplePollyRefit/SamplePollyRefit.csproj", "SamplePollyRefit/"]
RUN dotnet restore "SamplePollyRefit/SamplePollyRefit.csproj"
COPY . .
WORKDIR "/src/SamplePollyRefit"
RUN dotnet build "SamplePollyRefit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SamplePollyRefit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SamplePollyRefit.dll"]