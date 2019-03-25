namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static string SectionName(string text)
        {
            if(text.Contains(","))
            {
                return text.Split(',')[0].Split('*')[1];
            }
            else if(text.Contains(" "))
            {
                return text.Split(' ')[0].Split('*')[1];
            }
            else
            {
                return text.Split('*')[1];
            }
        }
    }
}