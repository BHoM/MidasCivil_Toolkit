using BH.oM.Structure.Loads;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCAreaTemperatureLoad(this AreaTemperatureLoad FEMeshLoad, string assignedFEMesh)
        {
            string midasFEMeshLoad = null;

            midasFEMeshLoad = assignedFEMesh + "," + FEMeshLoad.TemperatureChange.ToString() + "," + FEMeshLoad.Name;

            return midasFEMeshLoad;
        }
    }
}