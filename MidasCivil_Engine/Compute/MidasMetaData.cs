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
using System.Linq;
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

        [Description("Assigns meta data to a file.")]
        [Input("location", "Where is the project based")]
        [Input("description", "Any information about the project, or the script that maybe useful to others.")]
        [Input("discpline", "Which discpline is the primary lead")]
        [Input("creationDate", "The creation date will default to the current date if another date is not inputted.")]
        [Input("approved", "All review comments have been changed, and the script is approved for use.")]
        [Input("reviewer", "Who has reviewed this document, input as list for multple reviews/reviwers.")]
        [Input("reviewDate", "Review date, list length should be equal to the number of reviewers.")]
        [Output("Meta data to be used for MIDAS CIVIL")]

        public static MetaData MidasCivilMetaData(string projectNumber = "", string projectName = "", string location = "", string client = "", 
            string designStage = "", string projectLead = "", string revision = "", string author = "", string creationDate = null, string email = "", 
            string description = "", string discipline = "", List<string> reviewer = null, List<string> reviewDate = null, List<string> comments = null, 
            bool approved = false)
        {
            if(creationDate == null) { creationDate = DateTime.Now.ToString("dd/MM/yyyy"); }

            MetaData Data = new MetaData();
            Data.projectNumber = projectNumber;
            Data.projectName = projectName;
            Data.location = location;
            Data.client = client;
            Data.designStage = designStage;
            Data.projectLead = projectLead;
            Data.revision = revision;

            Data.author = author;
            Data.creationDate = creationDate;
            Data.email = email;

            Data.description = description;
            Data.discipline = discipline;

            Data.reviewer = reviewer;
            Data.reviewDate = reviewDate;
            Data.comments = comments;
            Data.approved = approved;

            return Data;
        }

        /***************************************************/

    }
}

