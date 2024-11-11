using Foundation;

namespace MAUIApi;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
  protected override MauiApp CreateMauiApp()
  {
    return MauiProgram.CreateMauiApp();
  }
}