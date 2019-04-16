using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Common.Materials;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Surface;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;        //boolean returning if the creation was successfull or not

            if (objects.Count() > 0)
            {
                if (objects.First() is Node)
                {
                    success = CreateCollection(objects as IEnumerable<Node>);
                }
                if (objects.First() is Material)
                {
                    success = CreateCollection(objects as IEnumerable<Material>);
                }
                if (objects.First() is Constraint6DOF)
                {
                    success = CreateCollection(objects as IEnumerable<Constraint6DOF>);
                }
                if (objects.First() is Bar)
                {
                    success = CreateCollection(objects as IEnumerable<Bar>);
                }
                if (objects.First() is FEMesh)
                {
                    success = CreateCollection(objects as IEnumerable<FEMesh>);
                }
                if (objects.First() is RigidLink)
                {
                    success = CreateCollection(objects as IEnumerable<RigidLink>);
                }
                if (objects.First() is Loadcase)
                {
                    success = CreateCollection(objects as IEnumerable<Loadcase>);
                }
                if (objects.First() is LoadCombination)
                {
                    success = CreateCollection(objects as IEnumerable<LoadCombination>);
                }
                if (typeof(ILoad).IsAssignableFrom(objects.First().GetType()))
                {
                    string loadType = objects.First().GetType().ToString();

                    switch (loadType)
                    {
                        case "BH.oM.Structure.Loads.PointForce":
                            success = CreateCollection(objects as IEnumerable<PointForce>);
                            break;
                        case "BH.oM.Structure.Loads.GravityLoad":
                            success = CreateCollection(objects as IEnumerable<GravityLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarUniformlyDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<BarUniformlyDistributedLoad>);
                            break;
                        case "BH.oM.Structure.Loads.AreaUniformalyDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<AreaUniformalyDistributedLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarTemperatureLoad":
                            success = CreateCollection(objects as IEnumerable<BarTemperatureLoad>);
                            break;
                        case "BH.oM.Structure.Loads.AreaTemperatureLoad":
                            success = CreateCollection(objects as IEnumerable<AreaTemperatureLoad>);
                            break;
                        case "BH.oM.Structure.Loads.PointDisplacement":
                            success = CreateCollection(objects as IEnumerable<PointDisplacement>);
                            break;
                        case "BH.oM.Structure.Loads.BarPointLoad":
                            success = CreateCollection(objects as IEnumerable<BarPointLoad>);
                            break;
                        case "BH.oM.Structure.Loads.BarVaryingDistributedLoad":
                            success = CreateCollection(objects as IEnumerable<BarVaryingDistributedLoad>);
                            break;
                    }
                }
                if (objects.First() is ISectionProperty)
                {
                    success = CreateCollection(objects as IEnumerable<ISectionProperty>);
                }
                if (objects.First() is ISurfaceProperty)
                {
                    success = CreateCollection(objects as IEnumerable<ISurfaceProperty>);
                }
            }

                return success;
        }

    }
}
