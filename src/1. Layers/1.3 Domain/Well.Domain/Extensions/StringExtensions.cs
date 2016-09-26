namespace PH.Well.Domain.Extensions
{
    using System.Text;

    public static class StringExtensions
    {
        public static string StripSpaces(this string value)
        {
            return (value ?? string.Empty).Replace(" ", string.Empty);
        }

        public static string SplitCapitalisedWords(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var newText = new StringBuilder(source.Length * 2);
            newText.Append(source[0]);

            for (var i = 1; i < source.Length; i++)
            {
                if (char.IsUpper(source[i]) && i + 1 < source.Length && !char.IsUpper(source[i + 1]))
                {
                    newText.Append(' ');
                }

                newText.Append(source[i]);
            }

            return newText.ToString();
        }
    }
}
