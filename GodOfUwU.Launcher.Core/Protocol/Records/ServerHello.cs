namespace GodOfUwU.Launcher.Core.Protocol.Records
{
    using System;
    using System.Buffers.Binary;

    public struct ServerHello : IRecord
    {
        public ProtocolVersion Version;

        public RecordType Type => RecordType.ServerHello;

        public int Read(Span<byte> source)
        {
            Version = (ProtocolVersion)BinaryPrimitives.ReadUInt16LittleEndian(source);
            return 2;
        }

        public int Size()
        {
            return 2;
        }

        public int Write(Span<byte> destination)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(destination, (ushort)Version);
            return 2;
        }
    }
}