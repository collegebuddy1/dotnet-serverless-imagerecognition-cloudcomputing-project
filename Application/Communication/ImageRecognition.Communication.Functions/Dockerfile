FROM public.ecr.aws/lambda/dotnet:6 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
COPY ["ImageRecognition.Communication.Functions.csproj", "."]
RUN dotnet restore "./ImageRecognition.Communication.Functions.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ImageRecognition.Communication.Functions.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "ImageRecognition.Communication.Functions.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .