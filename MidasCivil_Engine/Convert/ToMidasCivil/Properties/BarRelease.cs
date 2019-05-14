using BH.oM.Structure.Properties.Constraint;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static List<string> ToMCBarRelease(this BarRelease barRelease)
        {
            List<string> midasRelease = new List<string>();

            string startFixity = boolToConstraint(barRelease.StartRelease.TranslationX) +
                                    boolToConstraint(barRelease.StartRelease.TranslationY) +
                                    boolToConstraint(barRelease.StartRelease.TranslationZ) +
                                    boolToConstraint(barRelease.StartRelease.RotationX) +
                                    boolToConstraint(barRelease.StartRelease.RotationY) +
                                    boolToConstraint(barRelease.StartRelease.RotationZ);

            string endFixity = boolToConstraint(barRelease.EndRelease.TranslationX) +
                                    boolToConstraint(barRelease.EndRelease.TranslationY) +
                                    boolToConstraint(barRelease.EndRelease.TranslationZ) +
                                    boolToConstraint(barRelease.EndRelease.RotationX) +
                                    boolToConstraint(barRelease.EndRelease.RotationY) +
                                    boolToConstraint(barRelease.EndRelease.RotationZ);

            midasRelease.Add(",NO," + startFixity + ",0,0,0,0,0,0");
            midasRelease.Add(endFixity + ",0,0,0,0,0,0," + barRelease.Name);

            return midasRelease;
        }

        public static string boolToConstraint(DOFType fixity)
        {
            string converted = "0";

            if (fixity == DOFType.Free)
            {
                converted = "1";
            }

            return converted;
        }
    }
}