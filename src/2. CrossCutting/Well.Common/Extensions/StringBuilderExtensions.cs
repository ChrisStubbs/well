namespace PH.Well.Common.Extensions
{
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static void AppendConditional(this StringBuilder entryBuilder, bool shouldAdd, string entry)
        {
            if (shouldAdd)
            {
                entryBuilder.Append(entry);
            }
        }
    }
}