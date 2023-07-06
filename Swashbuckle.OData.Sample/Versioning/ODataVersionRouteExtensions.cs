﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData.Edm;

namespace SwashbuckleODataSample.Versioning
{
    public static class ODataVersionRouteExtensions
    {
        /// <summary>
        /// Map odata route with query string or header constraints
        /// </summary>
        public static void MapODataServiceRoute(
            this HttpRouteCollection routes,
            string routeName,
            string routePrefix,
            IEdmModel model,
            object queryConstraints,
            object headerConstraints)
        {
            MapODataServiceRoute(
                routes,
                routeName,
                routePrefix,
                model,
                new DefaultODataPathHandler(),
                ODataRoutingConventions.CreateDefault(),
                queryConstraints,
                headerConstraints);
        }

        /// <summary>
        /// Map odata route with query string or header constraints
        /// </summary>
        public static void MapODataServiceRoute(
            this HttpRouteCollection routes,
            string routeName,
            string routePrefix,
            IEdmModel model,
            IODataPathHandler pathHandler,
            IEnumerable<IODataRoutingConvention> routingConventions,
            object queryConstraints,
            object headerConstraints)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            string routeTemplate = string.IsNullOrEmpty(routePrefix) ? ODataRouteConstants.ODataPathTemplate : routePrefix + "/" + ODataRouteConstants.ODataPathTemplate;
            ODataVersionRouteConstraint routeConstraint = new ODataVersionRouteConstraint(routeName, queryConstraints, headerConstraints);
            var constraints = new HttpRouteValueDictionary();
            constraints.Add(ODataRouteConstants.ConstraintName, routeConstraint);
            routes.MapHttpRoute(
                routeName,
                routeTemplate, null, constraints);
        }
    }
}