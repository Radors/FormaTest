FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       clang zlib1g-dev

WORKDIR /source
COPY . .
RUN dotnet publish FormaTest.csproj -o /app

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-preview
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["/app/FormaTest"]