namespace PH.Well.Dashboard.Controllers
{
    using System.Web.Mvc;

    using PH.Well.Common.Contracts;

    public class BranchController : BaseController
    {
        public BranchController(IWebClient webClient)
            : base(webClient)
        {
        }

        public override ActionResult Index()
        {
            return this.View("Index", this.Model);
        }

        [HttpGet]
        [Route("user-preferences/branches/{username}/{domain}")]
        public ActionResult BranchesOnBehalfOfUser(string username, string domain)
        {
            this.Model.ConfigDictionary.Add("username", username);
            this.Model.ConfigDictionary.Add("domain", domain);
            return this.View("Index", this.Model);
        }
    }
}