FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ExpenseTracker.Api/ExpenseTracker.Api.csproj", "ExpenseTracker.Api/"]
COPY ["ExpenseTracker.csproj", "./"]

RUN dotnet restore "ExpenseTracker.Api/ExpenseTracker.Api.csproj"

COPY . .

WORKDIR "/src/ExpenseTracker.Api"
RUN dotnet publish "ExpenseTracker.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "ExpenseTracker.Api.dll"]