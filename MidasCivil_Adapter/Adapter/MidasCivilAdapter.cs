using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        //Add any applicable constructors here, such as linking to a specific file or anything else as well as linking to that file through the (if existing) com link via the API
        public MidasCivilAdapter(string filePath, bool active = false)
        {
            AdapterId = "MidasCivil_id";   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine
            if (active)
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new ArgumentException("No file path given");
                }
                else if (IsApplicationRunning())
                {
                    throw new Exception("MidasCivil process already running");
                }
                else
                {
                    System.Diagnostics.Process.Start(filePath);
                    string directory = Path.GetDirectoryName(filePath);
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string txtFile = directory + "\\" + fileName + ".txt";
                    string mctFile = directory + "\\" + fileName + ".mct";

                    if (File.Exists(txtFile))
                    {
                        midasText = File.ReadAllLines(txtFile).ToList();
                    }
                    else if (File.Exists(mctFile))
                    {
                        midasText = File.ReadAllLines(mctFile).ToList();
                    }
                }

                Config.SeparateProperties = true;   //Set to true to push dependant properties of objects before the main objects are being pushed. Example: push nodes before pushing bars
                Config.MergeWithComparer = true;    //Set to true to use EqualityComparers to merge objects. Example: merge nodes in the same location
                Config.ProcessInMemory = false;     //Set to false to to update objects in the toolkit during the push
                Config.CloneBeforePush = true;      //Set to true to clone the objects before they are being pushed through the software. Required if any modifications at all, as adding a software ID is done to the objects
                Config.UseAdapterId = true;         //Tag objects with a software specific id in the CustomData. Requires the NextIndex method to be overridden and implemented

            }
        }

        public static bool IsApplicationRunning()
        {
            return (Process.GetProcessesByName("CVlw").Length > 0) ? true : false;
        }

        public List<string> midasText;
        public string directory;

        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        //private SoftwareComLink m_softwareNameCom;

        /***************************************************/


    }
}
