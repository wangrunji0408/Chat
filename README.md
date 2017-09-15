# Chat

A chat client/server in .NetCore/C#, using DDD.

## Basic

* Platform: .Net Core 2.0
* Dependencies:
  * Communication: `gRPC` + `Protobuf`
  * Persistance: `EntityFrameworkCore`
  * ObjectMapping: `AutoMapper`
  * EventBus: `System.Reactive`
  * Identity: `Microsoft.AspNetCore.Identity`
  * Logging: `Microsoft.Extensions.Logging` +` NLog`
  * DI: `Microsoft.Extensions.DependencyInjection`
  * CommandParsing: `CommandLineParser`
  * Testing: `xUnit`

## Usage

### Basic

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

  Command: `login <username> <password>`

  ```
  > login user1 password
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

### More Commands

#### Server

* User

  ```
  # Signup
  > user signup <username> <password>

  # Get info
  > user info [userid]

  # Make friend
  > user friend <user1id> <user2id>
  ```

* Chatroom

  ```
  # New
  > room new [-p peopleid1,id2,...]

  # Get info
  > room info [roomid]

  # Get message
  > room message <roomid> [-c count =10]

  # Add people
  > room add <roomid> <-p peopleid1,id2,...>

  # Remove people
  > room remove <roomid> <-p peopleid1,id2,...>

  # Set role
  > room setrole <roomid> <peopleid> <rolename>
    # rolename = {Normal, Admin}
  ```

#### Client

* Login/Signup

  ```
  signup <username> <password>
  login <username> <password>
  ```

*The following commands are available only after login.*

* People

  ```
  > people <peopleid>
  ```

  Then, it will be like:  `People 1 >`. Enter `q` or `exit` to return.

  Subcommands:

  ```
  People 1 > info
  People 1 > makefriend
  ```

* Chatroom

  ```
  > room <roomid>
  ```

  Then, it will be like:  `Room 1 >`.  Enter `q` or `exit` to return.

  Subcommands:

  ```
  Room 1 > send <text>
  Room 1 > add <-p peopleid,>
  Room 1 > remove <-p peopleid,>
  Room 1 > new [-p peopleid,]
  Room 1 > dismiss
  Room 1 > info
  Room 1 > message [-c count =10]
  ```

## Documents

* [Release Note](./docs/Note.md)