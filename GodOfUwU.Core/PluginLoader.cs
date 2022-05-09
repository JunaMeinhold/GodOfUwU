namespace GodOfUwU.Core
{
    using Discord;
    using System.Reflection;
    using System.Runtime.Loader;

    public static class PluginLoader
    {
        private const string PluginsPath = "./plugins/";
        private static AssemblyLoadContext context;
        private static readonly List<Assembly> loadedAssemblies = new();
        private static readonly List<Plugin> loadedPlugins = new();

        static PluginLoader()
        {
            context = new AssemblyLoadContext(nameof(PluginLoader), true);
        }

        public static IReadOnlyList<Plugin> Plugins => loadedPlugins;

        public static event Func<Task>? Loaded;

        public static event Func<Task>? Unloaded;

        public static event Func<Task>? BeginUnload;

        public static event Func<Task>? Reloaded;

        public static event Func<LogMessage, Task>? Log;

        public static bool RegisterCommands { get; set; }

        public static async Task Load()
        {
            if (!Directory.Exists(PluginsPath))
                Directory.CreateDirectory(PluginsPath);
            string[] files = Directory.GetFiles(PluginsPath, "*.dll");

            context.Resolving += Resolving;

            HashSet<PluginDesc> nodes = PluginDesc.LoadAll(PluginsPath).ToHashSet();
            HashSet<Tuple<PluginDesc, PluginDesc>> edges = ConstructEdges(nodes);
            List<PluginDesc> sorted = TopologicalSort(nodes, edges);

            foreach (PluginDesc desc in sorted)
            {
                if (desc.Path != null)
                    Register(desc);
            }

            //await UserContext.Current.UpdatePermissions();

            if (Loaded != null)
                await Loaded.Invoke();
        }

        private static Assembly? Resolving(AssemblyLoadContext context, AssemblyName name)
        {
            string path = Path.Combine(PluginsPath, (name.Name ?? string.Empty) + ".dll");
            if (File.Exists(path))
            {
                return context.LoadFromAssemblyPath(Path.GetFullPath(path));
            }
            return null;
        }

        private static void Register(PluginDesc desc)
        {
            string path = Path.Combine(PluginsPath, desc.Path);
            string? folder = Path.GetDirectoryName(path);
            string filename = Path.GetFileName(path);

            if (folder == null) return;

            string pdb = Path.Combine(folder, Path.GetFileNameWithoutExtension(filename) + ".pdb");
            Assembly assembly;
            if (File.Exists(pdb))
            {
                using MemoryStream ms = new(File.ReadAllBytes(path));
                using MemoryStream ms2 = new(File.ReadAllBytes(pdb));
                assembly = context.LoadFromStream(ms, ms2);
            }
            else
            {
                using MemoryStream ms = new(File.ReadAllBytes(path));
                assembly = context.LoadFromStream(ms);
            }

            try
            {
                Type? type = assembly.GetTypes().FirstOrDefault(x => x.IsAssignableTo(typeof(Plugin)));
                if (type is null)
                {
                    PostLogMessageInternal(LogSeverity.Critical, $"{desc.Name}, {desc.Version} failed to find plugin type!").Wait();
                    return;
                }

                Plugin? plugin = (Plugin?)Activator.CreateInstance(type);

                if (plugin is null)
                {
                    PostLogMessageInternal(LogSeverity.Critical, $"{desc.Name}, {desc.Version} failed to create plugin instance!").Wait();
                    return;
                }

                plugin.Name = desc.Name;
                plugin.Version = desc.Version;
                plugin.Assembly = assembly;

                foreach (string dep in desc.Dependencies)
                {
                    Plugin depend = loadedPlugins.First(x => x.Name == dep);
                    plugin.Dependencies.Add(depend);
                }

                loadedPlugins.Add(plugin);

                if (Log != null)
                    PostLogMessageInternal(LogSeverity.Info, $"{desc.Name}, {desc.Version} loaded!").Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task Unload()
        {
            foreach (Plugin plugin in loadedPlugins)
            {
                await plugin.Uninitialize();
                if (Log != null)
                    await PostLogMessageInternal(LogSeverity.Info, $"{plugin.Name}, {plugin.Version} unloaded!");
            }
            if (BeginUnload != null)
                await BeginUnload.Invoke();
            loadedAssemblies.Clear();
            loadedPlugins.Clear();
            context.Unload();
            context = new(nameof(PluginLoader), true);
            if (Unloaded != null)
                await Unloaded.Invoke();
        }

        public static async Task Reload()
        {
            await Unload();
            await Load();
            if (Reloaded != null)
                await Reloaded.Invoke();
        }

        internal static async Task PostLogMessageInternal(LogSeverity severity, string message)
        {
            if (Log != null)
                await Log.Invoke(new(severity, "Plugins", message));
        }

        public static async Task PostLogMessage(LogMessage message)
        {
            if (Log != null)
                await Log.Invoke(message);
        }

        private static HashSet<Tuple<PluginDesc, PluginDesc>> ConstructEdges(IEnumerable<PluginDesc> descs)
        {
            HashSet<Tuple<PluginDesc, PluginDesc>> edges = new();
            foreach (PluginDesc desc in descs)
            {
                foreach (string dep in desc.Dependencies)
                {
                    PluginDesc depend = descs.First(x => x.Name == dep);
                    edges.Add(new(depend, desc));
                }
            }
            return edges;
        }

        /// <summary>
        /// Topological Sorting (Kahn's algorithm)
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <returns>Sorted node in topological order.</returns>
        private static List<T> TopologicalSort<T>(HashSet<T> nodes, HashSet<Tuple<T, T>> edges) where T : IEquatable<T>
        {
            // Empty list that will contain the sorted elements
            var L = new List<T>();

            // Set of all nodes with no incoming edges
            var S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            // while S is non-empty do
            while (S.Any())
            {
                //  remove a node n from S
                var n = S.First();
                S.Remove(n);

                // add n to tail of L
                L.Add(n);

                // for each node m with an edge e from n to m do
                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    var m = e.Item2;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        // insert m into S
                        S.Add(m);
                    }
                }
            }

            // if graph has edges then
            if (edges.Any())
            {
                // return error (graph has at least one cycle)
                PostLogMessageInternal(LogSeverity.Critical, "Dependency loop detected!").Wait();
                throw new Exception("Cycle detected!");
            }
            else
            {
                // return L (a topologically sorted order)
                return L;
            }
        }
    }
}