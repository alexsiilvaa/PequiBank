FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG RUNTIME=linux-x64
WORKDIR /src
COPY ./src .
RUN dotnet restore -r $RUNTIME
RUN dotnet publish --no-restore -r $RUNTIME -c Release --self-contained true -p:DebugType=none -p:DebugSymbols=false -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -o output ./Presentation/Presentation.csproj

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0
WORKDIR /app
COPY --from=build /src/output .
RUN apt update -y && apt install -y wget
ENTRYPOINT ["./PequiBank"]