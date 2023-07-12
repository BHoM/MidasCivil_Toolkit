/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.oM.Geometry;
using BH.Engine.Adapter;
using BH.Engine.Geometry;
using BH.oM.Structure.SectionProperties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            List<ISectionProperty> bhomSectionProperties = new List<ISectionProperty>();

            List<string> sectionProperties = GetSectionText("SECTION");

            for (int i = 0; i < sectionProperties.Count; i++)
            {
                string sectionProperty = sectionProperties[i];
                string type = sectionProperty.Split(',')[1].Trim();

                ISectionProperty bhomSectionProperty = null;

                if (type == "VALUE")
                {
                    string sectionProfile = sectionProperty;
                    string sectionProperties1 = sectionProperties[i + 1];
                    string sectionProperties2 = sectionProperties[i + 2];
                    string sectionProperties3 = sectionProperties[i + 3];

                    List<string> split = sectionProfile.Split(',').ToList();

                    bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(
                        split.GetRange(14, sectionProperty.Split(',').Count() - 15), sectionProperties1, sectionProperties2, sectionProperties3,
                        split[12].Trim(), m_lengthUnit);

                    bhomSectionProperty.Name = split[2].Trim();
                    bhomSectionProperty.SetAdapterId(typeof(MidasCivilId), split[0].Trim());

                    i = i + 3;
                }
                else if (type == "DBUSER")
                {
                    int numberColumns = sectionProperty.Split(',').Count();

                    if (numberColumns == 16)
                    {
                        Engine.Base.Compute.RecordWarning("Library sections are not yet supported in the MidasCivil_Toolkit");
                    }
                    else
                    {
                        List<string> split = sectionProperty.Split(',').ToList();
                        bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(split.GetRange(14, numberColumns - 16),
                            split[12].Trim(), m_lengthUnit);

                        bhomSectionProperty.Name = split[2].Trim();
                        bhomSectionProperty.SetAdapterId(typeof(MidasCivilId), split[0].Trim());
                    }
                }
                else if (type == "TAPERED")
                {
                    List<string> split = sectionProperty.Split(',').ToList();
                    List<string> profiles = sectionProperties[i + 1].Split(',').ToList();
                    string shape = split[14].Trim();
                    string interpolationOrder = Math.Max(System.Convert.ToInt32(split[15].Trim()), System.Convert.ToInt32(split[16].Trim())).ToString();

                    bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(profiles, "TAPERED" + "-" + shape + "-" + interpolationOrder, m_lengthUnit);

                    bhomSectionProperty.SetAdapterId(typeof(MidasCivilId), split[0].Trim());
                    bhomSectionProperty.Name = split[2].Trim();

                    i = i + 1;
                }
                else
                {
                    Engine.Base.Compute.RecordWarning(type + " not supported in the MidasCivil_Toolkit");
                }

                if (bhomSectionProperty != null)
                    bhomSectionProperties.Add(bhomSectionProperty);
            }

            // Read PSCValue sections which are effectively Freeform Profiles (ids are aligned with SECTION)
            List<string> pscSectionProperties = GetSectionText("SECT-PSCVALUE");
            for (int i = 0; i < pscSectionProperties.Count; i++)
            {
                int iEnd = pscSectionProperties.FindIndex(i + 1, x => x.Contains("SECT")) - 1;

                if (iEnd == -2)
                    iEnd = pscSectionProperties.Count() - 1;

                List<string> pscSectionProperty = pscSectionProperties.GetRange(i, iEnd - i + 1);

                string sectionProperty = pscSectionProperty[0];
                string type = sectionProperty.Split(',')[1].Trim();

                if (type == "VALUE")
                {
                    string sectionProfile = sectionProperty;
                    string sectionProperties1 = pscSectionProperty[1];
                    string sectionProperties2 = pscSectionProperty[2];
                    string sectionProperties3 = pscSectionProperty[3];

                    // Searching pscSectionProperty which contains a single section and therefore only one OPOLY
                    int iOPolyStart = pscSectionProperty.FindIndex(x => x.Contains("OPOLY"));
                    int iOPolyEnd = pscSectionProperty.FindIndex(x => x.Contains("IPOLY")) - 1;

                    // If there is no inner polylines, then the section only contains outer polylines
                    if (iOPolyEnd == -2)
                        iOPolyEnd = pscSectionProperty.Count() - 1;

                    Polyline oPoly = new Polyline() { ControlPoints = ParsePoints(pscSectionProperty, iOPolyStart, iOPolyEnd, "OPOLY") };

                    List<Polyline> polys = new List<Polyline>() { oPoly };

                    //Generate inner polyline if they exist
                    if (pscSectionProperty.Any(x => x.Contains("IPOLY")))
                    {
                        // Inner polylines always follow outer, find the start of the next IPOLY otherwise it's the end of the PSC Section
                        int iPolyStart = iOPolyEnd + 1;
                        int iPolyEnd = -2;
                        if(!(iPolyStart + 1 > pscSectionProperty.Count - 1))
                            iPolyEnd = pscSectionProperty.FindIndex(iPolyStart + 1, x => x.Contains("IPOLY")) - 1;

                        // This is the case where IPOLY is contained on a single line (i.e. three points)
                        if (iPolyEnd == -2)
                            iPolyEnd = pscSectionProperty.Count - 1;

                        // Iterate through each IPoly, index of -2 identifies no more IPOLY
                        while (!(iPolyEnd == -2))
                        {
                            polys.Add(new Polyline() { ControlPoints = ParsePoints(pscSectionProperty, iPolyStart, iPolyEnd, "IPOLY") });

                            // Get indexes for next polyline, IF statement to avoid out of index if there is a single IPOLY
                            iPolyStart = iPolyEnd + 1;
                            //This is the last IPOLY
                            if(iPolyStart > pscSectionProperty.Count() -1)
                                iPolyEnd = -2;
                            else
                                iPolyEnd = pscSectionProperty.FindIndex(iPolyStart + 1, x => x.Contains("IPOLY")) - 1;
                        }
                    }

                    List<string> split = sectionProfile.Split(',').ToList();

                    ISectionProperty bhomSectionProperty = null;

                    bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(
                        split.GetRange(14, sectionProperty.Split(',').Count() - 15), sectionProperties1, sectionProperties2, sectionProperties3,
                        split[12].Trim(), m_lengthUnit, polys);

                    bhomSectionProperty.Name = split[2].Trim();
                    bhomSectionProperty.SetAdapterId(typeof(MidasCivilId), split[0].Split('=')[1].Trim());

                    if (bhomSectionProperty != null)
                        bhomSectionProperties.Add(bhomSectionProperty);

                    //Set i index for next section property
                    i = iEnd;
                }
            }



            return bhomSectionProperties;
        }

        /***************************************************/

        private List<Point> ParsePoints(List<string> text, int start, int end, string excluder)
        {
            List<Point> parsedPoints = new List<Point>();

            //Generate polyline by extracting points from each line (variable number of points per line)
            for (int i = start; i < end + 1; i++)
            {
                //Extract single line and remove any text that cannot be parsed
                string polyText = text[i];
                if (polyText.Contains(excluder))
                    polyText = polyText.Split('=')[1];

                string[] polyCoords = polyText.Split(',');

                // Each point is formatted X, Y in pairs with each line containing up to four points
                for (int j = 0; j < polyCoords.Count() - 1; j += 2)
                    parsedPoints.Add(new Point() { X = double.Parse(polyCoords[j].Trim()), Y = double.Parse(polyCoords[j + 1].Trim()) });

            }

            parsedPoints.Add(parsedPoints[0]);

            return parsedPoints;
        }

        /***************************************************/

    }
}



