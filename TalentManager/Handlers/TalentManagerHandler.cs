using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
namespace TalentManager.Handlers
{
    public class TalentManagerHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //***
            //Message handler
            //This is the very first point where the request arrives, once from here is passing thru il the controllers,
            //where we can add security at controller level or action level


            //// Inspect and do your stuff with request here
            //// If you are not happy for any reason,
            //// you can reject the request right here like this

            //bool isBadRequest = false;
            //if (isBadRequest)
            //    return request.CreateResponse(HttpStatusCode.BadRequest);

            //var response = await base.SendAsync(request, cancellationToken);
            //// Inspect and do your stuff with response here
            //return response;



            if (request.Method == HttpMethod.Post && request.Headers.Contains("X-HTTP-Method-Override"))
            {
                var method = request.Headers.GetValues("X-HTTP-Method-Override").FirstOrDefault();
                bool isPut = String.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase);
                bool isDelete = String.Equals(method, "DELETE", StringComparison.OrdinalIgnoreCase);
                if (isPut || isDelete)
                {
                    request.Method = new HttpMethod(method);
                }
            }
            return await base.SendAsync(request, cancellationToken);

        }
    }
}