using System;
using System.Reflection;
using System.Web.Mvc;


namespace NEW_Mfile_Project.Models
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowMultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            isValidName = controllerContext.HttpContext.Request[Name] != null &&
                controllerContext.HttpContext.Request[Name] == Argument;

            return isValidName;
        }
    }
}