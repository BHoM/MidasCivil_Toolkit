﻿using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Constraint6DOF ToBHoMConstraint6DOF(this string support, string version)
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
                supportName = delimitted[2].Trim();
            }
            else
            {
                if(!(delimitted[1].Trim()=="LINEAR"))
                {
                    Engine.Reflection.Compute.RecordWarning(
                        "MidasCivil_Toolkit does not support tension/compression only springs or multi-linear springs");
                    return null;
                }
                else
                {

                    switch(version)
                    {
                        case "8.8.5":
                            for (int i = 2; i < 8; i++)
                            {
                                if (delimitted[i].Trim() == "YES")
                                {
                                    fixity.Add(true);
                                    stiffness.Add(0);
                                }
                                else if(delimitted[i].Trim() == "NO")
                                {
                                    double spring = double.Parse(delimitted[i+6]);
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
                            break;

                        default:
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
                            break;
                    }

                    supportName = delimitted[15].Trim();

                }
            }

            Constraint6DOF bhomConstraint6DOF = Engine.Structure.Create.Constraint6DOF(supportName, fixity, stiffness);
            bhomConstraint6DOF.CustomData[AdapterId] = supportName;

            return bhomConstraint6DOF;

        }
    }
}