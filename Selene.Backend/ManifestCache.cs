using System;
using System.Collections.Generic;

namespace Selene.Backend
{
    internal static class ManifestCache
    {
        static Dictionary<Type, ControlManifest> Cache;

        static ManifestCache()
        {
            Cache = new Dictionary<Type, ControlManifest>();
        }

        public static void Add(Type T, ControlManifest Manifest)
        {
            Cache.Add(T, Manifest);
        }

        public static ControlManifest Retreive(Type T)
        {
            if(!Cache.ContainsKey(T))
            {
                ControlManifest Ret;
                Add(T, Ret = Introspector.Inspect(T));
                return Ret;
            }
            else return Cache[T];
        }
    }
}
