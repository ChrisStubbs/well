namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Transactions;
    using System.Web.Http;
    using Common.Contracts;
    using Domain;
    using Domain.Enums;
    using Models;
    using Repositories.Contracts;

    public class DeliveryLineController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepo jobDetailDamageRepo;
        private readonly IJobRepository jobRepo;

        public DeliveryLineController(
            ILogger logger,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepo jobDetailDamageRepo,
            IJobRepository jobRepo)
        {
            this.logger = logger;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepo = jobDetailDamageRepo;
            this.jobRepo = jobRepo;
            this.jobDetailRepository.CurrentUser = UserName;
            this.jobDetailDamageRepo.CurrentUser = UserName;
            this.jobRepo.CurrentUser = UserName;
        }

        [HttpPut]
        [Route("delivery-line")]
        public HttpResponseMessage Update(DeliveryLineModel model)
        {
            try
            {
                IEnumerable<JobDetail> jobDetails = jobDetailRepository.GetByJobId(model.JobId);
                bool isCleanBeforeUpdate = jobDetails.All(jd => jd.IsClean());

                JobDetail jobDetail = jobDetails.SingleOrDefault(j => j.LineNumber == model.LineNo);
                if (jobDetail == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        Message = "Unable to update delivery line",
                        Errors = new List<string>()
                        {
                            $"No matching delivery line found for JobId: {model.JobId}, LineNumber: {model.LineNo}."
                        }
                    });
                }

                jobDetail.ShortQty = model.ShortQuantity;

                var damages = new Collection<JobDetailDamage>();
                foreach (var damageUpdateModel in model.Damages)
                {
                    var reasonCode = (DamageReasons) Enum.Parse(typeof(DamageReasons), damageUpdateModel.ReasonCode);
                    var damage = new JobDetailDamage
                    {
                        DamageReason = reasonCode,
                        JobDetailId = jobDetail.Id,
                        Qty = damageUpdateModel.Quantity
                    };
                    damages.Add(damage);
                }
                jobDetail.JobDetailDamages = damages;

                using (var transactionScope = new TransactionScope())
                {
                    jobDetailRepository.Update(jobDetail);
                    jobDetailDamageRepo.Delete(jobDetail.Id);
                    foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                    {
                        jobDetailDamageRepo.Save(jobDetailDamage);
                    }

                    bool isClean = jobDetails.All(jd => jd.IsClean());
                    if (isCleanBeforeUpdate && isClean == false)
                    {
                        //Make dirty
                        Job job = jobRepo.GetById(model.JobId);
                        job.PerformanceStatusId = (int) PerformanceStatus.Incom;
                        jobRepo.JobCreateOrUpdate(job);
                    }

                    if (isCleanBeforeUpdate == false && isClean)
                    {
                        //Resolve
                        Job job = jobRepo.GetById(model.JobId);
                        job.PerformanceStatusId = (int)PerformanceStatus.Resolved;
                        jobRepo.JobCreateOrUpdate(job);
                    }

                    transactionScope.Complete();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occcured when updating DeliveryLine", ex);
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}