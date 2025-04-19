# pWallet

pWallet is a lightweight UI for [Phoenix Server](https://phoenix.acinq.co/server) that allows users to manage their Lightning Wallet with ease.

## Prerequisites

Make sure you have the following installed on your system:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/downloads)

## Setup and Run on Linux, macOS, and Windows

### 1. Clone the Repository

Open a terminal and run:

```sh
git clone https://github.com/Hodladi/pWallet.git
cd pWallet
```

### 2. Build the Application

Run the following command to restore dependencies and build the application:

```sh
dotnet build
```

### 3. Run the Application

#### **Windows**
```sh
dotnet run
```

#### **Linux & macOS**
If you are on Linux or macOS, you might need to give execution permission first:
```sh
chmod +x pWallet.dll
```
Then run:
```sh
dotnet run
```

### 4. Access the Application

Once the application is running, open your browser and navigate to:
```
http://localhost:3939
```

## Publishing the Application
If you want to generate a **self-contained executable**:

#### **Windows**
```sh
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```
Run the executable inside the `publish` folder:
```sh
./publish/pWallet.exe
```

#### **Linux**
```sh
dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish
```
Run the application:
```sh
./publish/pWallet
```

#### **macOS**
```sh
dotnet publish -c Release -r osx-x64 --self-contained true -o ./publish
```
Run the application:
```sh
./publish/pWallet
```

## Additional Notes

- Make sure **port 3939** is not being used by any other service.
- If you encounter permission issues on **Linux/macOS**, try running:
  ```sh
  chmod +x ./publish/pWallet
  ```
- To **stop** the application, press `CTRL+C` in the terminal.

## License

This project is licensed under the Unlicense.

## Screenshots

<img src="https://github.com/user-attachments/assets/44ed7c38-fbd0-48fd-94a4-f73d4d2a1dcc" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/cc9bfdac-ede0-4848-ad01-db09baf02bb1" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/021b9782-18b7-4598-8c4c-d6acf07d4135" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/96837bac-b2e3-43fd-9e78-cdb2ba39a807" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/a9681319-ff32-4c66-904a-6e21a168591d" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/7b555654-224b-439f-9e69-92b1bd95e3c6" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/27b66971-d121-4973-92b5-9d76a020521c" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/3241f1a2-316a-432f-a0b3-5a8ffe56d997" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/706ff42a-6504-4d35-b79b-f439b77c9ee3" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/079649bd-2377-45d1-a760-a637a31dfe5a" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/97740d29-a68d-46b2-9d6e-ea47b0050fd1" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/5d1de241-3d54-4488-a64f-4f8c14d9d904" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/6192d5e3-4958-444b-9c9d-228142f4a2ac" alt="image" width="200">



## Screenshots
<img src="https://github.com/user-attachments/assets/44ed7c38-fbd0-48fd-94a4-f73d4d2a1dcc" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/cc9bfdac-ede0-4848-ad01-db09baf02bb1" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/021b9782-18b7-4598-8c4c-d6acf07d4135" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/96837bac-b2e3-43fd-9e78-cdb2ba39a807" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/a9681319-ff32-4c66-904a-6e21a168591d" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/7b555654-224b-439f-9e69-92b1bd95e3c6" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/27b66971-d121-4973-92b5-9d76a020521c" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/3241f1a2-316a-432f-a0b3-5a8ffe56d997" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/706ff42a-6504-4d35-b79b-f439b77c9ee3" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/079649bd-2377-45d1-a760-a637a31dfe5a" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/97740d29-a68d-46b2-9d6e-ea47b0050fd1" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/5d1de241-3d54-4488-a64f-4f8c14d9d904" alt="image" width="200">
<img src="https://github.com/user-attachments/assets/6192d5e3-4958-444b-9c9d-228142f4a2ac" alt="image" width="200">











