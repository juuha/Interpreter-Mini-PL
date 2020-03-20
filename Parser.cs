using System;

// Mini-PL grammar
/*
<prog> ::= <stmts>
<stmts> ::= <stmt> ";" ( <stmt> ";" )*
<stmt> ::= "var" <var_ident> ":" <type> [ ":=" <expr ]
    | <var_ident> ":=" <expr>
    | "for" <var_ident> "in" <expr> ".." <expr> "do" <stmts> "end" "for"
    | "read" <var_ident>
    | "print" <expr>
    | "assert" "(" <expr> ")"
<expr> ::= <opnd> <op> <opnd>
    | [ <unary_op> ] <opnd>
<opnd> ::= <int>
    | <string>
    | <var_ident>
    | "(" expr ")"
<type> ::= "int" | "string" | "bool"
<var_ident> ::= <ident>


<reserved keyword> ::=
"var" | "for" | "end" | "in" | "do" | "read" |
"print" | "int" | "string" | "bool" | "assert"

<op> ::=
"+" | "-" | "*" | "/" | "<" | "=" | "&"

<unary_op> ::= "!"
*/

namespace interpreter {
    
    class Parser {
        private Scanner scanner;
        private string[] symbol; // symbol[0]: type, symbol[1]: lexeme 
        private string[] operators = {"+", "-", "*", "/", "<", "=", "&"};
        private string unary_op = "!";

        public Parser(Scanner scanner) {
            this.scanner = scanner;
            this.symbol = this.scanner.getNext();
        }

        public void parse() {
            program();
            /*while (true) {
                Console.ReadLine();
                string[] token = this.scanner.getNext();
                if (token[1] != "$$") {
                    Console.Write(token[1] + " --:-- " + token[0]);
                } else {
                    break;
                }
            }*/
        }

        private void program() {
            statements();
            match("$$");
        }

        private void statements() {
            statement();
            match(";");
            while (symbol[0] != "$$" && symbol[0] != "end") {
                statement();
                match(";");
            }
        }

        private void statement() {
            switch (symbol[0]) {
                case "var":
                    match("var");
                    match_id();
                    match(":");
                    type();
                    if (symbol[0] == ":=") {
                        match(":=");
                        expression();
                    }
                    break;
                case "for":
                    match("for");
                    match_id();
                    match("in");
                    expression();
                    match("..");
                    expression();
                    match("do");
                    statements();
                    match("end");
                    match("for");
                    break;
                case "read": 
                    match("read");
                    match_id();
                    break;
                case "print":
                    match("print");
                    expression();
                    break;
                case "assert":
                    match("assert");
                    match("(");
                    expression();
                    match(")");
                    break;
                case "ident":
                    match_id();
                    match(":=");
                    expression();
                    break;
                default: throw new Exception($"A statement can't start with {symbol[1]}.");
            }
        }

        private void expression() {
            switch (symbol[0]) {
                case "unary_op":
                    match("!");
                    operand();
                    break;
                case "int":
                case "string":
                case "ident": 
                case "(": operand(); break;
                default: throw new Exception($"Unexpected symbol {symbol[1]}, expected an operand, an opening paranthesis or an unary operator.");
            }
            if (symbol[0] == "op") {
                switch (symbol[1]) {
                    case "+": 
                        match("+");
                        break;
                    case "-": 
                        match("-");
                        break;
                    case "*": 
                        match("*");
                        break;
                    case "/": 
                        match("/");
                        break;
                    case "<": 
                        match("<");
                        break;
                    case "=": 
                        match("=");
                        break;
                    case "&": 
                        match("&");
                        break;
                    default: throw new Exception($"Unexpected symbol {symbol[1]}, expected an operator.");
                }
                operand();
            }
        }

        private void operand() {
            switch (symbol[0]) {
                case "int":
                case "string":
                    match_literal();
                    break;
                case "ident": 
                    match_id();
                    break;
                case "(": 
                    match("(");
                    expression();
                    match(")");
                    break;
                default: throw new Exception($"Unexpected symbol {symbol[1]}, expected an operand or an opening paranthesis.");
            }
        }

        private void type() {
            if (symbol[0] == "type") {
                switch (symbol[1]) {
                    case "int": 
                        match("int");
                        break;
                    case "string": 
                        match("string");
                        break;
                    case "bool": 
                        match("bool");
                        break;
                    default: throw new Exception($"Unexpected symbol {symbol[1]}, expected a type.");
                }
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected a type.");
            }
            
        }

        private void match(String expected) {
            if (symbol[1] == expected) {
                Console.WriteLine($"Matched symbol {symbol[1]}");
                symbol = scanner.getNext();
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected {expected}.");
            }
        }

        private void match_id() {
            if (symbol[0] == "ident") {
                Console.WriteLine($"Matched identifier {symbol[1]}");
                symbol = scanner.getNext();
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected an identifier.");
            }
        }

        private void match_literal() {
            if (symbol[0] == "int" || symbol[0] == "string") {
                Console.WriteLine($"Matched literal {symbol[1]}");
                symbol = scanner.getNext();
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected a literal.");
            }
        }
    }

}