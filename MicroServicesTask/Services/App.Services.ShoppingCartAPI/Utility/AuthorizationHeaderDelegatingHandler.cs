
using System.Net.Http.Headers;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;

namespace  App.Services.ShoppingCartApi.Utility
{
    public class AuthorizationHeaderDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationHeaderDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //This delegating handler will intercept only those requests that are made by CouponClient.
            //Note:Delegating handlers are on client side
            //We can leverage the HttpClient using delegating handler to pass the bearer token to the other requests.
            //We can retrieve the bearer token from the HttpContextAccessor

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}