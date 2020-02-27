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
                string kirjain = scanner.getNext();
                if (kirjain != "$") {
                    Console.Write(kirjain);
                } else {
                    break;
                }
            }
            Console.WriteLine();
        }
    }

    class Scanner {
        private string filename;
        private char[] characters;
        public Scanner(string filename) {
            this.filename = filename;
            string text = "";
            try {
                text = System.IO.File.ReadAllText(this.filename);
            } catch (System.IO.FileNotFoundException ex) {
                Console.WriteLine(ex);
                return;
            }
            this.characters = text.ToArray();
        }

        public string getNext() {
            char character = '$';
            if (characters.Length > 0) {
                character = this.characters[0];

            }
            while (character == ' ') {
                this.characters = this.characters.Skip(1).ToArray();
                character = this.characters[0];
            }
            string token = "";
            switch (character) {
                case 'e': {
                        token += 'e';
                        if (this.characters[1] == 'n'
                            && this.characters[2] == 'd') {
                            skip(3);
                            return "end";
                        }
                        skip(1);
                        return token;
                    }
                case 'f': {
                        token += 'f';
                        if (this.characters[1] == 'o'
                                && this.characters[2] == 'r') {
                            skip(3);
                            return "for";
                        }
                        skip(1);
                        return token;
                    }
                case 'r': {
                        token += 'r';
                        if (this.characters[1] == 'e'
                            && this.characters[2] == 'a'
                            && this.characters[3] == 'd') {
                            skip(4);
                            return "read";

                        }
                        skip(1);
                        return token;
                    }
                case 'p': {
                        token += 'p';
                        if (this.characters[1] == 'r'
                        && this.characters[2] == 'i'
                        && this.characters[3] == 'n'
                        && this.characters[4] == 't') {
                            token += "rint";
                            skip(4);
                        }
                        skip(1);
                        return token;
                    }
                case (char)34: {
                        skip(1);
                        while (this.characters.Length > 0) {
                            char chara = this.characters[0];
                            char nextChara = this.characters[1];
                            if (chara == (char)34) {
                                skip(1);
                                return token;
                            } else {
                                token += chara;
                                skip(1);
                            }
                        }
                        return "";
                    }
                case '$': {
                        skip(1);
                        return "" + character;
                    }
                case 'v': {
                        if (this.characters[1] == 'a' && this.characters[2] == 'r') {
                            skip(3);
                            return "var";
                        } else {
                            skip(1);
                            return "" + character;
                        }
                    }
                case ';': {
                        skip(1);
                        return "" + character;
                    }
                default: {
                        skip(1);
                        return "" + character;
                    }
            }
        }
        private void skip(int amount) {
            this.characters = this.characters.Skip(amount).ToArray();
        }
    }
}
