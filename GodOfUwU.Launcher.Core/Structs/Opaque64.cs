namespace GodOfUwU.Launcher.Core.Structs
{
    using GodOfUwU.Launcher.Core.Protocol;
    using System;
    using System.Buffers.Binary;

    public struct Opaque64 : IPackageData
    {
        public byte[] Value;

        public int Read(Span<byte> source)
        {
            int len = BinaryPrimitives.ReadInt32LittleEndian(source);
            Value = new byte[len];
            source.Slice(4, len).CopyTo(Value);
            return 4 + Value.Length;
        }

        public int Size()
        {
            return 4 + Value?.Length ?? 0;
        }

        public int Write(Span<byte> destination)
        {
            BinaryPrimitives.WriteInt64LittleEndian(destination, Value.Length);
            Value.CopyTo(destination[4..]);
            return 4 + Value.Length;
        }

        public static implicit operator byte[](Opaque64 opaque)
        {
            return opaque.Value;
        }
    }
}