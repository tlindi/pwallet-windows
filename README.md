# pWallet

pWallet is a lightweight UI for [Phoenix Server](https://phoenix.acinq.co/server) that can be set up and run entirely using Docker.

## Prerequisites

Make sure you have the following installed on your system:

- [Docker](https://www.docker.com/products/docker-desktop/)
- Since the application runs in docker the OS is not relevant as long as you got Docker installed on your system.

## Setup and Run Using Docker

Follow these steps to set up and run pWallet using Docker:

### 1. Clone the Repository

```bash
# Clone the repository to your local machine
git clone https://github.com/Hodladi/pWallet.git

# Navigate into the project directory
cd pWallet
```

### 2. Build the Docker Image

Use the provided `Dockerfile` to build the Docker image for pWallet.

```bash
# Build the Docker image
docker build -t pwallet:latest .
```

### 3. Run the Docker Container

Run the application inside a Docker container, exposing port `3939` to access the application.

```bash
# Run the Docker container
docker run -d -p 3939:3939 --name pwallet-container pwallet:latest
```

### 4. Access the Application

Once the container is running, you can access pWallet by navigating to:

```
http://localhost:3939
```

### 5. Stop the Docker Container

To stop the container, use the following command:

```bash
docker stop pwallet-container
```

### 6. Restart the Docker Container

If you need to restart the container after stopping it:

```bash
docker start pwallet-container
```

### 7. Remove the Docker Container

To remove the container:

```bash
docker rm pwallet-container
```

### 8. Remove the Docker Image

If you want to remove the Docker image:

```bash
docker rmi pwallet:latest
```

## Additional Notes

- Ensure that port `3939` is not being used by any other service on your machine.
- Inside the container the application uses port 3939 but feel free to expose whatever port you want externally.
- If you encounter any issues, check the container logs using:

```bash
docker logs pwallet-container
```

## License

This project is licensed under the UnLicense.org