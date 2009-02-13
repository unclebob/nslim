// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;

namespace fitnesse.mtee.application {
    public interface Runnable {
        int Run(string[] commandLineArguments, Configuration configuration);
    }

    public class Shell {
        private string runnerName;
        private readonly List<string> extraArguments = new List<string>();
        private readonly Configuration configuration = new Configuration();

        public int Run(string[] commandLineArguments) {
            ParseArguments(commandLineArguments);
            return ExecuteRunner();
        }

        private void ParseArguments(string[] commandLineArguments) {
            for (int i = 0; i < commandLineArguments.Length; i++) {
                if (i < commandLineArguments.Length - 1) {
                    switch (commandLineArguments[i]) {
                        case "-c":
                            configuration.LoadFile(commandLineArguments[i + 1]);
                            break;
                        case "-r":
                            runnerName = commandLineArguments[i + 1];
                            break;
                        default:
                            extraArguments.Add(commandLineArguments[i]);
                            continue;
                    }
                    i++;
                }
                else extraArguments.Add(commandLineArguments[i]);
            }
        }

        private int ExecuteRunner() {
            var runnable = (Runnable) new BasicProcessor().Create(runnerName);
            return runnable.Run(extraArguments.ToArray(), configuration);
        }
    }
}
