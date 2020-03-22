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

        public Parser(Scanner scanner) {
            this.scanner = scanner;
            this.symbol = this.scanner.getNext();
        }

        public Node parse() {
            return program();
        }

        private Node program() {
            Node program = statements();
            match("$$");
            return program;
        }

        private Node statements() {
            Node statements = new Node("statements", "");
            statements.addNode(statement());
            match(";");
            while (symbol[0] != "$$" && symbol[0] != "end") {
                statements.addNode(statement());
                match(";");
            }
            return statements;
        }

        private Node statement() {
            Node statement = new Node("statement", "");
            switch (symbol[0]) {
                case "var":
                    statement.addNode(match("var"));
                    statement.addNode(match_id());
                    statement.addNode(match(":"));
                    statement.addNode(type());
                    if (symbol[0] == ":=") {
                        statement.addNode(match(":="));
                        statement.addNode(expression());
                    }
                    break;
                case "for":
                    statement.addNode(match("for"));
                    statement.addNode(match_id());
                    statement.addNode(match("in"));
                    statement.addNode(expression());
                    statement.addNode(match(".."));
                    statement.addNode(expression());
                    statement.addNode(match("do"));
                    statement.addNode(statements());
                    statement.addNode(match("end"));
                    statement.addNode(match("for"));
                    break;
                case "read": 
                    statement.addNode(match("read"));
                    statement.addNode(match_id());
                    break;
                case "print":
                    statement.addNode(match("print"));
                    statement.addNode(expression());
                    break;
                case "assert":
                    statement.addNode(match("assert"));
                    statement.addNode(match("("));
                    statement.addNode(expression());
                    statement.addNode(match(")"));
                    break;
                case "ident":
                    statement.addNode(match_id());
                    statement.addNode(match(":="));
                    statement.addNode(expression());
                    break;
                default: 
                    throw new Exception($"A statement can't start with {symbol[1]}.");
            }
            return statement;
        }

        private Node expression() {
            Node expression = new Node("expression", "");
            switch (symbol[0]) {
                case "unary_op":
                    expression.addNode(match("!"));
                    expression.addNode(operand());
                    break;
                case "int":
                case "string":
                case "ident": 
                case "(": 
                    expression.addNode(operand()); 
                    break;
                default: throw new Exception($"Unexpected symbol {symbol[1]}, expected an operand, an opening paranthesis or an unary operator.");
            }
            if (symbol[0] == "op") {
                switch (symbol[1]) {
                    case "+": 
                        expression.addNode(match("+"));
                        break;
                    case "-": 
                        expression.addNode(match("-"));
                        break;
                    case "*": 
                        expression.addNode(match("*"));
                        break;
                    case "/": 
                        expression.addNode(match("/"));
                        break;
                    case "<": 
                        expression.addNode(match("<"));
                        break;
                    case "=": 
                        expression.addNode(match("="));
                        break;
                    case "&": 
                        expression.addNode(match("&"));
                        break;
                    default: throw new Exception($"Unexpected symbol {symbol[1]}, expected an operator.");
                }
                expression.addNode(operand());
            }
            return expression;
        }

        private Node operand() {
            Node operand = new Node("operand", "");
            switch (symbol[0]) {
                case "int":
                case "string":
                    operand.addNode(match_literal());
                    break;
                case "ident": 
                    operand.addNode(match_id());
                    break;
                case "(": 
                    operand.addNode(match("("));
                    operand.addNode(expression());
                    operand.addNode(match(")"));
                    break;
                default: throw new Exception($"Unexpected symbol {symbol[1]}, expected an operand or an opening paranthesis.");
            }
            return operand;
        }

        private Node type() {
            Node type = new Node("type", "");
            if (symbol[0] == "type") {
                switch (symbol[1]) {
                    case "int": 
                        type.addNode(match("int"));
                        break;
                    case "string": 
                        type.addNode(match("string"));
                        break;
                    case "bool": 
                        type.addNode(match("bool"));
                        break;
                    default: throw new Exception($"Unexpected symbol {symbol[1]}, expected a type.");
                }
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected a type.");
            }
            return type;
        }

        private Node match(String expected) {
            if (symbol[1] == expected) {
                Node matched = new Node(symbol[0], symbol[1]);
                //Console.WriteLine($"Matched symbol {symbol[1]}");
                symbol = scanner.getNext();
                return matched;
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected {expected}.");
            }
        }

        private Node match_id() {
            if (symbol[0] == "ident") {
                Node matched = new Node(symbol[0], symbol[1]);
                //Console.WriteLine($"Matched identifier {symbol[1]}");
                symbol = scanner.getNext();
                return matched;
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected an identifier.");
            }
        }

        private Node match_literal() {
            if (symbol[0] == "int" || symbol[0] == "string") {
                Node matched = new Node(symbol[0], symbol[1]);
                //Console.WriteLine($"Matched literal {symbol[1]}");
                symbol = scanner.getNext();
                return matched;
            } else {
                throw new Exception($"Unexpected symbol {symbol[1]}, expected a literal.");
            }
        }
    }

}