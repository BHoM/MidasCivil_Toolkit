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

using BH.oM.Base;
using System;
using System.Collections.Generic;

namespace BH.oM.Adapters.MidasCivil
{

    public class MetaData : BHoMObject //Suggest making this universal
    {

        public virtual string ProjectNumber { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string Location { get; set; }
        public virtual string Client { get; set; }
        public virtual string DesignStage { get; set; }
        public virtual string ProjectLead { get; set; }
        public virtual string Revision { get; set; }

        public virtual string Author { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual string Email { get; set; }

        public virtual string Description { get; set; }
        public virtual string Discipline { get; set; }

        public virtual List<Review> Reviews { get; set; }

    }

    public class Review : BHoMObject
    {

        public virtual string Reviewer { get; set; }
        public virtual DateTime ReviewDate { get; set; }
        public virtual List<string> Comments { get; set; }
        public virtual bool Approved { get; set; }

    }
}

