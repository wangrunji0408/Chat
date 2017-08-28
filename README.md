# Chat

A chat client/server in .NetCore/C#.

## Basic

* Platform: .Net Core 2.0
* Libraries: gRPC

## Usage

* Setup server

  ```shell
  cd Chat.Server.ConsoleApp/
  dotnet run -- -p 8080
  ```

  This will start a server listening on port 8080.

* Signup users on server

  Command: `signup <username> <password>`

  ```
  > signup user1 password
  > signup user2 password
  ```

* Run a client

  ```shell
  cd Chat.Client.ConsoleApp/
  dotnet run -- -s localhost:8080
  ```

* Login

  Command: `login <userid> <password>`

  ```
  > login 1 password
  ```

  And login user2 on another client.

* Send message

  Command: `send <text>`

  ```
  > send Hello!
  ```

  No accident, you will receive the following message on both clients.

  ```
  1: Hello!
  ```

That's all. 😂 More features will be added in the future.

## Documents

* [Release Note](./docs/Note.md)