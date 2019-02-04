using BH.oM.Common.Materials;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Surface;
using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** BHoM Adapter Interface                    ****/
        /***************************************************/

        //Standard implementation for dependency types (change the dictionary below to override):

        protected override List<Type> DependencyTypes<T>()
        {
            Type type = typeof(T);

            if (m_DependencyTypes.ContainsKey(type))
                return m_DependencyTypes[type];

            else if (type.BaseType != null && m_DependencyTypes.ContainsKey(type.BaseType))
                return m_DependencyTypes[type.BaseType];

            else
            {
                foreach (Type interType in type.GetInterfaces())
                {
                    if (m_DependencyTypes.ContainsKey(interType))
                        return m_DependencyTypes[interType];
                }
            }


            return new List<Type>();
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private static Dictionary<Type, List<Type>> m_DependencyTypes = new Dictionary<Type, List<Type>>
        {
            {typeof(Node), new List<Type> { typeof(Constraint6DOF)} },
            { typeof(Bar), new List<Type> { typeof(Node) } },
            {typeof(ISectionProperty), new List<Type> { typeof(Material) } },
            {typeof(RigidLink), new List<Type> { typeof(LinkConstraint), typeof(Node) } },
            {typeof(MeshFace), new List<Type> { typeof(ISurfaceProperty), typeof(Node) } },
            {typeof(ISurfaceProperty), new List<Type> { typeof(Material) } },
            {typeof(PanelPlanar), new List<Type> { typeof(ISurfaceProperty) } }
        };


        /***************************************************/
    }
}
