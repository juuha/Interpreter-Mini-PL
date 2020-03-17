using System;
using System.Linq;

namespace compiler {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No filename given.");
                return;
            }
            string filename = args[0];
            Scanner scanner = new Scanner(filename);
            while (true) {
                Console.ReadLine();
                string[] token = scanner.getNext();
                if (token[1] != "$") {
                    Console.Write(token[1] + " --:-- " + token[0]);
                } else {
                    break;
                }
            }
            Console.WriteLine();
        }
    }
}
