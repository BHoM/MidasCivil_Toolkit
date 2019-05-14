using BH.oM.Structure.Constraints;

namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static bool SupportedDOFTypes(DOFType freedom)
        {
            if (freedom == DOFType.Damped)
            {
                Reflection.Compute.RecordWarning(DOFType.Damped.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.FixedNegative)
            {
                Reflection.Compute.RecordWarning(DOFType.FixedNegative.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.FixedPositive)
            {
                Reflection.Compute.RecordWarning(DOFType.FixedPositive.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.Friction)
            {
                Reflection.Compute.RecordWarning(DOFType.Friction.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.Gap)
            {
                Reflection.Compute.RecordWarning(DOFType.Gap.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.NonLinear)
            {
                Reflection.Compute.RecordWarning(DOFType.NonLinear.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.SpringNegative)
            {
                Reflection.Compute.RecordWarning(DOFType.SpringNegative.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.SpringPositive)
            {
                Reflection.Compute.RecordWarning(DOFType.SpringPositive.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.SpringRelativeNegative)
            {
                Reflection.Compute.RecordWarning(DOFType.SpringRelativeNegative.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }
            else if (freedom == DOFType.SpringRelativePositive)
            {
                Reflection.Compute.RecordWarning(DOFType.SpringRelativePositive.ToString() + "not supported in MidasCivil_Toolkit");
                return true;
            }

            return false;
        }
    }
}