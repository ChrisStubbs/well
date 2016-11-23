namespace PH.Well.Services.EpodServices
{
    using PH.Well.Common.Extensions;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;

    public class AdamUpdateService : IAdamUpdateService
    {
        public void Update(RouteUpdates route)
        {
            foreach (var stop in route.Stops)
            {
                var action = GetOrderUpdateAction(stop.ActionIndicator);

                switch (action)
                {
                    case OrderActionIndicator.Insert:
                        this.Insert(stop);
                        break;
                    case OrderActionIndicator.Update:
                        this.Update(stop);
                        break;
                    case OrderActionIndicator.Delete:
                        this.Delete(stop);
                        break;
                }
            }
        }

        private void Insert(StopUpdate stop)
        {
        }

        private void Update(StopUpdate stop)
        {
        }

        private void Delete(StopUpdate stop)
        {
        }

        private static OrderActionIndicator GetOrderUpdateAction(string actionIndicator)
        {
            return string.IsNullOrWhiteSpace(actionIndicator) ? OrderActionIndicator.Update : StringExtensions.GetValueFromDescription<OrderActionIndicator>(actionIndicator);
        }
    }
}