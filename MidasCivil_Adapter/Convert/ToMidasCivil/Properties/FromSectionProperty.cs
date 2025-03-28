/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Geometry;
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;
using BH.Engine.Geometry;
using BH.Engine.Structure;
using System.Linq;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<string> FromSectionProperty(this ISectionProperty sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            return CreateSection(sectionProperty as dynamic, lengthUnit, sectionPropertyCharacterLimit) ?? null;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static List<string> CreateSection(SteelSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                TaperedProfile taperedProfile = (TaperedProfile)sectionProperty.SectionProfile;
                List<IProfile> profiles = new List<IProfile>(taperedProfile.Profiles.Values);
                if (profiles.Any(x => x.Shape != profiles.First().Shape))
                {
                    Engine.Base.Compute.RecordError("MidasCivil_Toolkit does not support TaperedProfiles with different section shapes.");
                    return null;
                }
                else
                {
                    midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",TAPERED," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + "," + GetInterpolationOrder(sectionProperty) +
                        "," + GetInterpolationOrder(sectionProperty) + ",USER");
                    midasSectionProperty.Add(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
                }
            }
            else if (sectionProperty.SectionProfile is FreeFormProfile)
            {
                FreeFormProfile freeformProfile = (FreeFormProfile)sectionProperty.SectionProfile;

                // Add the preamble required for the section property
                midasSectionProperty.Add("SECT=" + sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + ",YES,YES");
                // Add the values corresponding to the section properties, Asy and Asz are currently quite different between MidasCivil and BHoM (ticket raised)
                midasSectionProperty.Add(sectionProperty.Area.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asy.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asz.AreaFromSI(lengthUnit).ToString() + "," +
                    sectionProperty.J.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iy.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iz.AreaMomentOfInertiaFromSI(lengthUnit).ToString());

                List<ICurve> perimeters = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList();

                double outerPerimeter = perimeters[0].ILength();
                double innerPerimeter = perimeters.Sum(x => x.ILength()) - outerPerimeter;
                double totalWidth = CulmalativeWidth(perimeters);
                double totalDepth = CulmalativeDepth(perimeters);

                midasSectionProperty.Add(sectionProperty.Vy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vz.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit)
                    + "," + sectionProperty.Wely.VolumeFromSI(lengthUnit) / totalWidth + "," + sectionProperty.Welz.VolumeFromSI(lengthUnit) / totalDepth + "," + outerPerimeter + "," + innerPerimeter + "," +
                    sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit));

                Engine.Base.Compute.RecordWarning("The shear factor (Qby and Qbz) are calculated by determining the thickness of the voided section and can differ from the Midas SPC calcualted values.");
                Engine.Base.Compute.RecordWarning("The shear areas are calculated using integration and can differ from the Midas SPC calculated value.");

                //Work out extreme points in each corner of the section p1 (top left), p2 (top right), p3 (bottom right), p4 (bottom left) of the outer polyline
                ICurve oPoly = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList()[0];
                List<Point> controlPoints = oPoly.IControlPoints();

                // ExtremePoints
                midasSectionProperty.Add(ExtremePoints(controlPoints, lengthUnit));

                // Add OPOLY and IPOLY
                midasSectionProperty.AddRange(CreatePSCProfile(freeformProfile, lengthUnit));
            }
            else
            {
                midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(ConcreteSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                TaperedProfile taperedProfile = (TaperedProfile)sectionProperty.SectionProfile;
                List<IProfile> profiles = new List<IProfile>(taperedProfile.Profiles.Values);
                if (profiles.Any(x => x.Shape != profiles.First().Shape))
                {
                    Engine.Base.Compute.RecordError("MidasCivil_Toolkit does not support TaperedProfiles with different section shapes.");
                    return null;
                }
                else
                {
                    midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",TAPERED," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + "," + GetInterpolationOrder(sectionProperty) +
                        "," + GetInterpolationOrder(sectionProperty) + ",USER");
                    midasSectionProperty.Add(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
                }
            }
            else if (sectionProperty.SectionProfile is FreeFormProfile)
            {
                FreeFormProfile freeformProfile = (FreeFormProfile)sectionProperty.SectionProfile;

                // Add the preamble required for the section property
                midasSectionProperty.Add("SECT=" + sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + ",YES,YES");
                // Add the values corresponding to the section properties, Asy and Asz are currently quite different between MidasCivil and BHoM (ticket raised)
                midasSectionProperty.Add(sectionProperty.Area.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asy.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asz.AreaFromSI(lengthUnit).ToString() + "," +
                    sectionProperty.J.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iy.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iz.AreaMomentOfInertiaFromSI(lengthUnit).ToString());

                List<ICurve> perimeters = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList();

                double outerPerimeter = perimeters[0].ILength();
                double innerPerimeter = perimeters.Sum(x => x.ILength()) - outerPerimeter;
                double totalWidth = CulmalativeWidth(perimeters);
                double totalDepth = CulmalativeDepth(perimeters);

                midasSectionProperty.Add(sectionProperty.Vy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vz.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit)
                    + "," + sectionProperty.Wely.VolumeFromSI(lengthUnit) / totalWidth + "," + sectionProperty.Welz.VolumeFromSI(lengthUnit) / totalDepth + "," + outerPerimeter + "," + innerPerimeter + "," +
                    sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit));

                Engine.Base.Compute.RecordWarning("The shear factor (Qby and Qbz) are calculated by determining the thickness of the voided section and can differ from the Midas SPC calcualted values.");
                Engine.Base.Compute.RecordWarning("The shear areas are calculated using integration and can differ from the Midas SPC calculated value.");

                //Work out extreme points in each corner of the section p1 (top left), p2 (top right), p3 (bottom right), p4 (bottom left) of the outer polyline
                ICurve oPoly = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList()[0];
                List<Point> controlPoints = oPoly.IControlPoints();

                // ExtremePoints
                midasSectionProperty.Add(ExtremePoints(controlPoints, lengthUnit));

                // Add OPOLY and IPOLY
                midasSectionProperty.AddRange(CreatePSCProfile(freeformProfile, lengthUnit));
            }
            else
            {
                midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(TimberSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                TaperedProfile taperedProfile = (TaperedProfile)sectionProperty.SectionProfile;
                List<IProfile> profiles = new List<IProfile>(taperedProfile.Profiles.Values);
                if (profiles.Any(x => x.Shape != profiles.First().Shape))
                {
                    Engine.Base.Compute.RecordError("MidasCivil_Toolkit does not support TaperedProfiles with different section shapes.");
                    return null;
                }
                else
                {
                    midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",TAPERED," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + "," + GetInterpolationOrder(sectionProperty) +
                        "," + GetInterpolationOrder(sectionProperty) + ",USER");
                    midasSectionProperty.Add(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
                }
            }
            else if (sectionProperty.SectionProfile is FreeFormProfile)
            {
                FreeFormProfile freeformProfile = (FreeFormProfile)sectionProperty.SectionProfile;

                // Add the preamble required for the section property
                midasSectionProperty.Add("SECT=" + sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + ",YES,YES");
                // Add the values corresponding to the section properties, Asy and Asz are currently quite different between MidasCivil and BHoM (ticket raised)
                midasSectionProperty.Add(sectionProperty.Area.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asy.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asz.AreaFromSI(lengthUnit).ToString() + "," +
                    sectionProperty.J.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iy.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iz.AreaMomentOfInertiaFromSI(lengthUnit).ToString());

                List<ICurve> perimeters = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList();

                double outerPerimeter = perimeters[0].ILength();
                double innerPerimeter = perimeters.Sum(x => x.ILength()) - outerPerimeter;
                double totalWidth = CulmalativeWidth(perimeters);
                double totalDepth = CulmalativeDepth(perimeters);

                midasSectionProperty.Add(sectionProperty.Vy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vz.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit)
                    + "," + sectionProperty.Wely.VolumeFromSI(lengthUnit) / totalWidth + "," + sectionProperty.Welz.VolumeFromSI(lengthUnit) / totalDepth + "," + outerPerimeter + "," + innerPerimeter + "," +
                    sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit));

                Engine.Base.Compute.RecordWarning("The shear factor (Qby and Qbz) are calculated by determining the thickness of the voided section and can differ from the Midas SPC calcualted values.");
                Engine.Base.Compute.RecordWarning("The shear areas are calculated using integration and can differ from the Midas SPC calculated value.");

                //Work out extreme points in each corner of the section p1 (top left), p2 (top right), p3 (bottom right), p4 (bottom left) of the outer polyline
                ICurve oPoly = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList()[0];
                List<Point> controlPoints = oPoly.IControlPoints();

                // ExtremePoints
                midasSectionProperty.Add(ExtremePoints(controlPoints, lengthUnit));

                // Add OPOLY and IPOLY
                midasSectionProperty.AddRange(CreatePSCProfile(freeformProfile, lengthUnit));
            }
            else
            {
                midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(GenericSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                TaperedProfile taperedProfile = (TaperedProfile)sectionProperty.SectionProfile;
                List<IProfile> profiles = new List<IProfile>(taperedProfile.Profiles.Values);
                if (profiles.Any(x => x.Shape != profiles.First().Shape))
                {
                    Engine.Base.Compute.RecordError("MidasCivil_Toolkit does not support TaperedProfiles with different section shapes.");
                    return null;
                }
                else
                {
                    midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",TAPERED," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + "," + GetInterpolationOrder(sectionProperty) +
                        "," + GetInterpolationOrder(sectionProperty) + ",USER");
                    midasSectionProperty.Add(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
                }
            }
            else if (sectionProperty.SectionProfile is FreeFormProfile)
            {
                FreeFormProfile freeformProfile = (FreeFormProfile)sectionProperty.SectionProfile;

                // Add the preamble required for the section property
                midasSectionProperty.Add("SECT=" + sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + ",YES,YES");
                // Add the values corresponding to the section properties, Asy and Asz are currently quite different between MidasCivil and BHoM (ticket raised)
                midasSectionProperty.Add(sectionProperty.Area.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asy.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asz.AreaFromSI(lengthUnit).ToString() + "," +
                    sectionProperty.J.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iy.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iz.AreaMomentOfInertiaFromSI(lengthUnit).ToString());

                List<ICurve> perimeters = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList();

                double outerPerimeter = perimeters[0].ILength();
                double innerPerimeter = perimeters.Sum(x => x.ILength()) - outerPerimeter;
                double totalWidth = CulmalativeWidth(perimeters);
                double totalDepth = CulmalativeDepth(perimeters);

                midasSectionProperty.Add(sectionProperty.Vy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vz.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit)
                    + "," + sectionProperty.Wely.VolumeFromSI(lengthUnit) / totalWidth + "," + sectionProperty.Welz.VolumeFromSI(lengthUnit) / totalDepth + "," + outerPerimeter + "," + innerPerimeter + "," +
                    sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit));

                Engine.Base.Compute.RecordWarning("The shear factor (Qby and Qbz) are calculated by determining the thickness of the voided section and can differ from the Midas SPC calcualted values.");
                Engine.Base.Compute.RecordWarning("The shear areas are calculated using integration and can differ from the Midas SPC calculated value.");

                //Work out extreme points in each corner of the section p1 (top left), p2 (top right), p3 (bottom right), p4 (bottom left) of the outer polyline
                ICurve oPoly = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList()[0];
                List<Point> controlPoints = oPoly.IControlPoints();

                // ExtremePoints
                midasSectionProperty.Add(ExtremePoints(controlPoints, lengthUnit));

                // Add OPOLY and IPOLY
                midasSectionProperty.AddRange(CreatePSCProfile(freeformProfile, lengthUnit));
            }
            else
            {
                midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(AluminiumSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                TaperedProfile taperedProfile = (TaperedProfile)sectionProperty.SectionProfile;
                List<IProfile> profiles = new List<IProfile>(taperedProfile.Profiles.Values);
                if (profiles.Any(x => x.Shape != profiles.First().Shape))
                {
                    Engine.Base.Compute.RecordError("MidasCivil_Toolkit does not support TaperedProfiles with different section shapes.");
                    return null;
                }
                else
                {
                    midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",TAPERED," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + "," + GetInterpolationOrder(sectionProperty) +
                        "," + GetInterpolationOrder(sectionProperty) + ",USER");
                    midasSectionProperty.Add(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
                }
            }
            else if (sectionProperty.SectionProfile is FreeFormProfile)
            {
                FreeFormProfile freeformProfile = (FreeFormProfile)sectionProperty.SectionProfile;

                // Add the preamble required for the section property
                midasSectionProperty.Add("SECT=" + sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE," +
                        new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                        ",CC, 0,0,0,0,0,0,YES,NO," + GetSectionShapeCode(sectionProperty) + ",YES,YES");
                // Add the values corresponding to the section properties, Asy and Asz are currently quite different between MidasCivil and BHoM (ticket raised)
                midasSectionProperty.Add(sectionProperty.Area.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asy.AreaFromSI(lengthUnit).ToString() + "," + sectionProperty.Asz.AreaFromSI(lengthUnit).ToString() + "," +
                    sectionProperty.J.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iy.AreaMomentOfInertiaFromSI(lengthUnit).ToString() + "," + sectionProperty.Iz.AreaMomentOfInertiaFromSI(lengthUnit).ToString());

                List<ICurve> perimeters = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList();

                double outerPerimeter = perimeters[0].ILength();
                double innerPerimeter = perimeters.Sum(x => x.ILength()) - outerPerimeter;
                double totalWidth = CulmalativeWidth(perimeters);
                double totalDepth = CulmalativeDepth(perimeters);

                midasSectionProperty.Add(sectionProperty.Vy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vz.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit)
                    + "," + sectionProperty.Wely.VolumeFromSI(lengthUnit) / totalWidth + "," + sectionProperty.Welz.VolumeFromSI(lengthUnit) / totalDepth + "," + outerPerimeter + "," + innerPerimeter + "," +
                    sectionProperty.Vpy.LengthFromSI(lengthUnit) + "," + sectionProperty.Vpz.LengthFromSI(lengthUnit));

                Engine.Base.Compute.RecordWarning("The shear factor (Qby and Qbz) are calculated by determining the thickness of the voided section and can differ from the Midas SPC calcualted values.");
                Engine.Base.Compute.RecordWarning("The shear areas are calculated using integration and can differ from the Midas SPC calculated value.");

                //Work out extreme points in each corner of the section p1 (top left), p2 (top right), p3 (bottom right), p4 (bottom left) of the outer polyline
                ICurve oPoly = freeformProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList()[0];
                List<Point> controlPoints = oPoly.IControlPoints();

                // ExtremePoints
                midasSectionProperty.Add(ExtremePoints(controlPoints, lengthUnit));

                // Add OPOLY and IPOLY
                midasSectionProperty.AddRange(CreatePSCProfile(freeformProfile, lengthUnit));
            }
            else
            {
                midasSectionProperty.Add(sectionProperty.AdapterId<string>(typeof(MidasCivilId)) + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(ExplicitSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            Engine.Base.Compute.RecordError("ExplicitSection not supported in MidasCivil_Toolkit");

            return null;
        }

        /***************************************************/

        private static string CreateProfile(RectangleProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "SB, 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + " ,0, 0, 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(BoxProfile profile, string lengthUnit)
        {
            double webSpacing = (profile.Width - profile.Thickness).LengthFromSI(lengthUnit);
            string midasSectionProperty = "B, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," + profile.Thickness.LengthFromSI(lengthUnit) + "," +
                profile.Thickness.LengthFromSI(lengthUnit) + "," + webSpacing + "," + profile.Thickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(FabricatedBoxProfile profile, string lengthUnit)
        {
            double webSpacing = (profile.Width - profile.WebThickness).LengthFromSI(lengthUnit);
            string midasSectionProperty = "B, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," + profile.WebThickness.LengthFromSI(lengthUnit) + "," +
                profile.TopFlangeThickness.LengthFromSI(lengthUnit) + "," + webSpacing + "," + profile.BotFlangeThickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(CircleProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "SR, 2," +
                profile.Diameter.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(TubeProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "P  , 2," +
                profile.Diameter.LengthFromSI(lengthUnit) + "," + profile.Thickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ISectionProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "H, 2, " + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.FlangeThickness.LengthFromSI(lengthUnit) + "," + profile.RootRadius.LengthFromSI(lengthUnit) + "," + profile.ToeRadius.LengthFromSI(lengthUnit) +
                ", 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(TSectionProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "T, 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) +
                ",0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(FabricatedISectionProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "H, 2, " + profile.Height.LengthFromSI(lengthUnit) + "," + profile.TopFlangeWidth.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.TopFlangeThickness.LengthFromSI(lengthUnit) + "," + profile.BotFlangeWidth.LengthFromSI(lengthUnit) + "," +
                profile.BotFlangeThickness.LengthFromSI(lengthUnit) + "," + profile.WeldSize.LengthFromSI(lengthUnit) +
                ", 0, 0, 0";
            Engine.Base.Compute.RecordWarning("The weld size for the FabiricatedISectionProfile been assumed equal to the root radius parameter in MidasCivil");

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(AngleProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "L, 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ChannelProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "C  , 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.FlangeWidth.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) + "," +
                profile.FlangeWidth.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) + ", 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(GeneralisedFabricatedBoxProfile profile, string lengthUnit)
        {
            double webSpacing = 0;
            double flangeWidth = (profile.Width);
            if (profile.TopLeftCorbelWidth != 0 || profile.TopRightCorbelWidth != 0 || profile.BotLeftCorbelWidth != 0 || profile.BotRightCorbelWidth != 0)
            {
                webSpacing = (profile.Width - profile.WebThickness).LengthFromSI(lengthUnit);
                flangeWidth = (flangeWidth + profile.TopLeftCorbelWidth + profile.TopRightCorbelWidth);

                Engine.Base.Compute.RecordWarning("MidasCivil does not support unequal corbel widths. Therefore the section width has been calculated using the TopCorbelWidths.");
            }
            string midasSectionProperty = "B, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + flangeWidth.LengthFromSI(lengthUnit) + "," + profile.WebThickness.LengthFromSI(lengthUnit) + "," +
                profile.TopFlangeThickness.LengthFromSI(lengthUnit) + "," + webSpacing + "," + profile.BotFlangeThickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ZSectionProfile profile, string lengthUnit)
        {
            Engine.Base.Compute.RecordError("ZSectionProfile not supported by the MidasCivil_Toolkit");

            return null;
        }

        /***************************************************/

        private static string CreateProfile(KiteProfile profile, string lengthUnit)
        {
            Engine.Base.Compute.RecordError("KiteProfile not supported by the MidasCivil_Toolkit");

            return null;
        }

        /***************************************************/

        private static string CreateProfile(GeneralisedTSectionProfile profile, string lengthUnit)
        {
            Engine.Base.Compute.RecordError("GeneralisedTSectionProfile not supported by the MidasCivil_Toolkit");

            return null;
        }

        /***************************************************/

        private static string CreateProfile(TaperedProfile profile, string lengthUnit)
        {
            List<IProfile> profiles = new List<IProfile>(profile.Profiles.Values);

            if (profiles.Count > 2)
                Engine.Base.Compute.RecordWarning("MidasCivil only supports TaperedProfiles with a startProfile and endProfile, any intermediate profiles will be ignored.");

            List<string> startProfile = new List<string>(CreateProfile(profiles[0] as dynamic, lengthUnit).Split(','));
            List<string> endProfile = new List<string>(CreateProfile(profiles[profiles.Count - 1] as dynamic, lengthUnit).Split(','));

            startProfile.GetRange(2, startProfile.Count - 4);

            return string.Join(",", startProfile.GetRange(2, startProfile.Count - 4)) + "," + string.Join(",", endProfile.GetRange(2, startProfile.Count - 4));
        }

        /***************************************************/

        private static List<string> CreatePSCProfile(FreeFormProfile sectionProfile, string lengthUnit)
        {
            List<string> profile = new List<string>();

            List<ICurve> edges = sectionProfile.Edges.OrderBy(x => x.ILength()).Reverse().ToList();

            // oPoly needs to be clockwise and iPoly needs to be anti-clockwise to visual correctly in MidasCivil
            Polyline oPoly = (Polyline)edges[0];

            if (!oPoly.IsClockwise(oPoly.Centroid().Translate(new Vector() { Z = 1 })))
                oPoly = oPoly.Flip();

            profile.AddRange(CreatePolystring(Engine.Geometry.Compute.ISortAlongCurve(oPoly.IControlPoints(), oPoly).Distinct().ToList(), "OPOLY", lengthUnit));

            if (edges.Count > 1)
            {
                for (int i = 1; i < edges.Count; i++)
                {
                    Polyline iPoly = (Polyline)edges[i];

                    if (iPoly.IsClockwise(iPoly.Centroid().Translate(new Vector() { Z = 1 })))
                        iPoly = iPoly.Flip();

                    profile.AddRange(CreatePolystring(Engine.Geometry.Compute.ISortAlongCurve(iPoly.IControlPoints(), iPoly).Distinct().ToList(), "IPOLY", lengthUnit));
                }
            }
            return profile;
        }

        /***************************************************/

        private static string GetInterpolationOrder(ISectionProperty sectionProperty)
        {
            if (sectionProperty is SteelSection)
            {
                SteelSection section = (SteelSection)sectionProperty;
                TaperedProfile taperedProfile = (TaperedProfile)section.SectionProfile;
                return taperedProfile.InterpolationOrder.Max().ToString();

            }
            else if (sectionProperty is ConcreteSection)
            {
                ConcreteSection section = (ConcreteSection)sectionProperty;
                TaperedProfile taperedProfile = (TaperedProfile)section.SectionProfile;
                return taperedProfile.InterpolationOrder.Max().ToString();
            }
            else if (sectionProperty is AluminiumSection)
            {
                AluminiumSection section = (AluminiumSection)sectionProperty;
                TaperedProfile taperedProfile = (TaperedProfile)section.SectionProfile;
                return taperedProfile.InterpolationOrder.Max().ToString();
            }
            else if (sectionProperty is TimberSection)
            {
                TimberSection section = (TimberSection)sectionProperty;
                TaperedProfile taperedProfile = (TaperedProfile)section.SectionProfile;
                return taperedProfile.InterpolationOrder.Max().ToString();
            }
            else if (sectionProperty is GenericSection)
            {
                GenericSection section = (GenericSection)sectionProperty;
                TaperedProfile taperedProfile = (TaperedProfile)section.SectionProfile;
                return taperedProfile.InterpolationOrder.Max().ToString();
            }
            return null;
        }

        /***************************************************/

        private static string GetSectionShapeCode(SteelSection sectionProperty)
        {
            return GetProfileShapeCode(sectionProperty.SectionProfile as dynamic);
        }

        /***************************************************/

        private static string GetSectionShapeCode(ConcreteSection sectionProperty)
        {
            return GetProfileShapeCode(sectionProperty.SectionProfile as dynamic);
        }

        /***************************************************/

        private static string GetSectionShapeCode(TimberSection sectionProperty)
        {
            return GetProfileShapeCode(sectionProperty.SectionProfile as dynamic);
        }

        /***************************************************/

        private static string GetSectionShapeCode(AluminiumSection sectionProperty)
        {
            return GetProfileShapeCode(sectionProperty.SectionProfile as dynamic);
        }

        /***************************************************/

        private static string GetSectionShapeCode(GenericSection sectionProperty)
        {
            return GetProfileShapeCode(sectionProperty.SectionProfile as dynamic);
        }

        /***************************************************/

        private static string GetSectionShapeCode(ExplicitSection sectionProperty)
        {
            return null;
        }

        /***************************************************/

        private static string GetProfileShapeCode(RectangleProfile sectionProfile)
        {
            return "SB";
        }

        /***************************************************/

        private static string GetProfileShapeCode(CircleProfile sectionProfile)
        {
            return "SR";
        }

        /***************************************************/

        private static string GetProfileShapeCode(TubeProfile sectionProfile)
        {
            return "P";
        }

        /***************************************************/

        private static string GetProfileShapeCode(GeneralisedFabricatedBoxProfile sectionProfile)
        {
            return "B";
        }

        /***************************************************/

        private static string GetProfileShapeCode(FabricatedBoxProfile sectionProfile)
        {
            return "B";
        }

        /***************************************************/

        private static string GetProfileShapeCode(BoxProfile sectionProfile)
        {
            return "B";
        }

        /***************************************************/

        private static string GetProfileShapeCode(ISectionProfile sectionProfile)
        {
            return "H";
        }

        /***************************************************/

        private static string GetProfileShapeCode(FabricatedISectionProfile sectionProfile)
        {
            return "H";
        }

        /***************************************************/

        private static string GetProfileShapeCode(TSectionProfile sectionProfile)
        {
            return "T";
        }

        /***************************************************/

        private static string GetProfileShapeCode(AngleProfile sectionProfile)
        {
            return "L";
        }

        /***************************************************/

        private static string GetProfileShapeCode(ChannelProfile sectionProfile)
        {
            return "C";
        }

        /***************************************************/

        private static string GetProfileShapeCode(FreeFormProfile sectionProfile)
        {
            return "GEN";
        }

        /***************************************************/

        private static string GetProfileShapeCode(GeneralisedTSectionProfile sectionProfile)
        {
            return null;
        }

        /***************************************************/

        private static string GetProfileShapeCode(TaperedProfile sectionProfile)
        {
            TaperedProfile taperedProfile = (TaperedProfile)sectionProfile;
            return GetProfileShapeCode(taperedProfile.Profiles.Values.First() as dynamic);
        }

        /***************************************************/

        private static List<string> CreatePolystring(List<Point> polyPoints, string polytype, string lengthUnit)
        {
            List<string> poly = new List<string>();
            int polyCount = polyPoints.Count();

            // Output polyline at first three points (minimum number required) including the polytype
            poly.Add($"{polytype}={polyPoints[0].X.LengthFromSI(lengthUnit)},{polyPoints[0].Y.LengthFromSI(lengthUnit)},{polyPoints[1].X.LengthFromSI(lengthUnit)},{polyPoints[1].Y.LengthFromSI(lengthUnit)}," +
                $"{polyPoints[2].X.LengthFromSI(lengthUnit)},{polyPoints[2].Y.LengthFromSI(lengthUnit)}");

            if (polyCount > 3)
            {
                for (int i = 3; i < polyCount - 3; i += 4)
                {
                    poly.Add($"{polyPoints[i].X.LengthFromSI(lengthUnit)},{polyPoints[i].Y.LengthFromSI(lengthUnit)},{polyPoints[i + 1].X.LengthFromSI(lengthUnit)},{polyPoints[i + 1].Y.LengthFromSI(lengthUnit)}," +
                        $"{polyPoints[i + 2].X.LengthFromSI(lengthUnit)},{polyPoints[i + 2].Y.LengthFromSI(lengthUnit)},{polyPoints[i + 3].X.LengthFromSI(lengthUnit)},{polyPoints[i + 3].Y.LengthFromSI(lengthUnit)}");
                }

                // 3 because 3 initial points (no duplicate points as Dinstict() used in input for this method)
                int remainderPointsCount = (polyCount - 3) % 4;

                if (remainderPointsCount > 0)
                {
                    string remainderPoints = "";
                    for (int j = polyCount - remainderPointsCount; j < polyCount; j++)
                    {
                        if (remainderPoints == "")
                            remainderPoints = polyPoints[j].X.LengthFromSI(lengthUnit) + "," + polyPoints[j].Y.LengthFromSI(lengthUnit);
                        else
                            remainderPoints = remainderPoints + "," + polyPoints[j].X + "," + polyPoints[j].Y;
                    }

                    poly.Add(remainderPoints);

                }
            }

            return poly;
        }

        private static string ExtremePoints(List<Point> controlPoints, string lengthUnit)
        {
            string extremePoints;

            Point p1 = null;
            Point p2 = null;
            Point p3 = null;
            Point p4 = null;

            if (controlPoints.Count == 4)
            {
                p1 = controlPoints[0];
                p2 = controlPoints[0];
                p3 = controlPoints[1];
                p4 = controlPoints[2];

                extremePoints = $"{p1.X.LengthFromSI(lengthUnit)}, {p2.X.LengthFromSI(lengthUnit)}, {p3.X.LengthFromSI(lengthUnit)}," +
                    $"{p1.Y.LengthFromSI(lengthUnit)},{p2.Y.LengthFromSI(lengthUnit)}, {p3.Y.LengthFromSI(lengthUnit)}";
            }
            else
            {
                // In general there will be a point in each quadrant, but there are instances where they are not 
                List<Point> q1 = controlPoints.Where(x => x.Y > 0).Where(x => x.X > 0).OrderBy(x => x.Distance(new Point())).Reverse().ToList();
                List<Point> q2 = controlPoints.Where(x => x.Y > 0).Where(x => x.X < 0).OrderBy(x => x.Distance(new Point())).Reverse().ToList();
                List<Point> q3 = controlPoints.Where(x => x.Y < 0).Where(x => x.X < 0).OrderBy(x => x.Distance(new Point())).Reverse().ToList();
                List<Point> q4 = controlPoints.Where(x => x.Y < 0).Where(x => x.X > 0).OrderBy(x => x.Distance(new Point())).Reverse().ToList();

                List<Point> p1p2p3p4 = new List<Point>();

                // Take the first point from each list - this works for a point in each quadrant and for the scenario where three points exist in a single quadrant 
                for (int i = 0; i < 4; i++)
                {
                    int j = 0;
                    while (j < 4)
                    {
                        if (q1.ElementAtOrDefault(i) != null)
                        {
                            p1p2p3p4.Add(q1[i]);
                            j = j + 1;
                        }
                        if (q2.ElementAtOrDefault(i) != null)
                        {
                            p1p2p3p4.Add(q2[i]);
                            j = j + 1;
                        }
                        if (q3.ElementAtOrDefault(i) != null)
                        {
                            p1p2p3p4.Add(q3[i]);
                            j = j + 1;
                        }
                        if (q4.ElementAtOrDefault(i) != null)
                        {
                            p1p2p3p4.Add(q4[i]);
                            j = j + 1;
                        }
                    }

                    if (j == 4)
                        break;
                }

                p1 = p1p2p3p4[0];
                p2 = p1p2p3p4[1];
                p3 = p1p2p3p4[2];
                p4 = p1p2p3p4[3];

                extremePoints = $"{p1.X.LengthFromSI(lengthUnit)}, {p2.X.LengthFromSI(lengthUnit)}, {p3.X.LengthFromSI(lengthUnit)}, {p4.X.LengthFromSI(lengthUnit)}," +
                    $"{p1.Y.LengthFromSI(lengthUnit)},{p2.Y.LengthFromSI(lengthUnit)}, {p3.Y.LengthFromSI(lengthUnit)}, {p4.Y.LengthFromSI(lengthUnit)}";
            }

            return extremePoints;
        }

        /***************************************************/

        private static double CulmalativeDepth(List<ICurve> perimeters)
        {
            //Calculate total depth of section
            BoundingBox outerBounds = perimeters[0].IBounds();

            // Create a line at the centroid
            double centreX = perimeters[0].ICentroid().X;
            Point bottom = new Point() { X = centreX, Y = outerBounds.Min.Y };
            Point top = new Point() { X = centreX, Y = outerBounds.Max.Y };
            Line centroid = Engine.Geometry.Create.Line(bottom,top);

            double depth = 0;

            List<Point> pts = centroid.ICurveIntersections(perimeters[0]).OrderBy(pt => pt.Y).ToList();

            // Calculate depth for the outer polyline
            CalculateDepth(ref depth, pts, perimeters[0], true);

            // Iterate over the inner polyline curves
            for (int j = 1; j < perimeters.Count; j++)
            {
                // Calculate depth reduction because of openings
                List<Point> innerPoints = centroid.ICurveIntersections(perimeters[j]).OrderBy(pt => pt.Y).ToList();
                CalculateDepth(ref depth, innerPoints, perimeters[j], false);
            }

            // Divide by count-1 to average out the depth
            return depth;
        }

        private static void CalculateDepth(ref double depth, List<Point> pts, ICurve curve, bool addition)
        {
            // Iterate over the points provided 
            for (int i = 0; i < pts.Count - 1; i++)
            {
                //Get a point between sequential points, and get the midpoint - if it's within the curve then the distance between the points can be added to the depth
                if (curve.IIsContaining(new List<Point>() { Engine.Geometry.Create.Point(pts[i].X + (pts[i + 1].X - pts[i].X), pts[i].Y + (pts[i + 1].Y - pts[i].Y), pts[i].Z + pts[i + 1].Z - pts[i].Z) }))
                {
                    if (addition)
                        depth += pts[i + 1].Y - pts[i].Y;
                    else
                        depth -= pts[i + 1].Y - pts[i].Y;
                }
            }
        }

        private static double CulmalativeWidth(List<ICurve> perimeters)
        {
            //Calculate total width of section
            BoundingBox outerBounds = perimeters[0].IBounds();

            // Create a line at the centroid
            double centreY = perimeters[0].ICentroid().Y;
            Point left = new Point() { X = outerBounds.Min.X, Y = centreY };
            Point right = new Point() { X = outerBounds.Max.X, Y = centreY };
            Line centroid = Engine.Geometry.Create.Line(left, right);

            double depth = 0;

            List<Point> pts = centroid.ICurveIntersections(perimeters[0]).OrderBy(pt => pt.X).ToList();

            // Calculate width for the outer polyline
            CalculateWidth(ref depth, pts, perimeters[0], true);

            // Iterate over the inner polyline curves
            for (int j = 1; j < perimeters.Count; j++)
            {
                // Calculate depth reduction because of openings
                List<Point> innerPoints = centroid.ICurveIntersections(perimeters[j]).OrderBy(pt => pt.X).ToList();
                CalculateWidth(ref depth, innerPoints, perimeters[j], false);
            }

            // Divide by count-1 to average out the depth
            return depth;
        }

        private static void CalculateWidth(ref double width, List<Point> pts, ICurve curve, bool addition)
        {
            // Iterate over the points provided 
            for (int i = 0; i < pts.Count - 1; i++)
            {
                //Get a point between sequential points, and get the midpoint - if it's within the curve then the distance between the points can be added to the depth
                if (curve.IIsContaining(new List<Point>() { Engine.Geometry.Create.Point(pts[i].X + (pts[i + 1].X - pts[i].X), pts[i].Y + (pts[i + 1].Y - pts[i].Y), pts[i].Z + pts[i + 1].Z - pts[i].Z) }))
                {
                    if (addition)
                        width += pts[i + 1].X - pts[i].X;
                    else
                        width -= pts[i + 1].X - pts[i].X;
                }
            }
        }

    }
}




