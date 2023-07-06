﻿using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData;
using Microsoft.OData.UriParser;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using ODataPath = Microsoft.AspNet.OData.Routing.ODataPath;

namespace SwashbuckleODataSample.Versioning
{
    /// <summary>
    /// Helper class to facilitate building an odata service.
    /// </summary>
    public static class ODataHelper
    {
        /// <summary>
        /// Helper method to get the odata path for an arbitrary odata uri.
        /// </summary>
        /// <param name="request">The request instance in current context</param>
        /// <param name="uri">OData uri</param>
        /// <returns>The parsed odata path</returns>
        public static ODataPath CreateODataPath(this HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var newRequest = new HttpRequestMessage(HttpMethod.Get, uri);
            var route = request.GetRouteData().Route;

            var newRoute = new HttpRoute(
                route.RouteTemplate,
                new HttpRouteValueDictionary(route.Defaults),
                new HttpRouteValueDictionary(route.Constraints),
                new HttpRouteValueDictionary(route.DataTokens),
                route.Handler);
            var routeData = newRoute.GetRouteData(request.GetConfiguration().VirtualPathRoot, newRequest);
            if (routeData == null)
            {
                throw new InvalidOperationException("The link is not a valid odata link.");
            }
            // cast nicht möglich
            return newRequest.ODataProperties().Path;
        }

        /// <summary>
        /// Helper method to get the key value from a uri.
        /// Usually used by $link action to extract the key value from the url in body.
        /// </summary>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <param name="request">The request instance in current context</param>
        /// <param name="uri">OData uri that contains the key value</param>
        /// <returns>The key value</returns>
        public static TKey GetKeyValue<TKey>(this HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            //get the odata path Ex: ~/entityset/key/$links/navigation
            var odataPath = request.CreateODataPath(uri);
            var keySegment = odataPath.Segments.OfType<KeySegment>().FirstOrDefault();
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Keys.SingleOrDefault().Value as string, ODataVersion.V4);
            return (TKey)value;
        }

        /// <summary>
        /// Convert model state errors into string value.
        /// </summary>
        /// <param name="modelState">Model state</param>
        /// <returns>String value which contains all model errors</returns>
        public static string GetModelStateErrorInformation(ModelStateDictionary modelState)
        {
            StringBuilder errorMessageBuilder = new StringBuilder();
            errorMessageBuilder.AppendLine("Invalid request received.");

            if (modelState != null)
            {
                foreach (var key in modelState.Keys)
                {
                    if (modelState[key].Errors.Count > 0)
                    {
                        errorMessageBuilder.AppendLine(key + ":" + (modelState[key].Value != null ? modelState[key].Value.RawValue : "null"));
                    }
                }
            }

            return errorMessageBuilder.ToString();
        }
    }
}
