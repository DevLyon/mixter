FROM php:7.2

RUN apt-get update &&\
    apt-get install git zip -y &&\
    rm -rf /var/lib/apt/lists/* /tmp/*

COPY --from=composer:2.5.8 /usr/bin/composer /usr/bin/composer

ENTRYPOINT [ "/bin/bash" ]
