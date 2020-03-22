# Interpreter-Mini-PL

This project is an interpreter meant for a Mini-PL language defined by the following grammar:

    <prog> ::= <stmts>
    <stmts> ::= <stmt> ";" ( <stmt> ";" )*
    <stmt> ::= "var" <var_ident> ":" <type> [ 
       ":=" <expr> ]
       | <var_ident> ":=" <expr>
       | "for" <var_ident> "in" <expr> ".." <expr> "do"
    <stmts> "end" "for"
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

## How to use

1. Install .NET core (dotnet) from [here](https://dotnet.microsoft.com/download).
2. Download or clone this project onto a local repository.
3. Run the command `dotnet run <filename>` in the root directory. (Where this README is). Replace the `<filename>` with for example `sample1.txt`.
