using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApiDotNet.Services
{   

    public class GoogleCalendar
    {
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "CalendarApi";

        public static void AddToCalendar(DateTime fromDate, DateTime toDate)
        {
            //todo : call google calendar service
        }

        //public async Task AddToCalendar(HttpContext context, string userId)
        //{
        //    var token = await context.Authentication.GetTokenAsync("access_token");
        //    var flow = CreateFlow(Scopes);
        //    UserCredential credential = new UserCredential(flow, userId, token);
        //    var service = new CalendarService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = ApplicationName,
        //    });
        //}

        //private AuthorizationCodeFlow CreateFlow(IEnumerable<string> scopes)
        //{
        //    return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
        //    {
        //        ClientSecrets = new Google.Apis.Auth.OAuth2.ClientSecrets
        //        {
        //            ClientId = "717025442383-9ghkuepeofl7hka4bb2cuul92utkhi7u.apps.googleusercontent.com",
        //            ClientSecret = "gMDtlyobLHfZqKs3TiMxx0FP"
        //        },
        //        DataStore = new FileDataStore("./GoogleApi"),
        //        Scopes = scopes
        //    });
        //}
    }    
}
