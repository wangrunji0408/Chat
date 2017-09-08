using Xunit;

namespace Chat.Test.Client
{
    public class TestLoginLocal : TestLogin<LocalSetup>
    {
    }

    [Collection("GrpcLocal")]
    public class TestLoginGrpcLocal : TestLogin<GrpcLocalSetup>
    {
    }

    public class TestRoomLocal : TestRoom<LocalSetup>
    {
    }
    
    [Collection("GrpcLocal")]
    public class TestRoomGrpcLocal : TestRoom<GrpcLocalSetup>
    {
    }

    public class TestMakeFriendLocal : TestMakeFriend<LocalSetup>
    {
    }

    [Collection("GrpcLocal")]
    public class TestMakeFriendGrpcLocal : TestMakeFriend<GrpcLocalSetup>
    {
    }

    public class TestGetInfoLocal : TestGetInfo<LocalSetup>
    {
    }

    [Collection("GrpcLocal")]
    public class TestGetInfoGrpcLocal : TestGetInfo<GrpcLocalSetup>
    {
    }
    
    public class TestGetDataLocal : TestGetData<LocalSetup>
    {
    }
    
    [Collection("GrpcLocal")]
    public class TestGetDataGrpcLocal : TestGetData<GrpcLocalSetup>
    {
    }
}