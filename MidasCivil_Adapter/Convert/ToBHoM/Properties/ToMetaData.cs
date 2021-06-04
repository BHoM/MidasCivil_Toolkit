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

using System.Collections.Generic;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapters.MidasCivil;
using System;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private IEnumerable<MetaData> ToMetaData(string id = null)
        {
            MetaData metaData = new MetaData();
            List<Review> reviews = new List<Review>();
            List<string> comments = new List<string>();
            List<string> data = GetSectionText("PROJINFO");
            Review review1 = new Review();
            Review review2 = new Review();
            Review review3 = new Review();
            Review review4 = new Review();

            foreach (string dataItem in data)
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
                    metaData.CreationDate = Create.ConvertDate(dataItem.Split('=')[1]);
                }
                else if (dataItem.Contains(";DESIGNSTAGE=")) { metaData.DesignStage = dataItem.Split('=')[1]; }
                else if (dataItem.Contains(";PROJECTLEAD=")) { metaData.ProjectLead = dataItem.Split('=')[1]; }
                else if (dataItem.Contains(";DESCRIPTION=")) { metaData.Description = dataItem.Split('=')[1]; }
                else if (dataItem.Contains(";DISCIPLINE=")) { metaData.Discipline = dataItem.Split('=')[1]; }

                if (dataItem.Contains("CHECK1="))
                {
                    review1.Reviewer = (dataItem.Split('=')[1]);
                    reviews.Add(review1);
                }
                else if (dataItem.Contains("CHECK2="))
                {
                    review2.Reviewer = (dataItem.Split('=')[1]);
                    reviews.Add(review2);
                }
                else if (dataItem.Contains("CHECK3="))
                {
                    review3.Reviewer = (dataItem.Split('=')[1]);
                    reviews.Add(review3);
                }
                else if (dataItem.Contains("APPROVE="))
                {
                    review4.Reviewer = (dataItem.Split('=')[1]);
                    reviews.Add(review4);
                    review4.Approved = true;
                }
                if (dataItem.Contains("CDATE1=")) { review1.ReviewDate = Create.ConvertDate(dataItem.Split('=')[1]); }
                else if (dataItem.Contains("CDATE2=")) { review2.ReviewDate = Create.ConvertDate(dataItem.Split('=')[1]); }
                else if (dataItem.Contains("CDATE3=")) { review3.ReviewDate = Create.ConvertDate(dataItem.Split('=')[1]); }
                else if (dataItem.Contains("ADATE=")) { review4.ReviewDate = Create.ConvertDate(dataItem.Split('=')[1]); }
                else if (dataItem.Contains("COMMENT="))
                {
                    if (!dataItem.Contains("COMMENT=This Model Was Created Using BHoM Version:"))
                    {
                        if (!string.IsNullOrEmpty(review4.Reviewer))
                        {
                            comments.Add(dataItem.Split('=')[1]);
                            review4.Comments = comments;
                        }
                        else if (!string.IsNullOrEmpty(review3.Reviewer))
                        {
                            review3.Comments.Add(dataItem.Split('=')[1]);
                            review3.Comments = comments;
                        }
                        else if (!string.IsNullOrEmpty(review2.Reviewer))
                        {
                            review2.Comments.Add(dataItem.Split('=')[1]);
                            review2.Comments = comments;
                        }
                        else if (!string.IsNullOrEmpty(review1.Reviewer))
                        {
                            review1.Comments.Add(dataItem.Split('=')[1]);
                            review1.Comments = comments;
                        }
                        else
                        {
                            review1.Reviewer = "Anonymous";
                            review1.Comments = comments;
                        }
                    }
                }
            }
            reviews.Reverse();
            metaData.Reviews = reviews;

            List<MetaData> returnMetaData = new List<MetaData>(1);
            returnMetaData.Add(metaData);

            return returnMetaData;
        }

        /***************************************************/
    }
}