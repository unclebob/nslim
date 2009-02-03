using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    public abstract class Converter<T>: ParseOperator<string>, ComposeOperator<string> {
        public bool IsMatch(Processor<string> processor, State<string> state) {
            return state.Type == typeof(T);
        }

        public Tree<string> Compose(Processor<string> processor, State<string> state) {
            return new TreeLeaf<string>(Compose((T)state.Instance));
        }

        public object Parse(Processor<string> processor, State <string>state) {
            return Parse(state.ParameterValue);
        }

        protected abstract T Parse(string input);
        protected abstract string Compose(T input);
    }
}
