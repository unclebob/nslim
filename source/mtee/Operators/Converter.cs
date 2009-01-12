using fitnesse.mtee.engine;

namespace fitnesse.mtee.Operators {
    public abstract class Converter<T>: ParseOperator, ComposeOperator {
        public bool IsMatch(Processor processor, State state) {
            return state.Type == typeof(T);
        }

        public object Compose(Processor processor, State state) {
            return Compose((T)state.Instance);
        }

        public object Parse(Processor processor, State state) {
            return Parse(state.ParameterValueString);
        }

        protected abstract T Parse(string input);
        protected abstract string Compose(T input);
    }
}
