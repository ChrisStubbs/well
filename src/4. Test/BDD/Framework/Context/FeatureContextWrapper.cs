namespace PH.Well.BDD.Framework.Context
{
    using TechTalk.SpecFlow;

    public class FeatureContextWrapper
    {
        public static T GetContextObject<T>()
        {
            var key = typeof(T).FullName;

            return GetContextObject<T>(key);
        }

        public static T GetContextObject<T>(string name)
        {
            if (FeatureContext.Current.ContainsKey(name))
            {
                return (T)FeatureContext.Current[name];
            }

            return default(T);
        }

        public static T SetContextObject<T>(string name, T value)
        {
            if (FeatureContext.Current.ContainsKey(name))
            {
                FeatureContext.Current.Remove(name);
            }

            FeatureContext.Current.Add(name, value);

            return value;
        }

        public static void DeleteContextObject(string name)
        {
            if (FeatureContext.Current.ContainsKey(name))
            {
                FeatureContext.Current.Remove(name);
            }
        }
    }
}