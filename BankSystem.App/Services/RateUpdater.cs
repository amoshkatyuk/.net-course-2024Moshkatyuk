using BankSystem.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BankSystem.App.Services
{
    public class RateUpdater
    {
        private readonly IClientStorage _clientStorage;
        private readonly decimal _percentage;
        private readonly TimeSpan _interval;
        private CancellationTokenSource _cancellationTokenSourse;
        private bool _disposed;

        public RateUpdater(IClientStorage clientStorage, decimal percentage, TimeSpan interval)
        {
            _clientStorage = clientStorage;
            _percentage = percentage;
            _interval = interval;
            _cancellationTokenSourse = new CancellationTokenSource();
        }

        public void Start() 
        {
            Task.Run(async () => await RunPeriodicAmountUpdate(_cancellationTokenSourse.Token));
        }

        private async Task RunPeriodicAmountUpdate(CancellationToken cancellationToken) 
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, cancellationToken);

                var clients = await _clientStorage.GetAsync(c => true, cancellationToken);

                foreach (var client in clients)
                {
                    foreach (var account in client.Accounts)
                    {
                        if (account.LastUpdate == null || account.LastUpdate.Value.AddMonths(1) <= DateTime.UtcNow)
                        {
                            account.Amount += account.Amount * (_percentage / 100m);
                            account.LastUpdate = DateTime.UtcNow;
                        }
                    }

                    await _clientStorage.UpdateAsync(client.Id, client, cancellationToken);
                }
            }
        }

        public void Stop() 
        {
            _cancellationTokenSourse.Cancel();
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) 
            {
                return;
            }

            if (disposing) 
            {
                _cancellationTokenSourse?.Dispose();
            }

            _disposed = true;
        }
    }
}
