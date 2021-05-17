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

using System.Text;
using BH.oM.Adapters.MidasCivil;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static StringBuilder FromMetaData(MetaData MidasMetaData)
        {
            StringBuilder midasMetaData = new StringBuilder();

            midasMetaData.AppendLine("*PROJINFO");
            midasMetaData.AppendLine($"PROJECT={MidasMetaData.ProjectNumber}");
            midasMetaData.AppendLine($";DESIGNSTAGE={MidasMetaData.DesignStage}");
            midasMetaData.AppendLine($";PROJECTLEAD={MidasMetaData.ProjectLead}");
            midasMetaData.AppendLine($"REVISION={MidasMetaData.Revision}");
            midasMetaData.AppendLine($"USER={MidasMetaData.Author}");
            midasMetaData.AppendLine($"EMAIL={MidasMetaData.Email}");
            midasMetaData.AppendLine($"ADDRESS={MidasMetaData.Location.Replace(System.Environment.NewLine, ", ")}");
            midasMetaData.AppendLine($"CLIENT={MidasMetaData.Client}");
            midasMetaData.AppendLine($"TITLE={MidasMetaData.ProjectName}");
            midasMetaData.AppendLine($"ENGINEER={MidasMetaData.Author}");
            midasMetaData.AppendLine($"EDATE={MidasMetaData.CreationDate.ToString("yyyy-mm-dd")}");
            midasMetaData.AppendLine($";DESCRIPTION={MidasMetaData.Description}");
            midasMetaData.AppendLine($";DISCIPLINE={MidasMetaData.Discipline}");



            if (MidasMetaData.Reviewer != null && MidasMetaData.Reviewer.Count >= 1)
            {
                int index = MidasMetaData.Reviewer.Count;
                midasMetaData.AppendLine($"CHECK1={MidasMetaData.Reviewer[0]}");

                if (MidasMetaData.Approved)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"APPROVE={MidasMetaData.Reviewer[index]}");
                }
                if (index >= 3)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"CHECK3={MidasMetaData.Reviewer[index]}");
                    if (index >= 3) { Engine.Reflection.Compute.RecordWarning($"More than three checkers detected, only the first and last two will be pushed."); }
                }
                if (index >= 2)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"CHECK2={MidasMetaData.Reviewer[index]}");
                }
            }

            if (MidasMetaData.ReviewDate != null && MidasMetaData.ReviewDate.Count >= 1)
            {
                int index = MidasMetaData.ReviewDate.Count;
                midasMetaData.AppendLine($"CDATE1={MidasMetaData.ReviewDate[0].ToString("yyyy-mm-dd")}");

                if (MidasMetaData.Approved)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"ADATE={MidasMetaData.ReviewDate[index].ToString("yyyy-mm-dd")}");
                }
                if (index >= 3)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"CDATE3={MidasMetaData.ReviewDate[index].ToString("yyyy-mm-dd")}");

                }
                if (index >= 2)
                {
                    index -= 1;
                    midasMetaData.AppendLine($"CDATE2={MidasMetaData.ReviewDate[index].ToString("yyyy-mm-dd")}");
                }
            }

            int counter = MidasMetaData.Comments.Count;
            if (counter > 16) { Engine.Reflection.Compute.RecordWarning($"The maximum amount of comments is 16, only the first 16 comments will be pushed."); }
            int i = 0;
            midasMetaData.AppendLine($"COMMENT=This Model Was Created Using BHoM Version: {BH.Engine.Reflection.Query.BHoMVersion()}");
            while (i <= System.Math.Min(16, counter - 1))
            {
                midasMetaData.AppendLine($"COMMENT={MidasMetaData.Comments[i]}");
                i++;
            }


            return midasMetaData;
        }

        /***************************************************/

    }
}
