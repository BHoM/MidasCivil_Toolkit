using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.IO;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<AreaTemperatureLoad> areaTemperatureLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (AreaTemperatureLoad areaTemperatureLoad in areaTemperatureLoads)
            {
                List<string> midasTemperatureLoads = new List<string>();
                string FEMeshLoadPath = CreateSectionFile(areaTemperatureLoad.Loadcase.Name + "\\ELTEMPER");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(areaTemperatureLoad);

                List<IAreaElement> assignedElements = areaTemperatureLoad.Objects.Elements;

                List<string> assignedFEMeshes = new List<string>();

                foreach (IAreaElement mesh in assignedElements)
                {
                    assignedFEMeshes.Add(mesh.CustomData[AdapterIdName].ToString());
                }

                foreach (string assignedFEMesh in assignedFEMeshes)
                {
                    midasTemperatureLoads.Add(Engine.MidasCivil.Convert.ToMCAreaTemperatureLoad(areaTemperatureLoad, assignedFEMesh));
                }

                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveLoadEnd(FEMeshLoadPath);
                File.AppendAllLines(FEMeshLoadPath, midasTemperatureLoads);
            }

            return true;
        }
    }
}
