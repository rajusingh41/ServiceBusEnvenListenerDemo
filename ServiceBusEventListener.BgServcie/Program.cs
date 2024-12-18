using ServiceBusEventListener.BgServcie;
using ServiceBusEventListener.BgServcie.Infrastructure;
using ServiceBusEventListener.BgServcie.Service;
using ServiceBusEventListener.BgServcie.TcpConnectionService;

IHost host = Host.CreateDefaultBuilder(args)
    //.ConfigureWebHostingDefaults(webBuilder =>
    //{
    //    webBuilder.UseStartup<Startup>();
    //})
    .ConfigureServices(services =>
    {
        services.AddHostedService<ServiceBusTopicListener>();
        services.AddHostedService<TcpHealthProbeService>();
    })
    .ConfigureServices(DependencyRegistration.ConfigureStaticDependencies)
    .ConfigureServices(DependencyRegistration.ConfigureProductionDependencies)
    .Build();

host.Run();
