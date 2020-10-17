<img alt="logo" width="150" height="150" src="nuget-logo.png">

Hto3.WindowsServiceJumpStart
========================================

#### Nuget Package
[![Hto3.WindowsServiceJumpStart](https://img.shields.io/nuget/v/Hto3.WindowsServiceJumpStart.svg)](https://www.nuget.org/packages/Hto3.WindowsServiceJumpStart/)

Features
--------
Enable Windows Service projects to run as application and be auto installable. See below the out-of-box features through a UI:
- Install
- Uninstall
- Run as application (commonly used to help you to debug your service)
  - Start
  - Stop
  - Pause
  - Continue

<img alt="logo" src="picture.png"><br>

Or run your services as application:

<img alt="logo" src="picture2.png"><br>

How to use?
-----------
After creating a new Windows Service project in Visual Studio, in your `program.cs` you will get something like it:

```C#
static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
        {
            new Service1()
        };
        ServiceBase.Run(ServicesToRun);
    }
}
```

First of all we need to configure our service, you can see all configurations available here. Let's configure the minimun necessary to get it work:

```C#
Runner.Configuration.DisplayName = "HTO3 dummy service";
Runner.Configuration.ServiceName = "HTO3Dummy";
Runner.Configuration.StartType = ServiceStartMode.Automatic;
```

Then we need to change the default engine `ServiceBase.Run()` to the Hto3.WindowsServiceJumpStart engine:

```C#
Runner.Run(ServicesToRun);
```

See below the complete code:

```C#
static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
        {
            new Service1()
        };

        //Old way
        //ServiceBase.Run(ServicesToRun);

        Runner.Configuration.DisplayName = "HTO3 dummy service";
        Runner.Configuration.ServiceName = "HTO3Dummy";
        Runner.Configuration.StartType = ServiceStartMode.Automatic;

        Runner.Run(ServicesToRun);
    }
}
```

If you want to change the the behavior of your service to run as application, just configure it:
```C#
Runner.Configuration.RunAsApplication = true;
```

The `Configuration.StartType` property can act as auto start of the application service (or no) and the `Runner.Configuration.DisplayName` property will appear as the dialog window name. Finally, you'll get the control panel as below:

<img alt="logo" src="picture2.png">