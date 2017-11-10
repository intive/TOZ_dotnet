FROM buildpack-deps:jessie-scm

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
		libgdiplus \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
        unzip \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_DOWNLOAD_URL https://download.microsoft.com/download/E/7/8/E782433E-7737-4E6C-BFBF-290A0A81C3D7/dotnet-dev-debian-x64.1.0.4.tar.gz

RUN curl -SL $DOTNET_SDK_DOWNLOAD_URL --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet


COPY libs /app
WORKDIR /app

run ["ls"]
run ["unzip", "publish.zip"]

WORKDIR /app/publish

#RUN ["dotnet","restore"]
#RUN ["dotnet", "--info"]
#RUN ["dotnet", "build"]
#RUN ["dotnet", "publish"]

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000/admin

CMD ["dotnet", "./Toz.Dotnet.dll"]