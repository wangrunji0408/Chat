##开发文档 Note

###v0.4

#### Target

服务端领域类及持久化存储

* 持久化：EntityFrameworkCore + SQLite
* 配置：Microsoft.Extensions.Configuration
* 日志文件：NLog

### v0.3

完成时间：2017.08.28。累计时间：19h

#### Target

客户端与网络测试

* Console，~~MacApp~~
* 局域网，~~外网测试~~

#### Problem

* Mac客户端无法正常工作。gRPC库不完全支持Xamarin.Mac。
* 技术所限，尚未进行外网测试。

### v0.2

完成时间：2017.08.24。累计时间：14h

#### Target

gRPC通信

#### Implement Details

* Client/Server引入工厂模式。负责注入服务（Log），设置连接方式。
* 连接由单独的模块实现。目前实现了Local（直接访问本地对象），Grpc。
* 工厂设置连接的工作委托给抽象类Client/ServiceConnectionBuilder，具体的连接模块（Local/Grpc）实现该类，可以定制C/S对象创建前后的操作。
  * 例如：Grpc的Client端，需要在Client构造前，创建Server服务的客户端，在构造后，创建Client服务的服务端，并使前者引用后者，以便在登陆时告知服务器自己的监听地址。

### v0.1

完成时间：2017.08.22。累计时间：7h

#### Target

最基础通信功能的服务端和客户端。

- 客户端
  - 登陆
  - 发送消息
  - 接收消息
- 服务端
  - 响应客户端的登陆请求
  - 接收消息并向所有在线用户转发
- 不需要……
  - 只有一个聊天室
  - 只支持文本消息
  - C/S使用对象直接通信，不使用网络