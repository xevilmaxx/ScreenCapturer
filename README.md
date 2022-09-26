# ScreenCapturer
Cross Plaform Linux / Windows Screenshoter in C# NET 6.0

## Usage Sample:

```csharp
var platform = new Factory().GetPlatform();
var screenshot = platform.TakeScreenshot();
```

Typically this internally will persist on local file (for debugging purposes only) [part than can be easily commented]

And returns byte[] of JPEG image (format of image is also not hard to change)
