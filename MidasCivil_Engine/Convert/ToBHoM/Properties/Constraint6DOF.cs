using BH.oM.Structure.Properties.Constraint;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Constraint6DOF ToBHoMConstraint6DOF(this string support, string supportName)
        {
            List<string> delimitted = support.Split(',').ToList();

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
                }
            }
            Constraint6DOF bhomConstraint6DOF = Engine.Structure.Create.Constraint6DOF(supportName, fixity, stiffness);
            bhomConstraint6DOF.CustomData[AdapterId] = supportName.Split('_')[1];

            return bhomConstraint6DOF;

        }
    }
}