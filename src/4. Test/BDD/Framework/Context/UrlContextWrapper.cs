namespace PH.Well.BDD.Framework.Context
{
    public class UrlContext
    {
        public static string CurrentUrl
        {
            get
            {
                return ScenarioContextWrapper.GetContextObject<string>(ContextDescriptors.Url);
            }
            set
            {
                ScenarioContextWrapper.SetContextObject(ContextDescriptors.Url, value);
            }
        }
    }
}