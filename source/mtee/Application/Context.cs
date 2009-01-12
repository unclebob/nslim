// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;

namespace fitnesse.mtee.application {
    public class Context {
        public static Context Instance { get; private set;}

        private readonly Dictionary<string, object> items = new Dictionary<string, object>();

        static Context() {
            Instance = new Context();
        }

        public T GetItem<T>() where T: new() {
            string typeName = typeof (T).FullName;
            if (!items.ContainsKey(typeName)) {
                items[typeName] = new T();
            }
            return (T)items[typeName];
        }

        public object GetItem(string typeName) {
            if (!items.ContainsKey(typeName)) {
                items[typeName] = new Processor().Create(typeName);
            }
            return items[typeName];
        }
    }
}