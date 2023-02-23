# SeedEncrypt
## The bip39 seed phrase encryption tool
- A given seed phrase is encrypted by password to a pair of fake seed phrases
- Providing valid password, the pair of fake seed phrases is decrypted and the original seed phrase is restored
- Wrong password provides just another fake seed phrase
- It's not possible to distinguish a fake seed phrase from a regular one
- It's not possible to detect if the fake seed phrases are a result of encryption

![Img](./art/screenshots/seednecrypt-console-1.png)

# Linux / Ubuntu installation
```
sudo snap install seedencrypt
```

# Run from sourcecode
Dotnet 6 SDK is required in order to run the console app

```
cd SeedEncrypt/src/SeedEncryptConsole
dotnet run
```

