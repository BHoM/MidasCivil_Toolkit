using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<AreaUniformalyDistributedLoad> areaUniformlyDistributedLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (AreaUniformalyDistributedLoad areaUniformlyDistributedLoad in areaUniformlyDistributedLoads)
            {
                List<string> midasPressureLoads = new List<string>();
                string FEMeshLoadPath = CreateSectionFile(areaUniformlyDistributedLoad.Loadcase.Name + "\\PRESSURE");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(areaUniformlyDistributedLoad);

                List<IAreaElement> assignedElements = areaUniformlyDistributedLoad.Objects.Elements;

                List<string> assignedFEMeshes = new List<string>();
				
                foreach (IAreaElement mesh in assignedElements)
                {
                    assignedFEMeshes.Add(mesh.CustomData[AdapterId].ToString());
                }

                List<double> loadVectors = new List<double> { areaUniformlyDistributedLoad.Pressure.X,
                                                              areaUniformlyDistributedLoad.Pressure.Y,
                                                              areaUniformlyDistributedLoad.Pressure.Z};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 3; i++)
                {
                    areaUniformlyDistributedLoad.Pressure = zeroVector;

                    if (loadVectors[i] != 0)
                    {
                        areaUniformlyDistributedLoad.Pressure = createSingleComponentVector(i, loadVectors[i]);

                            foreach (string assignedFEMesh in assignedFEMeshes)
                            {
                                midasPressureLoads.Add(Engine.MidasCivil.Convert.ToMCAreaUniformlyDistributedLoad(areaUniformlyDistributedLoad, assignedFEMesh));
                            }
                    }
                }

                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveLoadEnd(FEMeshLoadPath);
                File.AppendAllLines(FEMeshLoadPath, midasPressureLoads);
            }

            return true;
        }

    }
}
