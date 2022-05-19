namespace GodOfUwU;

public class Program
{
    public static async Task Main()
    {
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        File.Create("lock").Close();
        await new GodUwUClient().InitializeAsync();
    }

    private static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        File.Delete("lock");
    }
}