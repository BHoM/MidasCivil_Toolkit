using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;


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
                        if (!(nodes == null))
                        {
                            nodes.Reverse();
                            index = Convert.ToInt32(nodes[0].CustomData[AdapterId]) + 1;
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
            }

            m_indexDict[type] = index;
            return index;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        //Change from object to the index type used by the specific software
        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();


        /***************************************************/
    }
}
