# AlpineLinux with a glibc-2.21 and Oracle Java 8

FROM linuxserver/openssh-server:latest

ENV PUBLIC_KEY_FILE=/config/authorized_keys
ENV PUID=1000
ENV PGID=1000
ENV TZ=Etc/UTC
ENV SUDO_ACCESS=true

RUN mkdir /config/sourcecode

WORKDIR /opt
RUN wget https://aka.ms/download-jdk/microsoft-jdk-17.0.6-alpine-x64.tar.gz
RUN tar -xzf microsoft-jdk-17.0.6-alpine-x64.tar.gz
# RUN rm microsoft-jdk-17.0.6-alpine-x64.tar.gz

WORKDIR /opt/jdk-17.0.6+10
# RUN rm -rf *src.zip  \
#            lib/missioncontrol \
#            lib/visualvm  \
#            lib/*javafx*  \
#            jre/lib/plugin.jar       \
#            jre/lib/ext/jfxrt.jar    \
#            jre/bin/javaws           \
#            jre/lib/javaws.jar       \
#            jre/lib/desktop          \
#            jre/plugin               \
#            jre/lib/deploy*          \
#            jre/lib/*javafx*         \
#            jre/lib/*jfx*            \
#            jre/lib/amd64/libdecora_sse.so  \
#            jre/lib/amd64/libprism_*.so  \
#            jre/lib/amd64/libfxplugins.so  \
#            jre/lib/amd64/libglass.so  \
#            jre/lib/amd64/libgstreamer-lite.so  \
#            jre/lib/amd64/libjavafx*.so  \
#            jre/lib/amd64/libjfx*.so

# Set environment

ENV JAVA_HOME /opt/jdk-17.0.6+10

ENV PATH ${PATH}:${JAVA_HOME}/bin

WORKDIR /config