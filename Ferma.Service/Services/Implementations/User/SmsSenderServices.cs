using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class SmsSenderServices : ISmsSenderServices
    {
        public async Task<bool> SmsSend(string number, string code)
        {
            //Mesaj məzmunu
            string newText = "Kod: " + code + " Etibarliliq muddeti - 10 deqiqe. https://minimall.az/";
            // XML məzmunu 
            string xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <SMS-InsRequest>
                                    <CLIENT user=""demoapi"" pwd=""bVLj48xz"" from=""SOFTLINE""/>
                                    <INSERTMSG text=""metn"">
                                        <TO>nomre</TO>
                                    </INSERTMSG>
                                </SMS-InsRequest>";
            xmlContent = xmlContent.Replace("metn", newText);
            xmlContent = xmlContent.Replace("nomre", number);
            // HttpClient 
            using (HttpClient client = new HttpClient())
            {
                // İstek məzmununu ayarla
                StringContent content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                // İstek göndər
                HttpResponseMessage response = await client.PostAsync("https://gw.soft-line.az/sendsms", content);

                // Response yoxlanması
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    throw new SmsSenderException("Xəta baş verdi!");
            }

        }
    }
}
