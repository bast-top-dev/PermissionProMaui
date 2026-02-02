#if WINDOWS
using Microsoft.Maui.Hosting;

namespace PermissionProMaui;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        MauiProgram.CreateMauiApp();
    }
}
#endif
