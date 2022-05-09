namespace GodOfUwU.Launcher.Core.Protocol
{
    public interface IRecord : IPackageData
    {
        public RecordType Type { get; }
    }
}