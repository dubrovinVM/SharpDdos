namespace SharpDdos
{
    [Flags]
    public enum Method
    {
        udp = 1,
        http,
        tcp,
        https
    }
}
