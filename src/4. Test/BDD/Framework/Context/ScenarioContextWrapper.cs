namespace PH.Well.BDD.Framework.Context
{
    using TechTalk.SpecFlow;

    public class ScenarioContextWrapper
    {
        public static T GetContextObject<T>()
        {
            var key = typeof(T).FullName;

            return GetContextObject<T>(key);
        }

        public static T GetContextObject<T>(string name)
        {
            if (ScenarioContext.Current.ContainsKey(name))
            {
                return (T)ScenarioContext.Current[name];
            }

            return default(T);
        }

        public static T SetContextObject<T>(string name, T value)
        {
            if (ScenarioContext.Current.ContainsKey(name))
            {
                ScenarioContext.Current.Remove(name);
            }

            ScenarioContext.Current.Add(name, value);

            return value;
        }

        public static void DeleteContextObject(string name)
        {
            if (ScenarioContext.Current.ContainsKey(name))
            {
                ScenarioContext.Current.Remove(name);
            }
        }
    }
}