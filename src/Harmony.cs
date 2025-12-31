using System;
using System.IO;
using System.Reflection;
using HarmonyLib;

namespace hookdll
{
    public static class ScriptLoader
    {
        private static Harmony _harmony = null!;

        public static void Initialize()
        {
            // Redirect missing strong-named assemblies (System.Numerics.Vectors)
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            _harmony = new Harmony("CommunityPatches.scripts");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private static Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                var name = new AssemblyName(args.Name);

                if (name.Name == "System.Numerics.Vectors")
                {
                    // Path to the DLL next to hook.dll
                    string dllPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "System.Numerics.Vectors.dll");
                    if (File.Exists(dllPath))
                    {
                        return Assembly.LoadFrom(dllPath);
                    }
                }
            }
            catch
            {
                // ignored, fallback to default resolution
            }

            return null;
        }
    }
}
