namespace GodOfUwU.Launcher.Core.Protocol.Records
{
    using System;
    using System.Buffers.Binary;

    public struct Warning : IRecord
    {
        public RecordType Type => RecordType.ProtocolWarning;

        public ProtocolWarning ProtocolWarning;

        public int Read(Span<byte> source)
        {
            ProtocolWarning = (ProtocolWarning)BinaryPrimitives.ReadUInt32LittleEndian(source);
            return 4;
        }

        public int Size()
        {
            return 4;
        }

        public int Write(Span<byte> destination)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(destination, (uint)ProtocolWarning);
            return 4;
        }
    }
}