FROM linuxserver/openssh-server:latest

ENV PUBLIC_KEY_FILE=/config/authorized_keys
ENV PUID=1000
ENV PGID=1000
ENV TZ=Etc/UTC
ENV SUDO_ACCESS=true

RUN mkdir /config/sourcecode

RUN apk update && \
    apk add --no-cache gcc make musl-dev linux-headers libgcc g++ libc-dev

WORKDIR /config/sourcecode