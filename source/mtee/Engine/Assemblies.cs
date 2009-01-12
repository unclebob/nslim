// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Reflection;
using fitnesse.mtee.application;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public class Assemblies {
        private readonly List<Assembly> assemblies = new List<Assembly>();
        private readonly Namespaces namespaces;

        public static Assemblies Instance { get { return Context.Instance.GetItem<Assemblies>(); } }

        public Assemblies(Namespaces namespaces) {
            this.namespaces = namespaces;
        }

        public Assemblies(): this(Namespaces.Instance) {}

        public void Add(string assemblyName) {
            Assembly assembly = Assembly.LoadFrom(assemblyName);
            if (assemblies.Contains(assembly)) return;
            assemblies.Add(assembly);
        }

        public RuntimeType FindType(string typeName) {
            var languageName = new LanguageName(typeName);
            return new RuntimeType(Type.GetType(languageName.MatchName) ?? SearchForType(languageName));
        }

        private Type SearchForType(NameMatcher typeName) {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (Type type in assembly.GetExportedTypes()) {
                    if (typeName.Matches(type.FullName)) return type;
                    if (!namespaces.IsRegistered(type.Namespace)) continue;
                    if (typeName.Matches(type.Name)) return type;
                }
            }
            throw new ArgumentException(string.Format("Type '{0}' not found", typeName.MatchName));
        }
    }
}