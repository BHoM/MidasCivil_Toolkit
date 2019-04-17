using System.IO;
using System;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static List<string> ToMCBarRelease(this BarRelease release)
        {
            List<string> midasRelease = new List<string>();

            string startFixity = boolToConstraint(release.StartRelease.TranslationX) +
                            boolToConstraint(release.StartRelease.TranslationY) +
                            boolToConstraint(release.StartRelease.TranslationZ) +
                            boolToConstraint(release.StartRelease.RotationX) +
                            boolToConstraint(release.StartRelease.RotationY) +
                            boolToConstraint(release.StartRelease.RotationZ);

            string endFixity = boolToConstraint(release.EndRelease.TranslationX) +
                boolToConstraint(release.EndRelease.TranslationY) +
                boolToConstraint(release.EndRelease.TranslationZ) +
                boolToConstraint(release.EndRelease.RotationX) +
                boolToConstraint(release.EndRelease.RotationY) +
                boolToConstraint(release.EndRelease.RotationZ);

            midasRelease.Add("," + startFixity + ",0,0,0,0,0,0");
            midasRelease.Add(endFixity + ",0,0,0,0,0,0," + release.Name);

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