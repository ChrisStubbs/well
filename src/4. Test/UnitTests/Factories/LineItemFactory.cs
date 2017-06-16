using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.UnitTests.Factories
{
    class LineItemFactory : EntityFactory<LineItemFactory, LineItem>
    {
        public LineItemFactory()
        {
            this.Entity.Id = 1;
        }

        public LineItemFactory AddCreditAction()
        {
            this.Entity.LineItemActions.Add(new LineItemAction
            {
                DeliveryAction = Well.Domain.Enums.DeliveryAction.Credit,
                Quantity = 10
            });

            return this;
        }

        public LineItemFactory AddCloseAction()
        {
            this.Entity.LineItemActions.Add(new LineItemAction
            {
                DeliveryAction = Well.Domain.Enums.DeliveryAction.Close,
                Quantity = 0
            });

            return this;
        }

        public LineItemFactory AddNotDefinedAction()
        {
            this.Entity.LineItemActions.Add(new LineItemAction
            {
                DeliveryAction = Well.Domain.Enums.DeliveryAction.NotDefined,
                Quantity = 10
            });

            return this;
        }
    }
}
