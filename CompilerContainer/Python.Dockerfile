FROM linuxserver/openssh-server:latest

ENV PUBLIC_KEY_FILE=/config/authorized_keys
ENV PUID=1000
ENV PGID=1000
ENV TZ=Etc/UTC
ENV SUDO_ACCESS=true

RUN mkdir /config/sourcecode

ENV PYTHONUNBUFFERED=1
RUN apk add --update --no-cache python3 && ln -sf python3 /usr/bin/python
RUN python3 -m ensurepip
RUN pip3 install --no-cache --upgrade pip setuptools 
