namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;

    public class ThresholdLevelController : BaseApiController
    {
        private readonly IUserRepository userRepository;

        private readonly ILogger logger;

        public ThresholdLevelController(IUserRepository userRepository, ILogger logger, IUserNameProvider userNameProvider):
            base(userNameProvider)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [Route("threshold-level")]
        [HttpPost]
        public HttpResponseMessage Post(string threshold, string username)
        {
            try
            {
                var user = this.userRepository.GetByName(username);

                if (user == null) throw new UserNotFoundException();

                var thresholdLevel = EnumExtensions.GetValueFromDescription<ThresholdLevel>(threshold);

                this.userRepository.SetThresholdLevel(user, thresholdLevel);

                return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
            }
            catch (UserNotFoundException)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true, message = $"{username} does not exist please set the user up via 'User Branch Preferences'" });
            }
            catch (Exception exception)
            {
                this.logger.LogError(
                    $"Error occured when trying to save a threshold level ({threshold}) for user ({username})",
                    exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }

        }
    }
}

