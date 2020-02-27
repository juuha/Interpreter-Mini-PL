using System;
using System.Linq;

namespace compiler {
    class Program {
        static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No filename given.");
                return;
            }
            String filename = args[0];
            Scanner scanner = new Scanner(filename);
            while (true) {
                String more = Console.ReadLine();
                String kirjain = scanner.getNext();
                if (kirjain != "$$") {
                    Console.Write(kirjain);
                } else {
                    break;
                }
            }
            Console.WriteLine();
        }
    }

    class Scanner {
        private String filename;
        private char[] characters;
        public Scanner(String filename) {
            this.filename = filename;
            String text = "";
            try {
                text = System.IO.File.ReadAllText(this.filename);
            } catch (System.IO.FileNotFoundException ex) {
                Console.WriteLine(ex);
                return;
            }
            this.characters = text.ToArray();
        }

        public String getNext() {
            char character = '$';
            if (characters.Length > 0) {
                character = this.characters[0];

            }
            switch (character) {
                case '$': {
                        return "$$";
                    }
                default: {
                        this.characters = this.characters.Skip(1).ToArray();
                        return "" + character;
                    }
            }
        }
    }
}
