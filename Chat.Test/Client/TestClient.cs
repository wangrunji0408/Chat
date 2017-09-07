namespace Chat.Test.Client
{
    public class TestLoginLocal : TestLogin<LocalSetup>
    {
    }

    public class TestLoginGrpcLocal : TestLogin<GrpcLocalSetup>
    {
    }

    public class TestRoomLocal : TestRoom<LocalSetup>
    {
    }

    public class TestRoomGrpcLocal : TestRoom<GrpcLocalSetup>
    {
    }

    public class TestMakeFriendLocal : TestMakeFriend<LocalSetup>
    {
    }

    public class TestMakeFriendGrpcLocal : TestMakeFriend<GrpcLocalSetup>
    {
    }

    public class TestGetInfoLocal : TestGetInfo<LocalSetup>
    {
    }

    public class TestGetInfoGrpcLocal : TestGetInfo<GrpcLocalSetup>
    {
    }
}