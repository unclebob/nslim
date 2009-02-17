using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    public abstract class Converter<T>: ParseOperator<string>, ComposeOperator<string> {
        public bool TryParse(Processor<string> processor, Type type, TypedValue instance, Tree<string> parameters, ref object result) {
            if (!IsMatch(type)) return false;
            result = Parse(parameters.Value);
            return true;
        }

        public bool TryCompose(Processor<string> processor, TypedValue instance, ref Tree<string> result) {
            if (!IsMatch(instance.Type)) return false;
            result = new TreeLeaf<string>(Compose((T)instance.Value));
            return true;
        }

        private static bool IsMatch(Type type) {
            return type == typeof(T);
        }

        protected abstract T Parse(string input);
        protected abstract string Compose(T input);
    }
}
