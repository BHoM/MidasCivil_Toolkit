using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadAreaUniformlyDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomAreaUniformlyDistributedLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> AreaUniformlyDistributedLoadText = GetSectionText(loadcase + "\\PRESSURE");

                if (AreaUniformlyDistributedLoadText.Count != 0)
                {
                    List<string> feMeshComparison = new List<string>();
                    List<string> loadedFEMeshes = new List<string>();

                    foreach (string AreaUniformlyDistributedLoad in AreaUniformlyDistributedLoadText)
                    {
                        List<string> delimitted = AreaUniformlyDistributedLoad.Split(',').ToList();
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

                            AreaUniformlyDistributedLoad bhomAreaUniformlyDistributedLoad =
                                Engine.MidasCivil.Convert.ToBHoMAreaUniformlyDistributedLoad(
                                    distinctFEMeshLoad, matchingFEMeshes, loadcase, loadcaseDictionary, FEMeshDictionary, i);

                            bhomAreaUniformlyDistributedLoads.Add(bhomAreaUniformlyDistributedLoad);

                            if ((string.IsNullOrWhiteSpace(distinctFEMeshLoad.Split(',')[13].ToString())))
                            {
                                i = i + 1;
                            }

                        }

                    }
                }
            }
            return bhomAreaUniformlyDistributedLoads;
        }

    }
}