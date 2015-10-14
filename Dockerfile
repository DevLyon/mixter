FROM mcr.microsoft.com/dotnet/sdk:7.0-bookworm-slim as base
WORKDIR /app

RUN groupadd --gid 1000 app \
 && useradd --uid 1000 --gid app --create-home app \
 && mkdir -p /app \
 && chown -R 1000:1000 /app

USER app

CMD [ "/bin/bash" ]
