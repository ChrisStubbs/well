

namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Mapper.Contracts;
    using Models;
    using Common.Contracts;
    using Repositories.Contracts;
    using Services.Contracts;

    public class CreditThresholdController : BaseApiController
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;
        private readonly ILogger logger;
        private readonly ICreditThresholdMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserThresholdService userThresholdService;

        public CreditThresholdController(
            ICreditThresholdRepository creditThresholdRepository,
            ILogger logger,
            ICreditThresholdMapper mapper,
            IUserRepository userRepository,
            IUserNameProvider userNameProvider,
            IUserThresholdService userThresholdService)
            : base(userNameProvider)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.userThresholdService = userThresholdService;
        }

        [Route("{branchId:int}/credit-threshold")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var creditThresholds = this.creditThresholdRepository.GetAll().ToList();

            var model = new List<CreditThresholdModel>();

            foreach (var creditThreshold in creditThresholds)
            {
                var mappedModel = this.mapper.Map(creditThreshold);

                model.Add(mappedModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("{branchId:int}/credit-threshold/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            this.creditThresholdRepository.Delete(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });

        }

        [Route("{branchId:int}/credit-threshold/{isUpdate:bool}")]
        [HttpPost]
        public HttpResponseMessage Post(CreditThresholdModel model, bool isUpdate)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var creditThreshold = this.mapper.Map(model);

            if (isUpdate)
            {
                creditThresholdRepository.Update(creditThreshold);
            }
            else
            {
                creditThresholdRepository.Save(creditThreshold);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });

        }

        [Route("{branchId:int}/credit-threshold/getByUser")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetByUser()
        {
            var threshold = userThresholdService.GetCreditThreshold(this.UserIdentityName);
            return this.Request.CreateResponse(HttpStatusCode.OK, threshold);

        }
    }
}