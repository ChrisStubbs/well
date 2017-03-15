using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Steps
{
    using System.Net;
    using System.Security.Principal;
    using Api.Models;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Framework.Context;
    using Helpers;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using StructureMap;
    using Configuration = Framework.Configuration;

    public class SetupDeliveryLineUpdate
    {
        private IWebClientHelper webClientHelper;
        private IJobRepository jobRepository;
        private IUserRepository userRepository;

        public SetupDeliveryLineUpdate()
        {
            var container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);

            webClientHelper = container.GetInstance<IWebClientHelper>();
            jobRepository = container.GetInstance<IJobRepository>();
            userRepository = container.GetInstance<IUserRepository>();
        }


        public void SetDeliveriesToAction(int noOfDeliveries, bool confirmLines, DeliveryAction action)
        {
            string userIdentity = WindowsIdentity.GetCurrent().Name;
            var user = userRepository.GetByIdentity(userIdentity);


            for (int jobId = 1; jobId <= noOfDeliveries; jobId++)
            {
                //assign user to job
                var userJob = new UserJob()
                {
                    UserId = user.Id,
                    JobId = jobId
                };
                var res = webClientHelper.Post($"{Configuration.WellApiUrl}assign-user-to-job",
                    JsonConvert.SerializeObject(userJob));
                var codes = new List<HttpStatusCode>() {HttpStatusCode.Created, HttpStatusCode.OK};
                Assert.IsTrue(codes.Contains(webClientHelper.HttpWebResponse.StatusCode),
                    $"Unable to assign user to job, response: {res}");

                var job = jobRepository.GetById(jobId);

                foreach (var jobDetail in job.JobDetails)
                {
                    SetDeliveryLineActionToAction(jobId, jobDetail.LineNumber, action);
                }

                if (confirmLines)
                {
                    var confirmAddress = $"{Configuration.WellApiUrl}confirm-delivery-lines/{jobId}";
                    var response2 = webClientHelper.Post(confirmAddress, "");

                    Assert.AreEqual(HttpStatusCode.OK, webClientHelper.HttpWebResponse.StatusCode,
                        $"Unable to confirm complete action, response: {response2}");
                }
            }
        }

        public void SetDeliveryLineActionToAction(int jobId, int lineNo, DeliveryAction action)
        {
            var deliverylineUpdate = new DeliveryLineModel()
            {
                JobId = jobId,
                LineNo = lineNo,
                ShortsActionId = (int)action,
                ShortQuantity = 1
            };
            var address = $"{Configuration.WellApiUrl}DeliveryLine";
            var data = JsonConvert.SerializeObject(deliverylineUpdate);
            var response = webClientHelper.Put(address, data);

            Assert.AreEqual(HttpStatusCode.OK, webClientHelper.HttpWebResponse.StatusCode,
                $"Unable to set delivery to {action}, response: {response}");
        }
    }
}
