namespace GodOfUwU.Launcher.Core.Protocol.Records
{
    using System;
    using System.Buffers.Binary;

    public struct ClientHello : IRecord
    {
        public ProtocolVersion[] Versions;

        public RecordType Type => RecordType.ClientHello;

        public int Read(Span<byte> source)
        {
            ushort lenVersions = BinaryPrimitives.ReadUInt16LittleEndian(source);
            Versions = new ProtocolVersion[lenVersions];
            int idx = 2;
            for (int i = 0; i < lenVersions; i++)
            {
                Versions[i] = (ProtocolVersion)BinaryPrimitives.ReadUInt16LittleEndian(source[idx..]);
                idx += 2;
            }

            return idx;
        }

        public int Size()
        {
            return 2 * Versions.Length + 2;
        }

        public int Write(Span<byte> destination)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(destination, (ushort)Versions.Length);
            int idx = 2;
            for (int i = 0; i < Versions.Length; i++)
            {
                BinaryPrimitives.WriteUInt16LittleEndian(destination[idx..], (ushort)Versions[i]);
                idx += 2;
            }
            return idx;
        }
    }
}