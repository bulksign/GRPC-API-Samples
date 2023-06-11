using Google.Protobuf;

namespace GrpcApiSamples;

public static class ConversionUtilities
{
    public static ByteString ConvertToByteString(byte[] bytes)
    {
        return ByteString.CopyFrom(bytes);
    }
    
    
    public static byte[] ConvertToByteArray(ByteString b)
    {
        byte[] array = new byte[b.Length];
        b.CopyTo(array, 0);
        return array;
    }

}

