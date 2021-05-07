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

using BH.oM.Adapters.MidasCivil;
using BH.oM.Base;
using System.Collections.Generic;
using System.Linq;

namespace BH.oM.Adapters.MidasCivil
{

    public class MetaData : BHoMObject //Suggest making this universal
    {

        public virtual string projectNumber { get; set; }
        public virtual string projectName { get; set; }
        public virtual string location { get; set; }
        public virtual string client { get; set; }
        public virtual string designStage { get; set; }
        public virtual string projectLead { get; set; }
        public virtual string revision { get; set; }

        public virtual string author { get; set; }
        public virtual string creationDate { get; set; }
        public virtual string email { get; set; }

        public virtual string description { get; set; }
        public virtual string discipline { get; set; }

        public virtual List<string> reviewer { get; set; }
        public virtual List<string> reviewDate { get; set; }
        public virtual List<string> comments { get; set; }
        public virtual bool approved { get; set; }

    }
}

