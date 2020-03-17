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
            parser.parse();
            Console.WriteLine();
        }
    }
}
