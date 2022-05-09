namespace GodOfUwU.Launcher.Core.Protocol.Records
{
    using GodOfUwU.Launcher.Core.Structs;
    using System;

    public struct ApplicationData : IRecord
    {
        public RecordType Type => RecordType.ApplicationData;

        public Opaque64 Data;

        public int Read(Span<byte> source)
        {
            return Data.Read(source);
        }

        public int Write(Span<byte> destination)
        {
            return Data.Write(destination);
        }

        public int Size()
        {
            return Data.Size();
        }
    }
}