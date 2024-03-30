using System;
using System.Reflection;

namespace DevL10N.Fix
{
	public static class TypeExtension
	{
		public static MethodInfo GetMethodWithoutFlag(this Type type, string methodName)
		{
			return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
		}
		public static MethodInfo GetMethodWithoutFlag(this Type type, string methodName, Type[] types)
		{
			return type.GetMethod(methodName, types);
		}
	}
}
