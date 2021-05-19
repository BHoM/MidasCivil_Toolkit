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
        [Input("creationDate", "The creation date of the model inputted as yyyy-MM-dd. This will default to the current date if no date is provided.")]
        [Input("reviews", "A list of reviews inputted as a review object.")]
        [Output("An object containing various meta data for the model.")]
        public static MetaData MetaData(string projectNumber = "", string projectName = "", string location = "", string client = "", 
            string designStage = "", string projectLead = "", string revision = "", string author = "", object creationDate = null, string email = "", 
            string description = "", string discipline = "", List<Review> reviews = null)
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

            Data.Reviews = reviews;

            if (creationDate == null) { Data.CreationDate = DateTime.Now; }
            else
            {
                Data.CreationDate = (DateTime)convertDate(creationDate);
            }

            return Data;
        }

        [Description("An object for creating a review to be inputted into metdata object.")]
        [Input("reviewer", "A list of reviewers who have reviewed the model.")]
        [Input("reviewDate", "The date when the model was reviewed by each reviewer either as a string in the format yyyy-MM-dd, or DateTime object.")]
        [Input("approved", "The model is approved for it's intended use.")]
        [Output("An object containing a review of the model.")]
        public static Review Review(string reviewer = null, object reviewDate = null, List<string> comments = null, bool approved = false)
        {
            Review Review = new Review();
            Review.Reviewer = reviewer;
            Review.ReviewDate = (DateTime)convertDate(reviewDate);
            Review.Comments = comments;
            Review.Approved = approved;
            return Review;
        }

        private static DateTime? convertDate(object date)
        {
            if (date.GetType() == typeof(string))
            {
                string date_ = ((string)date).Replace(@"/", "").Replace("-", "").Replace(".", "").Replace(@"\", "");
                return DateTime.ParseExact(date_, "yyyyMMdd",
                  System.Globalization.CultureInfo.InvariantCulture);
            }
            else if(date.GetType() == typeof(DateTime))
            {
                return (DateTime)date;
            }
            else
            {
                Engine.Reflection.Compute.RecordError($"Date can not be inputted as {date.GetType()}, please input as DateTime or string.");
                return null;
            }
        }
    }
}

