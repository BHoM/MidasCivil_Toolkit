using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadAreaTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomAreaTemperatureLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> AreaTemperatureLoadText = GetSectionText(loadcase + "\\ELTEMPER");

                if (AreaTemperatureLoadText.Count != 0)
                {
                    List<string> feMeshComparison = new List<string>();
                    List<string> loadedFEMeshes = new List<string>();

                    foreach (string AreaTemperatureLoad in AreaTemperatureLoadText)
                    {
                        List<string> delimitted = AreaTemperatureLoad.Split(',').ToList();
                        loadedFEMeshes.Add(delimitted[0].Replace(" ", ""));
                        delimitted.RemoveAt(0);
                        feMeshComparison.Add(String.Join(",", delimitted));
                    }

                    if (feMeshComparison.Count != 0)
                    {
                        List<FEMesh> bhomMeshes = ReadFEMeshes();
                        Dictionary<string, FEMesh> FEMeshDictionary = bhomMeshes.ToDictionary(
                                                                    x => x.CustomData[AdapterId].ToString());
                        List<string> distinctFEMeshLoads = feMeshComparison.Distinct().ToList();

                        foreach (string distinctFEMeshLoad in distinctFEMeshLoads)
                        {
                            List<int> indexMatches = feMeshComparison.Select((meshload, index) => new { meshload, index })
                                                       .Where(x => string.Equals(x.meshload, distinctFEMeshLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingFEMeshes = new List<string>();
                            indexMatches.ForEach(x => matchingFEMeshes.Add(loadedFEMeshes[x]));

                            AreaTemperatureLoad bhomAreaTemperatureLoad =
                                Engine.MidasCivil.Convert.ToBHoMAreaTemperatureLoad(
                                    distinctFEMeshLoad, matchingFEMeshes, loadcase, loadcaseDictionary, FEMeshDictionary, i);

                            if (bhomAreaTemperatureLoad!=null)
                                bhomAreaTemperatureLoads.Add(bhomAreaTemperatureLoad);

                            if ((string.IsNullOrWhiteSpace(distinctFEMeshLoad.Split(',')[1].ToString())))
                            {
                                i = i + 1;
                            }

                        }

                    }
                }
            }
            return bhomAreaTemperatureLoads;
        }

    }
}