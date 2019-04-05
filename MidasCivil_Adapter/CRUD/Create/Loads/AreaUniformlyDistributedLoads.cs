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

            foreach (AreaUniformalyDistributedLoad areaUniformalyDistributedLoad in areaUniformlyDistributedLoads)
            {
                List<string> midasAreaLoads = new List<string>();
                string FEMeshLoadPath = CreateSectionFile(areaUniformalyDistributedLoad.Loadcase.Name + "\\PRESSURE");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(areaUniformalyDistributedLoad);

                List<IAreaElement> assignedElements = areaUniformalyDistributedLoad.Objects.Elements;
                List<string> assignedFEMeshes = new List<string>();

                foreach (IAreaElement mesh in assignedElements)
                {
                    assignedFEMeshes.Add(mesh.CustomData[AdapterId].ToString());
                }

                PanelPlanar panel = new PanelPlanar();

                

                List<double> loadVectors = new List<double> { areaUniformalyDistributedLoad.Pressure.X,
                                                              areaUniformalyDistributedLoad.Pressure.Y,
                                                              areaUniformalyDistributedLoad.Pressure.Z,};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 3; i++)
                {
                    areaUniformalyDistributedLoad.Pressure = zeroVector;

                    if (loadVectors[i] != 0)
                    {
                        areaUniformalyDistributedLoad.Pressure = createSingleComponentVector(i, loadVectors[i]);

                            foreach (string assignedFEMesh in assignedFEMeshes)
                            {
                                midasAreaLoads.Add(Engine.MidasCivil.Convert.ToMCAreaUniformlyDistributedLoad(areaUniformalyDistributedLoad, assignedFEMesh));
                            }
                    }
                }

                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveLoadEnd(FEMeshLoadPath);
                File.AppendAllLines(FEMeshLoadPath, midasAreaLoads);
            }

            return true;
        }
    }
}
