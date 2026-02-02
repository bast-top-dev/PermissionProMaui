using System;
using Android.App;
using Android.Runtime;
using Microsoft.Maui;

namespace PermissionProMaui;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp()
    {
        System.Diagnostics.Debug.WriteLine("MainApplication.CreateMauiApp invoked");
        return MauiProgram.CreateMauiApp();
    }
}


