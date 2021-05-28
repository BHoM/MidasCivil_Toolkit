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
using System.Globalization;
using BH.oM.Reflection.Attributes;


namespace BH.Engine.Adapters.MidasCivil
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Creates a MetaData object from a variety of inputs.")]
        [Input("location", "Where the project is based.")]
        [Input("description", "A short description of the project and model.")]
        [Input("discpline", "The discipline responsible for the model.")]
        [Input("creationDate", "The creation date of the model inputted as yyyy-MM-dd. This will default to the current date if no date is provided.", typeof(string))]
        [Input("reviews", "A list of reviews containing reviewers, their comments and the date of review.")]
        [Output("A summary of relevant information for the model.")]
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

            DateTime creationDate_;
            if (creationDate == null) { Data.CreationDate = DateTime.Now; }
            else if(DateTime.TryParseExact((string)creationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out creationDate_))
            {
                Data.CreationDate = creationDate_;
            }
            else if (creationDate.GetType() == typeof(DateTime))
            {
                Data.CreationDate = (DateTime)creationDate;
            }
            else
            {
                Engine.Reflection.Compute.RecordError($"creationDate does not support a {creationDate.GetType().ToString()} input.");
            }

            return Data;
        }
    }
}

