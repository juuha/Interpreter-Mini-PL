using System;

namespace interpreter {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No filename given.");
                return;
            }
            string filename = args[0];
            Scanner scanner = new Scanner(filename);
            Parser parser = new Parser(scanner);
            Node tree = parser.parse();
            Interpreter interpreter = new Interpreter(tree);
            interpreter.interpret();
        }
    }
}
