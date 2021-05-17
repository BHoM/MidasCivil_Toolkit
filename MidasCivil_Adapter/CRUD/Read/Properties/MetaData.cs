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
using BH.oM.Adapters.MidasCivil;
using System;
using System.Globalization;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IEnumerable<MetaData> ReadMetaData(string id = null)
        {
            MetaData metaData = new MetaData();
            List<string> Data = GetSectionText("PROJINFO");
            List<string> reviewers = new List<string>(4);
            List<DateTime> reviewDates = new List<DateTime>(4);
            List<string> comments = new List<string>();
            DateTime result;

            foreach (string dataItem in Data)
            {
                if (dataItem.Contains("PROJECT=")) { metaData.ProjectNumber = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("REVISION=")) { metaData.Revision = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("USER=")) { metaData.Author = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("EMAIL=")) { metaData.Email = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("ADDRESS=")) { metaData.Location = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("CLIENT=")) { metaData.Client = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("TITLE=")) { metaData.ProjectName = dataItem.Split('=')[1]; }
                else if (dataItem.Contains("EDATE="))
                {
                    if (DateTime.TryParseExact(dataItem.Split('=')[1], "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) { metaData.CreationDate = result; }
                    else if (DateTime.TryParse(dataItem.Split('=')[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) { metaData.CreationDate = result; }
                    else { Engine.Reflection.Compute.RecordWarning("Creation date format not recognised please use yyyy-mm-dd format."); }
                }
                else if (dataItem.Contains(";DESIGNSTAGE=")) { metaData.DesignStage = dataItem.Split('=')[1]; }
                else if (dataItem.Contains(";PROJECTLEAD=")) { metaData.ProjectLead = dataItem.Split('=')[1]; }
                else if (dataItem.Contains(";DESCRIPTION=")) { metaData.Description = dataItem.Split('=')[1]; }
                else if (dataItem.Contains(";DISCIPLINE=")) { metaData.Discipline = dataItem.Split('=')[1]; }

                if (dataItem.Contains("CHECK1=") || dataItem.Contains("CHECK2=") || dataItem.Contains("CHECK3="))
                {
                    reviewers.Add(dataItem.Split('=')[1]);
                }
                else if (dataItem.Contains("APPROVE="))
                {
                    reviewers.Add(dataItem.Split('=')[1]);
                    metaData.Approved = true;
                }
                if (dataItem.Contains("CDATE1=") || dataItem.Contains("CDATE2=") || dataItem.Contains("CDATE3="))
                {
                    if (DateTime.TryParseExact(dataItem.Split('=')[1], "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) { reviewDates.Add(result); }
                    else if (DateTime.TryParse(dataItem.Split('=')[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) { reviewDates.Add(result); }
                    else { Engine.Reflection.Compute.RecordWarning("Check date format not recognised please use yyyy-mm-dd format."); }
                }
                else if (dataItem.Contains("ADATE="))
                {
                    if (DateTime.TryParseExact(dataItem.Split('=')[1], "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) { reviewDates.Add(result); }
                    else if (DateTime.TryParse(dataItem.Split('=')[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) { reviewDates.Add(result); }
                    else { Engine.Reflection.Compute.RecordWarning("Approval date format not recognised please use yyyy-mm-dd format."); }
                    metaData.Approved = true;
                }
                else if (dataItem.Contains("COMMENT="))
                {
                    if (!dataItem.Contains("COMMENT=This Model Was Created Using BHoM Version:")) { comments.Add(dataItem.Split('=')[1]); }
                }
            }
            metaData.Reviewer = reviewers;
            metaData.ReviewDate = reviewDates;
            metaData.Comments = comments;

            List<MetaData> returnMetaData = new List<MetaData>(1);
            returnMetaData.Add(metaData);

            return returnMetaData;
        }

        /***************************************************/

    }
}

