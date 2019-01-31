namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public string GetSectionName(string text)
        {

            if(text.Contains(" "))
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