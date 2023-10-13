FROM linuxserver/openssh-server:latest

ENV PUBLIC_KEY_FILE=/config/authorized_keys
ENV PUID=1000
ENV PGID=1000
ENV TZ=Etc/UTC
ENV SUDO_ACCESS=true

RUN mkdir /config/sourcecode

RUN echo \ 
	'file="/config/sourcecode/$1" && ' \
	'sudo chmod +x $file && ' \
	'$file' > /run.sh && \ 
	chmod +x /run.sh
