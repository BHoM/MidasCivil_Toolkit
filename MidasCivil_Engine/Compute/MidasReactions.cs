/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Excel_programme_app = Microsoft.Office.Interop.Excel.Application;
using Excel_programme_workbook = Microsoft.Office.Interop.Excel.Workbook;
using Excel_programme_worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace BH.oM.Structure.Element.Node
{
    public class Reactions
    {
        static List<BH.oM.Structure.Element.Node.Reactions> Main(string file_path)
        {

            string csv_file_path = ExcelToCsv(file_path);
            string[] csv_file = File.ReadAllLines(csv_file_path);
            var list = new List<Reactions>();

            foreach (string line in csv_file)
            {
                try
                {
                    string[] line_delminated = line.Split(',');
                    var Results = new BH.oM.Structure.Element.Node.Reactions(line_delminated);
                    list.Add(Results);
                }
                catch { }

            }
            return list;
        }

        public Reactions(string[] line_delminated)
        {
            try
            {
                string Name = line_delminated[3];
                double Node_Number = Convert.ToDouble(line_delminated[2]);
                double f_x_ = Convert.ToDouble(line_delminated[7]);
                double f_y_ = Convert.ToDouble(line_delminated[8]);
                double f_z_ = Convert.ToDouble(line_delminated[9]);
                double m_x_ = Convert.ToDouble(line_delminated[10]);
                double m_y_ = Convert.ToDouble(line_delminated[11]);
                double m_z_ = Convert.ToDouble(line_delminated[12]);
                double m_b_ = Convert.ToDouble(line_delminated[13]);

                this.Name = Name;
                this.Node = Node_Number;
                this.Fx = f_x_;
                this.Fy = f_y_;
                this.Fz = f_z_;
                this.Mx = m_x_;
                this.My = m_y_;
                this.Mz = m_z_;
                this.Mb = m_b_;

            }
            catch { }


        }


        public string Name { get; private set; }
        public double Node { get; private set; }
        public double Fx { get; private set; }
        public double Fy { get; private set; }
        public double Fz { get; private set; }
        public double Mx { get; private set; }
        public double My { get; private set; }
        public double Mz { get; private set; }
        public double Mb { get; private set; }

        public static string ExcelToCsv(string file_path)
        {
            //Only run function when boolean toggle is true.
            string new_csv_name = new CSV_NAME(file_path).file_path;
            if (File.Exists(new_csv_name))
            {
                string csv_file_path = new_csv_name;
                return csv_file_path;
            }
            else
            {
                //Set new application as excel application
                Excel_programme_app excel = new Excel_programme_app();

                //Open the excel file as a background process
                Excel_programme_workbook xls_file = excel.Workbooks.Open(file_path);

                //Get the first sheet from the file, as csv's are only one sheet.
                Excel_programme_worksheet sheet_1 = (Microsoft.Office.Interop.Excel.Worksheet)xls_file.Sheets[1];

                //Save the sheet with the csv name, and as csv file type. Use default for rest.
                sheet_1.SaveAs(
                    new_csv_name,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing
                );

                //Close the excel file process
                xls_file.Close();

                //"return" the file path name to pass to the next function
                string csv_file_path = new_csv_name;
                return csv_file_path;
            }

        }

        /*This function checks to see if the file ends in a valid excel file format
        and returns the name with .csv instead, this function does not change the file
        itself. */
        class CSV_NAME
        {
            public string file_path;

            public CSV_NAME(string file_path)
            {
                string file_path_new = "";
                string new_csv_name = "";
                int length_name = file_path.Length;

                //This is an array of the various excel file formats capable of storing data,
                //Template formats are NOT included.
                String[] file_formats = new[] { "xlsx",
                                            "xlsm",
                                            "xls",
                                            "xlsb",
                                            "xlam",
                                            "xlm",
                                            "xla",
                                            "xlw",
                                            "xlr"
                                            };
                foreach (string file_format in file_formats)
                {
                    //Check four letter file types first
                    if (
                        file_path.Substring(length_name - 4, 4).Contains(file_format)
                    )
                    {
                        file_path_new = file_path.Substring(0, length_name - 4);
                    }

                    //if the file isnt a four letter file type check for three letters
                    else if (
                        file_path.Substring(length_name - 3, 3).Contains(file_format)
                    )
                    {
                        file_path_new = file_path.Substring(0, length_name - 3);
                    }
                }

                //Add csv to the name of the original file
                new_csv_name = file_path_new + ".csv";

                //return the name of the csv file.
                this.file_path = new_csv_name;
            }
        }
    }
}

