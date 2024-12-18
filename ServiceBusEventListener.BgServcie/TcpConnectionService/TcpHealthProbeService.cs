using Microsoft.Extensions.Diagnostics.HealthChecks;
using ServiceBusEventListenerDemo.Model.Configuration;
using System.Net;
using System.Net.Sockets;

namespace ServiceBusEventListener.BgServcie.TcpConnectionService
{
    public class TcpHealthProbeService : BackgroundService
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly TcpListener _tcpListener;
        private readonly ILogger<TcpHealthProbeService> _logger;
        private readonly BGServiceSettings _bGServiceSettings;

        public TcpHealthProbeService(HealthCheckService healthCheckService,
            TcpListener tcpListener,
            ILogger<TcpHealthProbeService> logger,
            BGServiceSettings bGServiceSettings)
        {
            _healthCheckService = healthCheckService;

            _logger = logger;
            _bGServiceSettings = bGServiceSettings;
            _tcpListener = new TcpListener(IPAddress.Any, bGServiceSettings.HealthCheckPort);

        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ListenerHandler(stoppingToken);
        }

        private async Task ListenerHandler(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ServiceBustEventListener - TcpHealthProbeService - ExecuteAsync - Start");

            await Task.Yield();

            _tcpListener.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateHeartbeatAsync(stoppingToken);
                _logger.LogInformation("ServiceBustEventListener - TcpHealthProbeService - Heartbeat check executed");
                Thread.Sleep(TimeSpan.FromSeconds(_bGServiceSettings.HealthCheckFrequencySeconds));
            }
            _tcpListener.Stop();
            _logger.LogInformation("ServiceBustEventListener - TcpHealthProbeService - ExecuteAsync - End");
        }

        private async Task UpdateHeartbeatAsync(CancellationToken token)
        {
            try
            {
                var result = await _healthCheckService.CheckHealthAsync(token);
                var isHealthy = result.Status == HealthStatus.Healthy;

                if (!isHealthy)
                {
                    _tcpListener.Stop();
                    _logger.LogError("ServiceBustEventListener - TcpHealthProbeService - Unhealthy - Listener stopped.");
                    return;
                }

                while(_tcpListener.Server.IsBound && _tcpListener.Pending())
                {
                    var client = await _tcpListener.AcceptTcpClientAsync();
                    client.Close();
                    _logger.LogInformation("ServiceBustEventListener - TcpHealthProbeService - Successfully processed health check request.");
                }

                _tcpListener.Start();
                _logger.LogInformation("ServiceBustEventListener - TcpHealthProbeService - Healthy - Listener started.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "ServiceBustEventListener - TcpHealthProbeService - An error occurred while checking heartbeat.");
            }
        }
    }
}
