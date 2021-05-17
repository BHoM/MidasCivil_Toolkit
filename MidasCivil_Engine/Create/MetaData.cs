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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using BH.oM.Adapters.MidasCivil;
using BH.oM.Reflection.Attributes;
using System.Globalization;
using BH.Engine;

namespace BH.Engine.Adapters.MidasCivil
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("An object containing various metadata for the model.")]
        [Input("location", "Where the project is based.")]
        [Input("description", "A short description of the project or the script that maybe useful to others.")]
        [Input("discpline", "The discipline responsible for the model.")]
        [Input("creationDate", "The creation date of the model inputted as YYYY-MM-DD. This will default to the current date if no date is provided.")]
        [Input("approved", "The model is approved for it's intended use.")]
        [Input("reviewer", "A list of reviewers who have reviewed the model.")]
        [Input("reviewDate", "The date when the model was reviewed by each reviewer.")]
        [Output("An object containing various meta data for the model.")]
        public static MetaData MetaData(string projectNumber = "", string projectName = "", string location = "", string client = "", 
            string designStage = "", string projectLead = "", string revision = "", string author = "", string creationDate = "Today", string email = "", 
            string description = "", string discipline = "", List<string> reviewer = null, List<string> reviewDate = null, List<string> comments = null, 
            bool approved = false)
        {
            MetaData Data = new MetaData();
            Data.ProjectNumber = projectNumber;
            Data.ProjectName = projectName;
            Data.Location = location;
            Data.Client = client;
            Data.DesignStage = designStage;
            Data.ProjectLead = projectLead;
            Data.Revision = revision;

            Data.Author = author;
            Data.Email = email;

            Data.Description = description;
            Data.Discipline = discipline;

            Data.Comments = comments;
            Data.Approved = approved;

            if (creationDate == "Today") { Data.CreationDate = DateTime.Now; }
            else
            {
                Data.CreationDate = convertDate(creationDate);
            }

            List<DateTime> dates = new List<DateTime>();
            if (reviewer.Count != reviewDate.Count)
            {
                Reflection.Compute.RecordWarning("List of reviewers should be equal in length to list of review dates.");
            }
            else
            {
                foreach(string date in reviewDate)
                {
                    dates.Add(convertDate(date));
                    Data.Reviewer = reviewer;
                    Data.ReviewDate = dates;
                }
            }


            return Data;
        }

        [Input("creationDate", "The creation date of the model inputted as from a calendar. This will default to the current date if no date is provided.")]
        [Input("reviews", "A dictionary containing a list of reviewers as string keys, and date values as DateTimes.")]
        public static MetaData MetaData(string projectNumber = "", string projectName = "", string location = "", string client = "",
            string designStage = "", string projectLead = "", string revision = "", string author = "", DateTime? creationDate = null, string email = "",
            string description = "", string discipline = "", Dictionary<string,DateTime> reviews = null, List<string> comments = null,
            bool approved = false)
        {
            if (creationDate == null) { creationDate = DateTime.Now; }

            MetaData Data = new MetaData();
            Data.ProjectNumber = projectNumber;
            Data.ProjectName = projectName;
            Data.Location = location;
            Data.Client = client;
            Data.DesignStage = designStage;
            Data.ProjectLead = projectLead;
            Data.Revision = revision;

            Data.Author = author;
            Data.CreationDate = (DateTime)creationDate;
            Data.Email = email;

            Data.Description = description;
            Data.Discipline = discipline;


            List<string> reviewers = new List<string>();
            foreach(string reviewer in reviews.Keys)
            {
                reviewers.Add(reviewer);
            }
            Data.Reviewer = reviewers;

            List<DateTime> reviewDates = new List<DateTime>();
            foreach (DateTime reviewDate in reviews.Values)
            {
                reviewDates.Add(reviewDate);
            }
            Data.ReviewDate = reviewDates;

            Data.Comments = comments;
            Data.Approved = approved;

            return Data;
        }

        /***************************************************/
        private static DateTime convertDate(string date)
        {
            date = date.Replace(@"/", "").Replace("-", "").Replace(".", "").Replace(@"\", "");
            return DateTime.ParseExact(date, "yyyyMMdd",
              System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

