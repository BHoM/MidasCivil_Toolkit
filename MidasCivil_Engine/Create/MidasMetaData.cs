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
        [Input("creationDate", "The creation date of the model. This will default to the current date if no date is provided.")]
        [Input("approved", "The model is approved for it's intended use.")]
        [Input("reviewer", "A list of reviewers who have reviewed the model.")]
        [Input("reviewDate", "The date when the model was reviewed.")]
        [Output("An object containing various meta data for the model.")]

        public static MetaData MetaData(string projectNumber = "", string projectName = "", string location = "", string client = "", 
            string designStage = "", string projectLead = "", string revision = "", string author = "", string creationDate = "Today", string email = "", 
            string description = "", string discipline = "", List<string> reviewer = null, List<string> reviewDate = null, List<string> comments = null, 
            bool approved = false)
        {
            if(creationDate == "Today") { creationDate = DateTime.Now.ToString("dd/MM/yyyy"); }

            MetaData Data = new MetaData();
            Data.ProjectNumber = projectNumber;
            Data.ProjectName = projectName;
            Data.Location = location;
            Data.Client = client;
            Data.DesignStage = designStage;
            Data.ProjectLead = projectLead;
            Data.Revision = revision;

            Data.Author = author;
            Data.CreationDate = creationDate;
            Data.Email = email;

            Data.Description = description;
            Data.Discipline = discipline;

            Data.Reviewer = reviewer;
            Data.ReviewDate = reviewDate;
            Data.Comments = comments;
            Data.Approved = approved;

            return Data;
        }

        /***************************************************/

    }
}

