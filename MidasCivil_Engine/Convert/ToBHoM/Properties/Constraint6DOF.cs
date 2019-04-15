using BH.oM.Structure.Properties.Constraint;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Constraint6DOF ToBHoMConstraint6DOF(this string support)
        {
            List<string> delimitted = support.Split(',').ToList();
            string supportName;

            List<bool> fixity = new List<bool>();
            List<double> stiffness = new List<double>();

            int constraint = 0;

            string assignment = delimitted[1].Replace(" ",string.Empty);

            if(int.TryParse(assignment, out constraint))
            {
                foreach(char freedom in assignment)
                {
                    int.Parse(freedom.ToString());
                    if (freedom == 1)
                    {
                        fixity.Add(false);
                        stiffness.Add(0.0);
                    }
                    else
                    {
                        fixity.Add(true);
                        stiffness.Add(0.0);
                    }
                }
                supportName = delimitted[2].Replace(" ", "");
            }
            else
            {
                if(!(delimitted[1]==" LINEAR"))
                {
                    Engine.Reflection.Compute.RecordWarning(
                        "MidasCivil_Toolkit does not support tension/compression only springs or multi-linear springs");
                    return null;
                }
                else
                {
                    for (int i = 2; i < 8; i++)
                    {
                        if (delimitted[i] == "")
                        {
                            fixity.Add(false);
                            stiffness.Add(0);
                        }
                        else
                        {
                            double spring = double.Parse(delimitted[i]);
                            if (spring == 1E+016 || spring == 100000)
                            {
                                fixity.Add(true);
                                stiffness.Add(0);
                            }
                            else
                            {
                                fixity.Add(false);
                                stiffness.Add(spring);
                            }
                        }
                    }
                    supportName = delimitted[15].Replace(" ", "");

                }
            }

            Constraint6DOF bhomConstraint6DOF = Engine.Structure.Create.Constraint6DOF(supportName, fixity, stiffness);
            bhomConstraint6DOF.CustomData[AdapterId] = supportName;

            return bhomConstraint6DOF;

        }
    }
}