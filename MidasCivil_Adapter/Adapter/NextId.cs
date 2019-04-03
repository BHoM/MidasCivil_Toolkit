using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Properties.Section;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override object NextId(Type type, bool refresh = false)
        {
            //Method that returns the next free index for a specific object type.
            //Software dependent which type of index to return. Could be int, string, Guid or whatever the specific software is using
            //At the point of index assignment, the objects have not yet been created in the target software. 
            //The if statement below is designed to grab the first free index for the first object being created and after that increment.

            //Change from object to what the specific software is using
            int index = 1;
            bool isString = false;

            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(Node))
                {
                    string section = "NODE";

                    if (ExistsSection(section))
                    {
                        List<Node> nodes = ReadNodes();
                        List<int> nodeID = new List<int>();
                        nodes.ForEach(x => nodeID.Add(Convert.ToInt32(x.CustomData[AdapterId].ToString())));
                        nodeID.Sort();
                        nodeID.Reverse();

                        if (!(nodes == null))
                        {
                            index = nodeID[0] + 1;
                        }
                        else
                        {
                            index = 1;
                        }
                    }
                    else
                    {
                        index = 1;
                    }

                }

                if (type == typeof(Bar))
                {
                    string section = "ELEMENT";

                    if (ExistsSection(section))
                    {
                        index = GetMaxID(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }

                }

                if (type == typeof(FEMesh))
                {
                    string section = "ELEMENT";

                    if (ExistsSection(section))
                    {
                        index = GetMaxID(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

                if (type == typeof(Loadcase))
                {
                    isString = true;
                }

                if (type == typeof(PointForce))
                {
                    isString = true;
                }

                if (type == typeof(GravityLoad))
                {
                    isString = true;
                }

                if (type == typeof(BarUniformlyDistributedLoad))
                {
                    isString = true;
                }

                if (type == typeof(BarVaryingDistributedLoad))
                {
                    isString = true;
                }

                if (type == typeof(BarPointLoad))
                {
                    isString = true;
                }

                if (type == typeof(ConstantThickness))
                {
                    string section = "THICKNESS";

                    if (ExistsSection(section))
                    {

                        index = GetMaxID(section) +1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

                if (type == typeof(SteelSection))
                {
                    string section = "SECTION";

                    if (ExistsSection(section))
                    {
                        index = GetMaxID(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

            }

            if (isString)
            {
                return null;
            }
            else
            {
                m_indexDict[type] = index;
                return index;
            }
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        //Change from object to the index type used by the specific software
        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();


        /***************************************************/
    }
}
