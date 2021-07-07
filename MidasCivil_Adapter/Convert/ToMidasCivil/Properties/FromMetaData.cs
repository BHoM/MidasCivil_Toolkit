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
using System.Collections.Generic;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static StringBuilder FromMetaData(MetaData metaData)
        {
            StringBuilder midasMetaDataSB = new StringBuilder();
            List<Review> reviews = midasMetaData.Reviews;
            midasMetaDataSB.AppendLine("*PROJINFO");
            midasMetaDataSB.AppendLine($"PROJECT={midasMetaData.ProjectNumber}");
            midasMetaDataSB.AppendLine($";DESIGNSTAGE={midasMetaData.DesignStage}");
            midasMetaDataSB.AppendLine($";PROJECTLEAD={midasMetaData.ProjectLead}");
            midasMetaDataSB.AppendLine($"REVISION={midasMetaData.Revision}");
            midasMetaDataSB.AppendLine($"USER={midasMetaData.Author}");
            midasMetaDataSB.AppendLine($"EMAIL={midasMetaData.Email}");
            midasMetaDataSB.AppendLine($"ADDRESS={midasMetaData.Location.Replace(System.Environment.NewLine, ", ")}");
            midasMetaDataSB.AppendLine($"CLIENT={midasMetaData.Client}");
            midasMetaDataSB.AppendLine($"TITLE={midasMetaData.ProjectName}");
            midasMetaDataSB.AppendLine($"ENGINEER={midasMetaData.Author}");
            midasMetaDataSB.AppendLine($"EDATE={midasMetaData.CreationDate.ToString("yyyy-MM-dd")}");
            midasMetaDataSB.AppendLine($";DESCRIPTION={midasMetaData.Description}");
            midasMetaDataSB.AppendLine($";DISCIPLINE={midasMetaData.Discipline}");

            if(reviews.Count > 4)
            {
                reviews.RemoveRange(1, reviews.Count - 2);
                Engine.Reflection.Compute.RecordWarning("The number of reviews exceeds the limit of MidasCivil. Only the first, last and second to last reviews will be recorded.");
            }
            if(reviews.Count >= 1)
            {
                midasMetaDataSB.AppendLine($"CHECK1={reviews[0].Reviewer}");
                midasMetaDataSB.AppendLine($"CDATE1={reviews[0].ReviewDate.ToString("yyyy-MM-dd")}");
            }
            if (reviews.Count >= 2)
            {
                midasMetaDataSB.AppendLine($"CHECK2={reviews[1].Reviewer}");
                midasMetaDataSB.AppendLine($"CDATE2={reviews[1].ReviewDate.ToString("yyyy-MM-dd")}");
                Engine.Reflection.Compute.RecordWarning("Only comments from the last review will be pushed to MidasCivil.");
            }
            if (reviews.Count >= 3)
            {
                midasMetaDataSB.AppendLine($"CHECK3={reviews[2].Reviewer}");
                midasMetaDataSB.AppendLine($"CDATE3={reviews[2].ReviewDate.ToString("yyyy-MM-dd")}");
            }
            if(reviews[reviews.Count - 1].Approved)
            {
                midasMetaDataSB.AppendLine($"APPROVE={reviews[reviews.Count - 1].Reviewer}");
                midasMetaDataSB.AppendLine($"ADATE={reviews[reviews.Count - 1].ReviewDate.ToString("yyyy-MM-dd")}");
            }

            foreach(string comment in reviews[reviews.Count - 1].Comments)
            {
                midasMetaDataSB.AppendLine($"COMMENT={comment}");
            }

            return midasMetaDataSB;
        }

        /***************************************************/

    }
}
