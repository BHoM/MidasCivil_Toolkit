using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Base;
using System.Linq;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadGravityLoads(List<string> ids = null)
        {
            List<ILoad> bhomGravityLoads = new List<ILoad>();
            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            List<BHoMObject> objects = new List<BHoMObject>();
            objects.AddRange(ReadBars());
            objects.AddRange(ReadFEMeshses());

            Engine.Reflection.Compute.RecordWarning("Note: Midas applies Self Weight to all objects in a given loadcase");

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> gravityLoadText = GetSectionText(loadcase + "\\SELFWEIGHT");

                if (gravityLoadText.Count != 0)
                {
                    List<string> gravityLoads = new List<string>();

                    foreach (string gravityLoad in gravityLoadText)
                    {
                        List<string> delimitted = gravityLoad.Split(',').ToList();
                        delimitted.RemoveAt(0);
                        gravityLoads.Add(String.Join(",", delimitted));
                    }

                    for (int i=0; i< gravityLoads.Count; i++)
                    {
                        GravityLoad bhomGravityLoad = Engine.MidasCivil.Convert.ToBHoMGravityLoad(objects, gravityLoads[i], loadcase, loadcaseDictionary, i+1);
                        bhomGravityLoads.Add(bhomGravityLoad);
                    }
                }
            }

            return bhomGravityLoads;
        }
    }
}