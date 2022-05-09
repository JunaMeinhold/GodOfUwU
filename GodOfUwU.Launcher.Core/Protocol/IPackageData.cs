namespace GodOfUwU.Launcher.Core.Protocol
{
    using System;

    public interface IPackageData
    {
        public int Read(Span<byte> source);

        public int Write(Span<byte> destination);

        public Task<int> ReadAsync(Span<byte> source)
        {
            return Task.FromResult(Read(source));
        }

        public Task WriteAsync(Span<byte> destination)
        {
            return Task.FromResult(Write(destination));
        }

        public int Size();
    }
}