using System;

namespace interpreter {
    
    class Parser {
        private Scanner scanner;

        public Parser(Scanner scanner) {
            this.scanner = scanner;
        }

        public void parse() {
            while (true) {
                Console.ReadLine();
                string[] token = this.scanner.getNext();
                if (token[1] != "$") {
                    Console.Write(token[1] + " --:-- " + token[0]);
                } else {
                    break;
                }
            }
        }
    }

}