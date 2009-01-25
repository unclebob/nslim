// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Reflection;
using fitnesse.mtee.model;
using fitnesse.mtee.Model;

namespace fitnesse.mtee.engine {
    public class SystemUnderTest: Copyable {
        private readonly List<Assembly> assemblies;
        private readonly List<LanguageName> namespaces;

        public SystemUnderTest() {
            assemblies = new List<Assembly>();
            namespaces = new List<LanguageName>();
            AddNamespace(GetType().Namespace);
        }

        public SystemUnderTest(SystemUnderTest other) {
            assemblies = new List<Assembly>(other.assemblies);
            namespaces = new List<LanguageName>(other.namespaces);
        }

        public void AddAssembly(string assemblyName) {
            Assembly assembly = Assembly.LoadFrom(assemblyName);
            if (assemblies.Contains(assembly)) return;
            assemblies.Add(assembly);
        }

        public void AddNamespace(string namespaceName) {
            var newNamespace = new LanguageName(namespaceName);
            if (!namespaces.Contains(newNamespace)) namespaces.Add(newNamespace);
        }

        public RuntimeType FindType(string typeName) {
            var languageName = new LanguageName(typeName);
            return new RuntimeType(Type.GetType(languageName.MatchName) ?? SearchForType(languageName));
        }

        private Type SearchForType(NameMatcher typeName) {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (Type type in assembly.GetExportedTypes()) {
                    if (typeName.Matches(type.FullName)) return type;
                    if (type.Namespace == null || !IsRegistered(type.Namespace)) continue;
                    if (typeName.Matches(type.Name)) return type;
                }
            }
            throw new ArgumentException(string.Format("Type '{0}' not found", typeName.MatchName));
        }

        private bool IsRegistered(string namespaceString) {
            var existingNamespace = new LanguageName(namespaceString);
            return namespaces.Contains(existingNamespace);
        }

        public Copyable Copy() {
            return new SystemUnderTest(this);
        }
    }
}