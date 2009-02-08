using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    public abstract class Converter<T>: ParseOperator<string>, ComposeOperator<string> {
        public bool IsMatch(Command<string> command) {
            return command.Type == typeof(T);
        }

        public Tree<string> Compose(Command<string> command) {
            return new TreeLeaf<string>(Compose((T)command.Instance));
        }

        public object Parse(Command <string>command) {
            return Parse(command.ParameterValue);
        }

        protected abstract T Parse(string input);
        protected abstract string Compose(T input);
    }
}
