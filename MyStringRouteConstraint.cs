using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLibrary;
public class MyStringRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        // Implement your custom logic to determine if the route value matches the constraint
        // For example, you might check if the value is a string and meets certain criteria
        // For demonstration purposes, let's assume any string is considered a match
        if (values.TryGetValue(routeKey, out object? routeValue))
        {
            return routeValue is string;
        }

        return false;
    }
}
