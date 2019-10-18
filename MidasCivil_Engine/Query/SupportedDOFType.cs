using BH.oM.Structure.Constraints;

namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static bool SupportedDOFTypes(DOFType freedom)
        {
            switch(freedom)
            {
                case DOFType.Free:
                case DOFType.Fixed:
                case DOFType.Spring:
                    return true;
                default:
                    Reflection.Compute.RecordWarning(freedom.ToString() + " not supported in MidasCivil_Toolkit");
                    return false;
            }
        }
    }
}