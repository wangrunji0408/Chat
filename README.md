# Chat

A chat client/server in .NetCore/C#.

## Basic

* Platform: .Net Core 2.0
* Dependencies:
  * Communication: `gRPC` + `Protobuf`
  * Persistance: `EntityFrameworkCore`
  * ObjectMapping: `AutoMapper`
  * EventBus: `System.Reactive`
  * Logging: `Microsoft.Extensions.Logging` +` NLog`
  * DI: `Microsoft.Extensions.DependencyInjection`
  * CommandParsing: `CommandLineParser`
  * Testing: `xUnit`

## Usage

* Setup server

  ```shell
  cd Chat.Server.ConsoleApp/
  dotnet run -- -p 8080
  ```

  This will start a server listening on port 8080.

* Signup users on server

  Command: `user signup <username> <password>`

  ```
  > user signup user1 password
  > user signup user2 password
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

* Enter a chatroom

  Command: `room <id>`

  ```
  > room 1
  ```

  Chatroom 1 is a global room that contains all users.

* Send message

  Command: `send <text>`

  ```
  Room 1 > send Hello!
  ```

  No accident, you will receive the following message on both clients.

  ```
  [Room 1 User 1] Hello!
  ```
  You can use `q` or `exit` to leave the room.

* Create chatroom on server

  Command: `room new [-p PeopleId1,PeopleId2,...]`

  ```
  > room new -p 1
  ```

  This will create a chatroom with user 1. It will return:

  ```
  New chatroom 2.
  ```

  You can try to enter room 2 and send a message again in client 1. At this time, only client 1 will receive the message.

That's all. 😂 More features will be added in the future.

## Documents

* [Release Note](./docs/Note.md)