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

        [Description("An object for creating a review to be inputted into metdata object.")]
        [Input("reviewer", "The person who has reviewed the model.")]
        [Input("reviewDate", "The date when the model was reviewed by the reviewer. As a string in the format yyyy-MM-dd, or DateTime object.",typeof(string))]
        [Input("Comments", "A list of comments made by the reviewer.")]
        [Input("approved", "True if the model is approved for its intended use.")]
        [Output("An object containing the reviewer, review date and whether the model has been approved.")]
        public static Review Review(string reviewer = null, object reviewDate = null, List<string> comments = null, bool approved = false)
        {
            Review Review = new Review();
            Review.Reviewer = reviewer;
            DateTime reviewDate_;
            if (reviewDate == null)
            {
                Review.ReviewDate = DateTime.Now;
            }
            else if (DateTime.TryParseExact((string)reviewDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out reviewDate_))
            {
                Review.ReviewDate = reviewDate_;
            }
            else if (reviewDate.GetType() == typeof(DateTime))
            {
                Review.ReviewDate = (DateTime)reviewDate;
            }
            else
            {
                Engine.Reflection.Compute.RecordError($"reviewDate does not support a {reviewDate.GetType().ToString()} input.");
            }
            Review.Comments = comments;
            Review.Approved = approved;
            return Review;
        }
    }
}

