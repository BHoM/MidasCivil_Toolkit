namespace BH.Engine.MidasCivil
{
    public partial class Convert
    {
        private static bool Fixity(string number)
        {
            bool fixity = true;

            if (int.Parse(number)==1)
            {
                fixity = false;
            }

            return fixity;
        }

    }
}
