using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestRapidPay.Services
{
    public sealed class UniversalFeesExchange
    {
        private static UniversalFeesExchange instance = new UniversalFeesExchange();
        private decimal currentFee;

        private UniversalFeesExchange()
        {
            
            currentFee = 1.00m;

            // Start a timer to update the fee every hour
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromHours(1));
                    UpdateFee();
                }
            });
        }

        public static UniversalFeesExchange Instance => instance;

        public decimal GetCurrentFee() => currentFee;

        private void UpdateFee()
        {
            Random random = new Random();
            // Alternative approach for decimal generation:
            decimal newFactor = random.Next(0, 21) / 10m;
            currentFee = currentFee * newFactor;
        }
    }
}
