/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System.IO;
using System.Collections.Generic;
using System.Text;
using BH.oM.Adapters.MidasCivil;
using BH.Engine;
using System;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool SetMetaData(MetaData MidasMetaData)
        {
            string path = m_directory + @"\TextFiles\00_MetaData";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "\\" + "PROJINFO.txt";

            if (File.Exists(path))
            {
                Engine.Reflection.Compute.RecordWarning("PROJINFO.txt already exists, the information has been update to latest user inputs");
            }

            StringBuilder midasMetaData = new StringBuilder();

            midasMetaData.AppendLine("*PROJINFO");
            midasMetaData.AppendLine($"   PROJECT={MidasMetaData.projectNumber}");
            midasMetaData.AppendLine($"   REVISION={MidasMetaData.revision}");
            midasMetaData.AppendLine($"   USER={MidasMetaData.author}");
            midasMetaData.AppendLine($"   EMAIL={MidasMetaData.email}");
            midasMetaData.AppendLine($"   ADDRESS={MidasMetaData.address.Replace(",","").Replace(System.Environment.NewLine, ", ")}");
            midasMetaData.AppendLine($"   CLIENT={MidasMetaData.client}");
            midasMetaData.AppendLine($"   TITLE={MidasMetaData.projectName}");
            midasMetaData.AppendLine($"   ENGINEER={MidasMetaData.author}");
            midasMetaData.AppendLine($"   EDATE={MidasMetaData.creationDate}");

            if (MidasMetaData.reviewer != null && MidasMetaData.reviewer.Count >= 1)
            {
                int index = MidasMetaData.reviewer.Count;
                midasMetaData.AppendLine($"   CHECK1={MidasMetaData.reviewer[0]}");

                if (MidasMetaData.approved)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"   APPROVE={MidasMetaData.reviewer[index]}");
                }
                if (index >= 3)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"   CHECK3={MidasMetaData.reviewer[index]}");
                    if(index >= 3) { Engine.Reflection.Compute.RecordWarning($"More than three checkers will only record first and last two"); }
                }
                if (index >= 2)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"   CHECK2={MidasMetaData.reviewer[index]}");
                }
            }

            if (MidasMetaData.reviewDate != null && MidasMetaData.reviewDate.Count >= 1)
            {
                int index = MidasMetaData.reviewDate.Count;
                midasMetaData.AppendLine($"   CDATE1={MidasMetaData.reviewDate[0]}");

                if (MidasMetaData.approved)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"   ADATE={MidasMetaData.reviewDate[index]}");
                }
                if (index >= 3)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"   CDATE3={MidasMetaData.reviewDate[index]}");

                }
                if (index >= 2)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"   CDATE2={MidasMetaData.reviewDate[index]}");
                }
            }

            int counter = MidasMetaData.comments.Count;
            if (counter > 16) { Engine.Reflection.Compute.RecordWarning($"The maximum amount of comments is 16, only the first 16 comments will be pushed"); }
            int i = 0;
            midasMetaData.AppendLine($"   COMMENT=This Model Was Created Using BHoM Version: {BH.Engine.Reflection.Query.BHoMVersion()}");
            while (i <= System.Math.Min(16, counter - 1))
            {
                midasMetaData.AppendLine($"   COMMENT={MidasMetaData.comments[i]}");
                i++;
            }

            File.WriteAllText(path, midasMetaData.ToString());

            return true;
        }

        private bool SetUnits(string length = "M", string force = "N", string temperature = "C", string heat = "KJ")
        {
            string path = m_directory + @"\TextFiles\00_MetaData";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "\\" + "UNIT.txt";

            if (!File.Exists(path))
            {
                string units = "*UNIT\n";

                string[] lengths = { "M", "CM", "MM", "FT", "IN" };
                string[] forces = { "N", "KN", "KGF", "TONF", "LBF", "KIPS" };
                string[] temperatures = { "C", "F" };
                string[] heats = { "CAL", "KCAL", "J", "KJ", "BTU" };

                if (Array.Exists(forces, element => element == force))
                {
                    units += force + ", ";
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning($"Unit input {force} not recognised using Newtons (N) instead");
                    units += "N" + ", ";
                }

                if (Array.Exists(lengths, element => element == length))
                {
                    units += length + ", ";
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning($"Unit input {length} not recognised using meters (m) instead");
                    units += "M" + ", ";
                }

                if (Array.Exists(heats, element => element == heat))
                {
                    units += heat + ", ";
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning($"Unit input {heat} not recognised using kilojoules (kJ) instead");
                    units += "KJ" + ", ";
                }
                if (Array.Exists(temperatures, element => element == temperature))
                {
                    units += temperature;
                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning($"Unit input {temperature} not recognised using celcius (C) instead");
                    units += "C";
                }
                units += "\n";

                File.WriteAllText(path, units);
            }

            return true;
        }

        private bool SetVersion(string version)
        {
            string path = m_directory + @"\TextFiles\00_MetaData";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "\\" + "VERSION.txt";

            if (File.Exists(path))
            {
                Engine.Reflection.Compute.RecordWarning($"VERSION.txt already exists, version number has been updated.");
            }

            File.WriteAllText(path, $"\n*VERSION\n   {version}\n");

            return true;
        }


        private MetaData getMetaData(string filepath)
        {
            MetaData metaData = new MetaData();
            List<string> Data = new List<string>(File.ReadAllLines(filepath));
            List<string> reviewers = new List<string>(4);
            List<string> reviewDates = new List<string>(4);
            List<string> comments = new List<string>();

            foreach (string dataItem in Data)
            {
                if(dataItem.Contains("PROJECT=")) { metaData.projectNumber = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("REVISION=")) { metaData.revision = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("USER=")) { metaData.author = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("EMAIL=")) { metaData.email = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("ADDRESS=")) { metaData.address = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("CLIENT=")) { metaData.client = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("TITLE=")) { metaData.projectName = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("EDATE=")) { metaData.creationDate = dataItem.Split('=')[1]; }
                if (dataItem.Contains("CHECK1=")|| dataItem.Contains("CHECK2=")|| dataItem.Contains("CHECK3="))
                {
                    reviewers.Add(dataItem.Split('=')[1]);
                }
                else if (dataItem.Contains("APPROVE="))
                {
                    reviewers.Add(dataItem.Split('=')[1]);
                    metaData.approved = true;
                }
                if (dataItem.Contains("CDATE1=") || dataItem.Contains("CDATE2=") || dataItem.Contains("CDATE3="))
                {
                    reviewDates.Add(dataItem.Split('=')[1]);
                }
                else if (dataItem.Contains("ADATE="))
                {
                    reviewDates.Add(dataItem.Split('=')[1]);
                    metaData.approved = true;
                }
                else if (dataItem.Contains("COMMENT="))
                {
                    if(!dataItem.Contains("COMMENT=This Model Was Created Using BHoM Version:")) { comments.Add(dataItem.Split('=')[1]); }
                }
            }
            metaData.reviewer = reviewers;
            metaData.reviewDate = reviewDates;
            metaData.comments = comments;

            return metaData;
        }
        /***************************************************/

    }
}

