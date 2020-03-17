using System;

namespace compiler {
    class Scanner {
        private string filename;
        private String characters;
        private int line = 1;
        private int column = 1;
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
                if (character == (char)10 || character == (char)13) {
                    this.column = 0;
                    this.line++;
                }
                skip(1);
                character = this.characters[0];
            }
            string lexeme = "";
            if (Char.IsLetterOrDigit(character)) {
                // digits and letters
                switch (character) {
                    // reserved keywords
                    case 'a': { // assert
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
                    case 'b': { // bool
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
                    case 'd': { // do
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
                    case 'e': { // end
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
                    case 'f': { // for
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
                    case 'i': { // in, int
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
                    case 'p': { // print
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
                    case 'r': { // read
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
                    case 's': { // string
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
                    case 'v': { // var
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
                            if (this.characters.Length == 0) {
                                break;
                            }
                            character = this.characters[0];
                        }
                        String[] token = {"identifier", lexeme};
                        return token;
                }
                }
            } else {
                // all other characters
                switch (character) {
                    // special symbols
                    case ';': { // end of statement ;
                    String[] token = {";", ";"};
                    skip(1);
                    return token;
                }
                    case ':': { // declartion : and assignment :=
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
                    case '(': { // left parenthesis ()
                    String[] token = {"(", "("};
                    skip(1);
                    return token;
                }
                    case ')': { // right parenthesis )
                    String[] token = {")", ")"};
                    skip(1);
                    return token;
                }
                    case (char)34: { // string "string"
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
                    case '.': { // range operator ..
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
                    case '+': { // addition +
                    String[] token = {"+", "+"};
                    skip(1);
                    return token;
                }
                    case '-': { // substraction -
                    String[] token = {"-", "-"};
                    skip(1);
                    return token;
                }
                    case '*': { // multiplication *
                    String[] token = {"*", "*"};
                    skip(1);
                    return token;
                }
                    case '/': { // division /
                    String[] token = {"/", "/"};
                    skip(1);
                    return token;
                }
                    case '<': { // less than comparator <
                    String[] token = {"<", "<"};
                    skip(1);
                    return token;
                }
                    case '=': { // equals operator =
                    String[] token = {"=", "="};
                    skip(1);
                    return token;
                }
                    case '&': { // and operator &
                    String[] token = {"&", "&"};
                    skip(1);
                    return token;
                }
                    case '!': { // not operator !
                    String[] token = {"!", "!"};
                    skip(1);
                    return token;
                }
                    case '$': { // end of file
                        lexeme = "$";
                        String[] token = {"identifier", lexeme};
                        return token;
                    }
                    default: { // any other symbol
                        //throw new Exception($"Unexpected symbol on line: {this.line} on column: {this.column}.");
                        Console.WriteLine($"Unexpected symbol on line: {this.line} on column: {this.column}. Skipping to next symbol.");
                        skip(1);
                        return this.getNext();
                    }
                }
            }
        }
        private void skip(int amount) {
            this.column += amount;
            if (this.characters.Length <= amount) {
                this.characters = "";
            } else {
            this.characters = this.characters.Substring(amount);
            }
        }
    }
}