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
            foreach (var order in route.Stops)
            {
                var action = GetOrderUpdateAction(order.ActionIndicator);

                switch (action)
                {
                    case OrderActionIndicator.Insert:
                        this.Insert(order);
                        break;
                    case OrderActionIndicator.Update:
                        this.Update(order);
                        break;
                    case OrderActionIndicator.Delete:
                        this.Delete(order);
                        break;
                }
            }
        }

        private void Insert(StopUpdate stopUpdate)
        {
        }

        private void Update(StopUpdate stopUpdate)
        {
        }

        private void Delete(StopUpdate stopUpdate)
        {
        }

        private static OrderActionIndicator GetOrderUpdateAction(string actionIndicator)
        {
            return string.IsNullOrWhiteSpace(actionIndicator) ? OrderActionIndicator.Update : StringExtensions.GetValueFromDescription<OrderActionIndicator>(actionIndicator);
        }
    }
}