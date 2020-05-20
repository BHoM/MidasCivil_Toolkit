/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.Adapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using BH.Engine.Base.Objects;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.Engine.Adapters.MidasCivil.Comparer;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        //Add any applicable constructors here, such as linking to a specific file or anything else as well as linking to that file through the (if existing) com link via the API
        public MidasCivilAdapter(string filePath, bool active = false, string version = "")
        {
            if (active)
            {
                AdapterIdName = "MidasCivil_id";   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine

                Modules.Structure.ModuleLoader.LoadModules(this);

                AdapterComparers = new Dictionary<Type, object>
                {
                    {typeof(Node), new Engine.Structure.NodeDistanceComparer(3) },   //The 3 in here sets how many decimal places to look at for node merging. 3 decimal places gives mm precision
                    {typeof(Bar), new BarMidPointComparer(3) },
                    {typeof(FEMesh), new MeshCentreComparer() },
                    {typeof(Constraint6DOF), new BHoMObjectNameComparer() },
                    {typeof(RigidLink), new BHoMObjectNameComparer() },
                    {typeof(BarRelease), new BHoMObjectNameComparer() },
                    {typeof(SteelSection), new BHoMObjectNameComparer() },
                    {typeof(ISectionProperty), new BHoMObjectNameComparer() },
                    {typeof(Steel), new BHoMObjectNameComparer() },
                    {typeof(Concrete), new BHoMObjectNameComparer() },
                    {typeof(IMaterialFragment), new BHoMObjectNameComparer() },
                    {typeof(LinkConstraint), new BHoMObjectNameComparer() },
                    {typeof(ConstantThickness), new BHoMObjectNameComparer() },
                    {typeof(ISurfaceProperty), new BHoMObjectNameComparer() },
                    {typeof(Loadcase), new BHoMObjectNameComparer() },
                    {typeof(PointLoad), new BHoMObjectNameComparer() },
                    {typeof(GravityLoad), new BHoMObjectNameComparer() },
                    {typeof(BarUniformlyDistributedLoad), new BHoMObjectNameComparer() },
                    {typeof(BarVaryingDistributedLoad), new BHoMObjectNameComparer() },
                    {typeof(BarPointLoad), new BHoMObjectNameComparer() },
                    {typeof(AreaUniformlyDistributedLoad), new BHoMObjectNameComparer() },
                    {typeof(AreaTemperatureLoad), new BHoMObjectNameComparer() },
                    {typeof(BarTemperatureLoad), new BHoMObjectNameComparer() },
                    {typeof(LoadCombination), new BHoMObjectNameComparer() },
                };

                DependencyTypes = new Dictionary<Type, List<Type>>
                {
                    {typeof(Node), new List<Type> { typeof(Constraint6DOF)} },
                    {typeof(Bar), new List<Type> { typeof(Node) , typeof(ISectionProperty), typeof(BarRelease) } },
                    {typeof(FEMesh), new List<Type> { typeof(Node), typeof(ISurfaceProperty) } },
                    {typeof(ISectionProperty), new List<Type> { typeof(IMaterialFragment) } },
                    {typeof(RigidLink), new List<Type> { typeof(Node) } },
                    {typeof(ISurfaceProperty), new List<Type> { typeof(IMaterialFragment) } },
                    {typeof(ILoad), new List<Type> {typeof(Loadcase) } }
                };

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("No file path given");
                }
                else if (IsApplicationRunning())
                {
                    throw new Exception("MidasCivil process already running");
                }
                else
                {
                    try
                    {
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch (System.ComponentModel.Win32Exception)
                    {
                        throw new Exception("File does not exist, please reference an .mcb file");
                    }
                    directory = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string txtFile = directory + "\\" + fileName + ".txt";
                    string mctFile = directory + "\\" + fileName + ".mct";

                    if (File.Exists(txtFile))
                    {
                        midasText = File.ReadAllLines(txtFile).ToList();
                        SetSectionText();
                    }
                    else if (File.Exists(mctFile))
                    {
                        midasText = File.ReadAllLines(mctFile).ToList();
                        SetSectionText();
                    }
                    string versionFile = directory + "\\TextFiles\\" + "VERSION" + ".txt";
                    midasCivilVersion = "8.8.1";

                    if (!(version == ""))
                    {
                        midasCivilVersion = version.Trim();
                        if (File.Exists(versionFile))
                        {
                            Engine.Reflection.Compute.RecordWarning("*VERSION file found, user input used to overide: version =  " + midasCivilVersion);
                        }
                    }
                    else if (File.Exists(versionFile))
                    {
                        List<string> versionText = GetSectionText("VERSION");
                        midasCivilVersion = versionText[0].Trim();
                    }
                    else
                    {
                        Engine.Reflection.Compute.RecordWarning("*VERSION file not found in directory and no version specified, MidasCivil version assumed default value =  " + midasCivilVersion);
                    }

                    try
                    {
                        List<string> units = GetSectionText("UNITS");
                        forceUnit = units[0].Split(',')[0].Trim();
                        lengthUnit = units[1].Split(',')[0].Trim();
                        heatUnit = units[2].Split(',')[0].Trim();
                        temperatureUnit = units[3].Split(',')[0].Trim();
                    }
                    catch(DirectoryNotFoundException)
                    {
                        Engine.Reflection.Compute.RecordWarning(
                            "No UNITS.txt file found, MidasCivil model units assumed to be Newtons, metres, calories and celcius. Therefore, no unit conversion will occur when pushing and pulling to/from MidasCivil.");
                    }
                }
            }
        }

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("CVlw").Length > 0) ? true : false;
        }

        public List<string> midasText;
        public string directory;
        public string midasCivilVersion;
        public string forceUnit;
        public string lengthUnit;
        public string heatUnit;
        public string temperatureUnit;
        private Dictionary<Type, Dictionary<int, HashSet<string>>> m_tags = new Dictionary<Type, Dictionary<int, HashSet<string>>>();


        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        //private SoftwareComLink m_softwareNameCom;

        /***************************************************/
    }
}
