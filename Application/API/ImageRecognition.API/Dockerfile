FROM public.ecr.aws/lambda/dotnet:6 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY ["ImageRecognition.API.csproj", "."]
RUN dotnet restore "./ImageRecognition.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ImageRecognition.API.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "ImageRecognition.API.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .