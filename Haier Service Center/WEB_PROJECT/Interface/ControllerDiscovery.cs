using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

public class ControllerDiscovery
{
	public static IEnumerable<string> GetActionMethods(string controllerName)
	{
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();

		// Filter assemblies to include only those that reference MVC
		var mvcAssemblies = assemblies.Where(a => !a.IsDynamic && a.GetReferencedAssemblies()
								.Any(ra => ra.FullName.StartsWith("Microsoft.AspNetCore.Mvc")));

		// Get the controller types from the MVC assemblies
		var controllerTypes = mvcAssemblies
			.SelectMany(a => a.GetTypes())
			.Where(t => t.Name.Equals(controllerName +"Controller", StringComparison.OrdinalIgnoreCase));

		// Extract and return distinct action method names
		return controllerTypes
			.SelectMany(controllerType =>
			controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Default)
					//.Where(m => typeof(IActionResult).IsAssignableFrom(m.ReturnType))
					.Select(m => m.Name))
			.Distinct();
	}


	public static IEnumerable<string> GetControllerType()
    {
        // Get all loaded assemblies
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Filter assemblies to include only those that reference MVC
        var mvcAssemblies = assemblies.Where(a => !a.IsDynamic && a.GetReferencedAssemblies()
                                    .Any(ra => ra.FullName.StartsWith("Microsoft.AspNetCore.Mvc")));

        var controllerTypes = mvcAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.Name.Contains("Controller", StringComparison.OrdinalIgnoreCase) && !t.IsAbstract && t.IsSubclassOf(typeof(ControllerBase)))
            .ToList();


       // Get the names of the controller types
        var controllerTypeNames = controllerTypes.Select(t => t.Name.Replace("Controller", ""));

        return controllerTypeNames;
    }


}


