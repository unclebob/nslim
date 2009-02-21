// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.application;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim {
    public class Runner: Runnable {
        private Messenger messenger;
        private Service service;

        public int Run(string[] commandLineArguments, Configuration configuration) {
            service = configuration.GetItem<Service>();
            service.ApplicationUnderTest = configuration.GetItem<ApplicationUnderTest>();
            ParseCommandLine(commandLineArguments);
            ProcessInstructions();
            return 0;
        }

        private void ParseCommandLine(string[] commandLineArguments) {
            messenger = Messenger.Make(int.Parse(commandLineArguments[commandLineArguments.Length - 1]));
        }

        private void ProcessInstructions() {
            while (true) {
                string instruction = messenger.Read();
                if (messenger.IsEnd) break;
                Document results = Execute(Document.Parse(instruction));
                messenger.Write(results.ToString());
            }
        }

        private Document Execute(Document document) {
            var results = new TreeList<string>();
            foreach (Tree<string> statement in document.Content.Branches) {
                results.AddBranchValue(service.Execute(statement).Value);
            }
            return new Document(results);
        }
    }
}
