using Google.Protobuf;

namespace GrpcApiSamples;

public static class ConversionUtilities
{
    public static ByteString ConvertoToByteString(byte[] bytes)
    {
        return ByteString.CopyFrom(bytes);
    }
}

