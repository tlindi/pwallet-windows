# Step 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

RUN addgroup --system pwallet && adduser --system --group pwallet
RUN chown -R pwallet:pwallet /app

# Copy project files
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./
RUN dotnet publish -c Release -o /app/out --no-restore

#
# Step 2: Runtime stage
#
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

ENV HOST_IP=host.docker.internal

RUN addgroup --system pwallet && adduser --system --ingroup pwallet pwallet
RUN chown -R pwallet:pwallet /app

# Install required system libraries
RUN apt-get update && apt-get install -y --no-install-recommends \
    libfontconfig1 \
    libfreetype6 \
    libpng16-16 \
    libjpeg62-turbo \
    libglib2.0-0 \
    libx11-6 \
    libxext6 \
    libxrender1 && \
    rm -rf /var/lib/apt/lists/*

# Copy published files from the build stage
COPY --from=build /app/out .

# Expose the port specified in README.md
EXPOSE 3939

# Ensure correct permissions for execution
RUN chmod +x pWallet.dll

USER pwallet

# Set the entry point
ENTRYPOINT ["dotnet", "pWallet.dll"]
