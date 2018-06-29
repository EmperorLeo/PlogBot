using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PlogBot.Configuration;
using PlogBot.Data;
using PlogBot.Services.Extensions;
using PlogBot.Services.Interfaces;

namespace PlogBot.Alerts
{
    public class AlertsProcessor
    {
        private readonly IAlertService _alertService;
        private readonly ILoggingService _loggingService;
        private readonly PlogDbContext _plogDbContext;

        public AlertsProcessor(IAlertService alertService, ILoggingService loggingService, PlogDbContext plogDbContext)
        {
            _alertService = alertService;
            _loggingService = loggingService;
            _plogDbContext = plogDbContext;
        }

        public async Task Process()
        {
            var alerts = await _alertService.GetReadyAlerts();
            var tasks = alerts.Select(a => _alertService.BlastAlert(a.Name, a.Description, a.Time, a.Roles.GetULongs(), a.ChannelId));
            await Task.WhenAll(tasks);
            var processedTime = DateTime.UtcNow;
            alerts.ForEach(a => a.LastProcessed = processedTime);
            _plogDbContext.UpdateRange(alerts);
            await _plogDbContext.SaveChangesAsync();
        }
    }
}