# SeedEncrypt

## Features

- A given seed phrase is transformed into a valid seed phrase using a password
- The transformed seed phrase has a valid BIP39 checksum, making it indistinguishable from a regular seed phrase
- Applying the same password to the transformed seed phrase restores the original seed phrase
- Using an incorrect password produces a different valid seed phrase
- There is no way to detect whether a seed phrase is original or password-transformed

![Img](./art/screenshots/seednecrypt-console-1.png)

# Linux / Ubuntu installation

```
sudo snap install seedencrypt
```

# Run from source code

The .NET 10 SDK is required in order to run the console app

```
cd SeedEncrypt/src/SeedEncryptConsole
dotnet run
```
