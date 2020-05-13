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

using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public void SetSectionText()
        {
            List<int> sectionIndexes = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("*"))
                .Select(x => x.index)
                .ToList();

            List<int> loadcaseStarts = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("*USE-STLD"))
                .Select(x => x.index)
                .ToList();

            List<int> loadcaseEnds = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("; End of data for load case"))
                .Select(x => x.index)
                .ToList();

            List<int> loadcaseRange = new List<int>();

            for (int j = 0; j < loadcaseStarts.Count(); j++)
            {
                loadcaseRange.AddRange(Enumerable.Range(
                    loadcaseStarts[j] +1, loadcaseEnds[j] - loadcaseStarts[j] + 1));
            }

            List<int> loadSectionIndexes = loadcaseRange.Where(x => sectionIndexes.Contains(x)).ToList();

            for (int i = 0; i < sectionIndexes.Count() - 1; i++)
            {
                int sectionStart = sectionIndexes[i];
                int sectionEnd = sectionIndexes[i + 1] - 1;

                string sectionHeader = midasText[sectionStart];

                if (!(sectionHeader[0] == '*') || loadSectionIndexes.Contains(sectionStart))
                {
                    continue;
                }

                string sectionName = SectionName(sectionHeader);
                List<string> sectionText = midasText.GetRange(sectionStart, sectionEnd - sectionStart);

                if (loadcaseStarts.Contains(sectionStart))
                {
                    string loadcaseName = sectionHeader.Split(',')[1].Replace(" ","");
                    string path = directory + "\\TextFiles\\" + loadcaseName;
                    System.IO.Directory.CreateDirectory(path);
                    WriteSectionText(sectionText, sectionName, path);

                    int loadcaseEnd = loadcaseEnds[loadcaseStarts.IndexOf(sectionStart)];

                    for(int k = i+1; sectionIndexes[k] < loadcaseEnd; k++)
                    {
                        int loadStart = sectionIndexes[k];
                        int loadEnd = sectionIndexes[k + 1] - 1;
                        string loadHeader = midasText[loadStart];
                        List<string> loadText = midasText.GetRange(loadStart, loadEnd - loadStart);
                        if (!(loadHeader[0] == '*'))
                        {
                            continue;
                        }
                        string loadName = SectionName(midasText[loadStart]);
                        WriteSectionText(loadText, loadName, path);
                    }
                }
                else
                {
                    CreateSectionFile(sectionName);
                    WriteSectionText(sectionText, sectionName);
                }
            }
        }

        private static string SectionName(string text)
        {
            if (text.Contains(","))
            {
                return text.Split(',')[0].Split('*')[1];
            }
            else if (text.Contains(" "))
            {
                return text.Split(' ')[0].Split('*')[1];
            }
            else
            {
                return text.Split('*')[1];
            }
        }

    }
}