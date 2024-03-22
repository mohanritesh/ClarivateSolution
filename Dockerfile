#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TM_WebAPI/PaymentScreening_API.csproj", "TM_WebAPI/"]
COPY ["TM_DataLayer/PScrn_DataLayer.csproj", "TM_DataLayer/"]
COPY ["TM_Models/PScrn_Models.csproj", "TM_Models/"]
RUN dotnet restore "TM_WebAPI/PaymentScreening_API.csproj"
COPY . .
WORKDIR "/src/TM_WebAPI"
RUN dotnet build "PaymentScreening_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentScreening_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+80
ENTRYPOINT ["dotnet", "PaymentScreening_API.dll"]