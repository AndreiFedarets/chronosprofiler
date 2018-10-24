using System;
using System.Collections.Generic;

namespace Adenium.Layouting
{
    public static class LayoutPathResolver
    {
        private static readonly List<ILayoutPathResolver> Resolvers;

        static LayoutPathResolver()
        {
            Resolvers = new List<ILayoutPathResolver>();
            Resolvers.Add(new DefaultLayoutPathResolver());
        }

        public static void RegisterResolver(ILayoutPathResolver resolver)
        {
            lock (Resolvers)
            {
                if (!Resolvers.Contains(resolver))
                {
                    Resolvers.Add(resolver);   
                }
            }
        }

        public static List<string> ResolveLayouts(Type viewModelType)
        {
            List<string> paths = new List<string>();
            lock (Resolvers)
            {
                foreach (ILayoutPathResolver resolver in Resolvers)
                {
                    foreach (string fileFullName in resolver.Resolve(viewModelType))
                    {
                        string temp = fileFullName.ToLowerInvariant();
                        if (!paths.Contains(temp))
                        {
                            paths.Add(temp);
                        }
                    }
                }
            }
            return paths;
        }
    }
}
