using System;
using System.Reflection;
using System.Windows.Forms;
using HarmonyLib;

using SharpDX;
using SharpDX.Windows;


[HarmonyPatch(typeof(Application), "ProductVersion", MethodType.Getter)]
class Patch_ProductVersion_Getter
{
    static void Postfix(ref string __result) => __result += " - SLX CommunityPatches";
}


/*

[HarmonyPatch(typeof(b6), "b", argumentTypes: new Type[] { })]  // parameterless method
public class PatchClass
{
    static void Postfix()
    {

    }
}
*/