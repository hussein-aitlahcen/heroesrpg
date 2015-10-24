using HeroesRpg.Common.Generic;
using HeroesRpg.Common.Manager;
using HeroesRpg.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Manager
{
    /// <summary>
    /// Manager supervisor
    /// </summary>
    public sealed class SuperManager : Singleton<SuperManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private sealed class ManagerDefinition
        {
            /// <summary>
            /// 
            /// </summary>
            public Type Type
            {
                get;
                private set;
            }

            /// <summary>
            /// 
            /// </summary>
            public ManagerDefinitionAttribute Definition
            {
                get;
                private set;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="type"></param>
            /// <param name="definition"></param>
            public ManagerDefinition(Type type, ManagerDefinitionAttribute definition)
            {
                Type = type;
                Definition = definition;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ManagerDefinition> m_definitions;

        /// <summary>
        /// 
        /// </summary>
        public SuperManager()
        {
            m_definitions = new List<ManagerDefinition>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitializeManagers(string nameSpace)
        {
            foreach (var type in ReflectHelper.FetchTypesInNamespace(nameSpace))
            {
                var attr = ReflectHelper.FetchTypeAttribute<ManagerDefinitionAttribute>(type);
                if (attr != null)                
                    m_definitions.Add(new ManagerDefinition(type, attr));                
            }
            if(m_definitions.Count > 0)
                InitializeManager(m_definitions[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private void InitializeManager(ManagerDefinition manager)
        {
            foreach(var dependency in manager.Definition.Dependencies)
            {
                var dependencyDefinition = m_definitions.FirstOrDefault(m => m.Definition.Id == dependency);
                if (dependencyDefinition != null)
                    InitializeManager(dependencyDefinition);                
            }

            var instance = ReflectHelper.FetchPropertyValue<IManager>
                (
                    manager.Type,
                    "Instance",
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.FlattenHierarchy
                );
            instance.Initialize();

            // Since the manager is loaded we dont need to load it back
            m_definitions.Remove(manager);

            if (m_definitions.Count > 0)
                InitializeManager(m_definitions[0]);
        }
    }
}
