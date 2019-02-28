using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadLoad(Type type, List<string> ids = null)
        {
            List<ILoad> readLoads = null;
            string typeName = type.Name;
            switch (typeName)
            {
                case "PointForce":
                    readLoads = ReadPointForces(ids as dynamic);
                    break;
                case "GravityLoad":
                    readLoads = ReadGravityLoads(ids as dynamic);
                    break;
                case "BarUniformlyDistributedLoad":
                    readLoads = ReadBarUniformlyDistributedLoads(ids as dynamic);
                    break;
                case "AreaUniformalyDistributedLoad":
                    readLoads = ReadAreaUniformlyDistributedLoads(ids as dynamic);
                    break;
                case "BarTemperatureLoad":
                    readLoads = ReadBarTemperatureLoads(ids as dynamic);
                    break;
                case "AreaTemperatureLoad":
                    readLoads = ReadAreaTemperatureLoads(ids as dynamic);
                    break;
                case "PointDisplacement":
                    readLoads = ReadPointDisplacements(ids as dynamic);
                    break;
                case "BarPointLoad":
                    readLoads = ReadBarPointLoads(ids as dynamic);
                    break;
                case "BarVaryingDistributedLoad":
                    readLoads = ReadBarVaryingDistributedLoads(ids as dynamic);
                    break;
            }

            return readLoads;
        }
    }
}
