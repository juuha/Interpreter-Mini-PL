using System;

namespace interpreter {
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
            string type = "$$";
            if (Char.IsLetterOrDigit(character)) {
                // digits and letters
                switch (character) {
                    // reserved keywords
                    case 'a': // assert
                        if (this.characters.StartsWith("assert")){
                            if (this.characters.Length > 6) {
                                if (char.IsLetterOrDigit(characters[6])) {
                                    goto default;
                                }
                            }
                            lexeme = "assert";
                            type = "assert";
                            skip(6);
                            break;
                        }
                        goto default;
                    case 'b': // bool
                        if (this.characters.StartsWith("bool")) {
                            if (this.characters.Length > 4) {
                                if (Char.IsLetterOrDigit(this.characters[4])) {
                                    goto default;
                                }
                            }
                            lexeme = "bool";
                            type = "type";
                            skip(4);
                            break;
                        }
                        goto default;
                    case 'd': // do
                        if (this.characters.StartsWith("do")) {
                            if (this.characters.Length > 2) {
                                if (Char.IsLetterOrDigit(this.characters[2])) {
                                    goto default;
                                }
                            }
                            lexeme = "do";
                            type = "do";
                            skip(2);
                            break;
                        }
                        goto default;
                    case 'e': // end
                        if (this.characters.StartsWith("end")) {
                            if (this.characters.Length > 3) {
                                if (Char.IsLetterOrDigit(this.characters[3])) {
                                    goto default;
                                }
                            }
                            lexeme = "end";
                            type = "end";
                            skip(3);
                            break;
                        }
                        goto default;
                    case 'f': // for
                        if (this.characters.StartsWith("for")) {
                            if (this.characters.Length > 3) {
                                if (Char.IsLetterOrDigit(this.characters[3])) {
                                    goto default;
                                }
                            }
                            lexeme = "for";
                            type = "for";
                            skip(3);
                            break;
                        }
                        goto default;
                    case 'i': // in, int
                        if (this.characters.StartsWith("int")) {
                            if (this.characters.Length > 3) {
                                if (Char.IsLetterOrDigit(this.characters[3])) {
                                    goto default;
                                }
                            }
                            lexeme = "int";
                            type = "type";
                            skip(3);
                            break;
                        }
                        if (this.characters.StartsWith("in")) {
                            if (this.characters.Length > 2) {
                                if (Char.IsLetterOrDigit(this.characters[2])) {
                                    goto default;
                                }
                            }
                            lexeme = "in";
                            type = "in";
                            skip(2);
                            break;
                        }
                        goto default;
                    case 'p': // print
                        if (this.characters.StartsWith("print")) {
                            if (this.characters.Length > 5) {
                                if (Char.IsLetterOrDigit(this.characters[5])) {
                                    goto default;
                                }
                            }
                            lexeme = "print";
                            type = "print";
                            skip(5);
                            break;
                        }
                        goto default;
                    case 'r': // read
                        if (this.characters.StartsWith("read")) {
                            if (this.characters.Length > 4) {
                                if (Char.IsLetterOrDigit(this.characters[4])) {
                                    goto default;
                                }
                            }
                            lexeme = "read";
                            type = "read";
                            skip(4);
                            break;
                        }
                        goto default;
                    case 's': // string
                        if (this.characters.StartsWith("string")) {
                            if (this.characters.Length > 5) {
                                if (Char.IsLetterOrDigit(this.characters[5])) {
                                    goto default;
                                }
                            }
                            lexeme = "string";
                            type = "type";
                            skip(5);
                            break;
                        }
                        goto default;
                    case 'v': // var
                        if (this.characters.StartsWith("var")) {
                            if (this.characters.Length > 3) {
                                if (Char.IsLetterOrDigit(this.characters[3])) {
                                    goto default;
                                }
                            }
                            lexeme = "var";
                            type = "var";
                            skip(3);
                            break;
                        }
                        goto default;
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
                    case '0': // integer
                        while (Char.IsDigit(character)) {
                            lexeme += character;
                            skip(1);
                            if (this.characters.Length == 0) break;
                            character = this.characters[0];
                        }
                        type = "int";
                        break;
                    default:
                        while (Char.IsLetterOrDigit(character)) {
                            lexeme += character;
                            skip(1);
                            if (this.characters.Length == 0) {
                                break;
                            }
                            character = this.characters[0];
                        }
                        type = "ident";
                        break;
                }
            } else {
                // all other characters
                switch (character) {
                    // special symbols
                    case ';': // end of statement ;
                        lexeme = ";";
                        type = ";";
                        skip(1);
                        break;
                    case ':': // declartion : and assignment :=
                        lexeme += ":";
                        skip(1);
                        if (this.characters.Length > 0) {
                            if (this.characters[0] == '=') {
                                lexeme += "=";
                                type = ":=";
                                skip(1);
                                break;
                            }
                        }
                        type = ":";
                        break;
                    case '(': // left parenthesis ()
                        lexeme = "(";
                        type = "(";
                        skip(1);
                        break;
                    case ')': // right parenthesis )
                        lexeme = ")";
                        type = ")";
                        skip(1);
                        break;
                    case (char)34: // string "string"
                        lexeme += "";
                        type = "string";
                        skip(1);
                        if (this.characters.Length > 0) {
                            while (true) {
                                if (this.characters[0] == (char)92) {
                                    lexeme += this.characters[0];
                                    lexeme += this.characters[1];
                                    skip(2);
                                } else {
                                    if (this.characters[0] == (char)34) {
                                        skip(1);
                                        break;
                                    } else {
                                        lexeme += this.characters[0];
                                        skip(1);
                                    }
                                }
                            }   
                        }
                        break;
                    case '.': // range operator ..
                        if (this.characters.Length > 1) {
                            if (this.characters[1] == '.') {
                                lexeme = "..";
                                type = "..";
                                skip(2);
                                break;
                            }
                        }
                        goto default;

                    // operators
                    case '+': // addition +
                        lexeme = "+";
                        type = "op";
                        skip(1);
                        break;
                    case '-': // substraction -
                        lexeme = "-";
                        type = "op";
                        skip(1);
                        break;
                    case '*': // multiplication *
                        lexeme = "*";
                        type = "op";
                        skip(1);
                        break;
                    case '/': // division /, start of comment */, line comment //
                        lexeme += "/";
                        skip(1);
                        if (this.characters.Length > 0) {
                            if (this.characters[0] == '/') {
                                while (true) {
                                    if (this.characters[0] == (char)10 || this.characters[0] == (char)13) {
                                        skip(1);
                                        return this.getNext();
                                    }
                                    skip(1);
                                }
                            }
                            if (this.characters[0] == '*') {
                                while (true) {
                                    if (this.characters[0] == '*' && this.characters[1] == '/') {
                                        skip(2);
                                        return this.getNext();
                                    }
                                    skip(1);
                                }
                            }
                        }
                        type = "op";
                        break;
                    case '<': // less than comparator <
                        lexeme = "<";
                        type = "op";
                        skip(1);
                        break;
                    case '=': // equals operator =
                        lexeme = "=";
                        type = "op";
                        skip(1);
                        break;
                    case '&': // and operator &
                        lexeme = "&";
                        type = "op";
                        skip(1);
                        break;
                    case '!': // not operator !
                        lexeme = "!";
                        type = "unary_op";
                        skip(1);
                        break;
                    case '$':// end of file
                        lexeme = "$$";
                        type = "$$";
                        break;
                    default: // any other symbol
                        //throw new Exception($"Unexpected symbol on line: {this.line} on column: {this.column}.");
                        Console.WriteLine($"Unexpected symbol on line: {this.line} on column: {this.column}. Skipping to next symbol.");
                        skip(1);
                        return this.getNext();
                }
            }
            String[] token = {type, lexeme};
            return token;
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