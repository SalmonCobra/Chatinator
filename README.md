# Chatinator 

## Project Scope

Chatinator – name subject to change – is supposed to be an easy-to-maintain, and secure chat server, for text chat, and later VOIP, and Video. It is supposed to be fast and efficient, while maintaining a relatively simple and intuitive API.

## Server Goals

1. Low-latency data access.
2. Efficient memory usage.
3. Simple intuitive API.
4. Secure – TLS transport, E2EE support, and minimal server knowledge of sensitive client data.
5. Minimize synchronization and bandwidth usage through caching and incremental updates.
6. Reliable – Server should be able to shut down gracefully and recover from errors.
7. Extensibillity – Server should be able to be extended to support new features with reasonable ease.


## Core Systems

### Connection Layer
- Common connection abstraction.
- Connection lifecycle management.
- Session management.

### Data Layer
- Storage Engine
- Data Structures
- Incremental Synchronization
- Cache Management
- Encryption

### Security Layer
- TLS Transport
- E2EE Support
- Key Management
- Server-side Authentication
- Secure Credential Storage (Salted Password Hashing)







