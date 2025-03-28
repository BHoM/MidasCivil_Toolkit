﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using System.Net.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public async Task<HttpResponseMessage> SendRequestAsync(string endpoint, HttpMethod method, string jsonPayload = "")
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://moa-engineers.midasit.com:443/civil/")
            };
            client.DefaultRequestHeaders.Add("mapi-key", m_mapiKey);

            var request = new HttpRequestMessage(method, endpoint);

            if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete)
            {
                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            }
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }

            catch (HttpRequestException e)
            {
                Engine.Base.Compute.RecordError("Something went wrong with the request. Make sure the API is active and the mapikey is correct. If this does not solve the issue, try using the MCT command shell by remvoing NX from the MidasCivil version.");

                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent($"Error: {e.Message}")
                };
            }
        }
    }
}
