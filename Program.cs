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

    class Scanner {
        private string filename;
        private String characters;
        public Scanner(string filename) {
            this.filename = filename;
            string text = "";
            try {
                text = System.IO.File.ReadAllText(this.filename);
            } catch (System.IO.FileNotFoundException ex) {
                Console.WriteLine(ex);
                return;
            }
            this.characters = text;
        }

        public string[] getNext() {
            char character = '$';
            if (this.characters.Length > 0) {
                character = this.characters[0];

            }
            while (Char.IsWhiteSpace(character)) {
                skip(1);
                character = this.characters[0];
            }
            string lexeme = "";
            switch (character) {
                // reserved keywords
                case 'a': // assert
                {
                    if (this.characters.StartsWith("assert")){
                        if (this.characters.Length > 6) {
                            if (char.IsLetterOrDigit(characters[6])) {
                                goto default;
                            }
                        }
                        String[] token = {"assert", "assert"};
                        skip(6);
                        return token;
                    }
                    goto default;
                }
                case 'b': // bool
                {
                    if (this.characters.StartsWith("bool")) {
                        if (this.characters.Length > 4) {
                            if (Char.IsLetterOrDigit(this.characters[4])) {
                                goto default;
                            }
                        }
                        String[] token = {"bool", "bool"};
                        skip(4);
                        return token;
                    }
                    goto default;
                }
                case 'd': // do
                {
                    if (this.characters.StartsWith("do")) {
                        if (this.characters.Length > 2) {
                            if (Char.IsLetterOrDigit(this.characters[2])) {
                                goto default;
                            }
                        }
                        String[] token = {"do", "do"};
                        skip(2);
                        return token;
                    }
                    goto default;
                }
                case 'e': // end
                {
                    if (this.characters.StartsWith("end")) {
                        if (this.characters.Length > 3) {
                            if (Char.IsLetterOrDigit(this.characters[3])) {
                                goto default;
                            }
                        }
                        String[] token = {"end", "end"};
                        skip(3);
                        return token;
                    }
                    goto default;
                }
                case 'f': // for
                {
                    if (this.characters.StartsWith("for")) {
                        if (this.characters.Length > 3) {
                            if (Char.IsLetterOrDigit(this.characters[3])) {
                                goto default;
                            }
                        }
                        String[] token = {"for", "for"};
                        skip(3);
                        return token;
                    }
                    goto default;
                }
                case 'i': // in, int
                {
                    if (this.characters.StartsWith("int")) {
                        if (this.characters.Length > 3) {
                            if (Char.IsLetterOrDigit(this.characters[3])) {
                                goto default;
                            }
                        }
                        String[] token = {"int", "int"};
                        skip(3);
                        return token;
                    }
                    if (this.characters.StartsWith("in")) {
                        if (this.characters.Length > 2) {
                            if (Char.IsLetterOrDigit(this.characters[2])) {
                                goto default;
                            }
                        }
                        String[] token = {"in", "in"};
                        skip(2);
                        return token;
                    }
                    goto default;
                }
                case 'p': // print
                {
                    if (this.characters.StartsWith("print")) {
                        if (this.characters.Length > 5) {
                            if (Char.IsLetterOrDigit(this.characters[5])) {
                                goto default;
                            }
                        }
                        String[] token = {"print", "print"};
                        skip(5);
                        return token;
                    }
                    goto default;
                }
                case 'r': // read
                {
                    if (this.characters.StartsWith("read")) {
                        if (this.characters.Length > 4) {
                            if (Char.IsLetterOrDigit(this.characters[4])) {
                                goto default;
                            }
                        }
                        String[] token = {"read", "read"};
                        skip(4);
                        return token;
                    }
                    goto default;
                }
                case 's': // string
                {
                    if (this.characters.StartsWith("string")) {
                        if (this.characters.Length > 5) {
                            if (Char.IsLetterOrDigit(this.characters[5])) {
                                goto default;
                            }
                        }
                        String[] token = {"string", "string"};
                        skip(5);
                        return token;
                    }
                    goto default;
                }
                case 'v': // var
                {
                    if (this.characters.StartsWith("var")) {
                        if (this.characters.Length > 3) {
                            if (Char.IsLetterOrDigit(this.characters[3])) {
                                goto default;
                            }
                        }
                        String[] token = {"var", "var"};
                        skip(3);
                        return token;
                    }
                    goto default;
                }
                // weird marks
                case ';': // ;
                {
                    String[] token = {";", ";"};
                    skip(1);
                    return token;
                }
                case ':': // :, :=
                {
                    lexeme += ":";
                    skip(1);
                    if (this.characters.Length > 0) {
                        if (this.characters[0] == '=') {
                            lexeme += "=";
                            String[] tokenEqual = {":=", lexeme};
                            skip(1);
                            return tokenEqual; 
                        }
                    }
                    String[] token = {":", ":"};
                    return token; 
                }
                case '(': // (
                {
                    String[] token = {"(", "("};
                    skip(1);
                    return token;
                }
                case ')': // )
                {
                    String[] token = {")", ")"};
                    skip(1);
                    return token;
                }
                case (char)34: // "
                {
                    lexeme += character;
                    skip(1);
                    if (this.characters.Length > 0) {
                        while (true) {
                            if (this.characters[0] == (char)92) {
                                lexeme += this.characters[0];
                                lexeme += this.characters[1];
                                skip(2);
                            } else {
                                if (this.characters[0] == (char)34) {
                                    lexeme += (char)34;
                                    skip(1);
                                    break;
                                } else {
                                    lexeme += this.characters[0];
                                    skip(1);
                                }
                            }
                        }   
                    }
                    String[] token = {"string", lexeme};
                    return token;
                }
                case '.': // ..
                {
                    if (this.characters.Length > 1) {
                        if (this.characters[1] == '.') {
                            String[] token = {"..", ".."};
                            skip(2);
                            return token;
                        }
                    }
                    goto default;
                }
                // operators
                case '+': // +
                {
                    String[] token = {"+", "+"};
                    skip(1);
                    return token;
                }
                case '-': // substraction
                {
                    String[] token = {"-", "-"};
                    skip(1);
                    return token;
                }
                case '*': // *
                {
                    String[] token = {"*", "*"};
                    skip(1);
                    return token;
                }
                case '/': // /
                {
                    String[] token = {"/", "/"};
                    skip(1);
                    return token;
                }
                case '<': // /
                {
                    String[] token = {"<", "<"};
                    skip(1);
                    return token;
                }
                case '=': // =
                {
                    String[] token = {"=", "="};
                    skip(1);
                    return token;
                }
                case '&': // &
                {
                    String[] token = {"&", "&"};
                    skip(1);
                    return token;
                }
                case '!': // !
                {
                    String[] token = {"!", "!"};
                    skip(1);
                    return token;
                }
                // integers
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '0': { // integer
                    while (Char.IsDigit(character)) {
                        lexeme += character;
                        skip(1);
                        if (this.characters.Length == 0) break;
                        character = this.characters[0];
                    }
                    String[] token = {"int", lexeme};
                    return token;
                }
                
                default: {
                    while (Char.IsLetterOrDigit(character)) {
                        lexeme += character;
                        skip(1);
                        if (this.characters.Length == 0) break;
                        character = this.characters[0];
                    }
                    if (lexeme.Length == 0) {
                        lexeme = "$";
                    }
                    //Console.WriteLine(lexeme);
                    String[] token = {"identifier", lexeme};
                    return token;
                }
            }
        }
        private void skip(int amount) {
            if (this.characters.Length <= amount) {
                this.characters = "";
            } else {
            this.characters = this.characters.Substring(amount);
            }
        }
    }
}
