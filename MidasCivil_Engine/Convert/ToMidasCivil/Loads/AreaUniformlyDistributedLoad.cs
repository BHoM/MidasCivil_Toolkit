using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCAreaUniformlyDistributedLoad(this AreaUniformalyDistributedLoad FEMeshLoad, string assignedFEMesh)
        {
            string midasFEMeshLoad = null;

            string direction = MCDirectionConverter(FEMeshLoad.Pressure);
            midasFEMeshLoad = assignedFEMesh + ", PRES, PLATE, FACE, " + MCAxisConverter(FEMeshLoad.Axis) + direction +
                                                                    ", 0, 0, 0, " + MCProjectionConverter(FEMeshLoad.Projected) +
                                                                    ", " + MCVectorConverter(FEMeshLoad.Pressure, direction) +
                                                                    ", 0, 0, 0, 0, " + FEMeshLoad.Name;

            return midasFEMeshLoad;
        }

    }
}