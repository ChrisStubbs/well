namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;

    public class DeliveryLineToJobDetailMapper : IDeliveryLineToJobDetailMapper
    {
        public void Map(DeliveryLineModel from, JobDetail to)
        {
            to.ShortQty = from.ShortQuantity;
            to.JobDetailReasonId = from.JobDetailReasonId;
            to.JobDetailSourceId = from.JobDetailSourceId;
            to.ShortsActionId = from.ShortsActionId;

            var damages = new List<JobDetailDamage>();

            foreach (var damageUpdateModel in from.Damages)
            {
                var damage = new JobDetailDamage
                {
                    JobDetailReasonId = damageUpdateModel.JobDetailReasonId,
                    JobDetailSourceId = damageUpdateModel.JobDetailSourceId,
                    DamageActionId = damageUpdateModel.DamageActionId,
                    JobDetailId = to.Id,
                    Qty = damageUpdateModel.Quantity,
                    DamageStatus = damageUpdateModel.Quantity > 0 ? JobDetailStatus.UnRes : JobDetailStatus.Res
                };
                damages.Add(damage);
            }

            to.JobDetailDamages = damages;
        }
    }
}