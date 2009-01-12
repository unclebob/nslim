// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.application;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public class Namespaces {
        private readonly List<LanguageName> namespaces = new List<LanguageName>();

        public static Namespaces Instance { get { return Context.Instance.GetItem<Namespaces>(); }}

        public Namespaces() {
            Add(GetType().Namespace);
        }

        public void Add(string namespaceString) {
            var newNamespace = new LanguageName(namespaceString);
            if (!namespaces.Contains(newNamespace)) namespaces.Add(newNamespace);
        }

        public bool IsRegistered(string namespaceString) {
            var existingNamespace = new LanguageName(namespaceString);
            return namespaces.Contains(existingNamespace);
        }
    }
}