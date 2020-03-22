using System;
using System.Collections;
using System.Collections.Generic;

namespace interpreter {
    class Interpreter {
        private Node tree;
        private Dictionary<string, string[]> symbol_table = new Dictionary<string, string[]>(); // [Type, Value, Locked]
        private string[] reserved = {"var", "for", "end", "in", "do", "read", "print", "int", "string", "bool", "assert"};

        public Interpreter(Node tree) {
            this.tree = tree;
        }

        public void interpret() {
            visitStatemnts(tree);
        }

        private void visitStatemnts(Node statements) {
            foreach(Node statement in statements.getChildren()) {
                visitStatement(statement);
            }
        }

        private void visitStatement(Node statement) {
            ArrayList children = statement.getChildren();
            switch (((Node)children[0]).getType()) {
                case "var":
                    string new_ident_var = ((Node)children[1]).getLexeme();
                    if (Array.Exists(reserved, element => element == new_ident_var)) {
                        throw new Exception($"An identifier can't have the same name as the reserved word {new_ident_var}");
                    }
                    if (symbol_table.ContainsKey(new_ident_var)) {
                        throw new Exception($"Identifier {new_ident_var} already declared.");
                    }
                    string type = visitType((Node)children[3]);
                    string[] var_arr;
                    if (children.Count > 4) {
                        string[] value = visitExpression((Node)children[5]);
                        if (value[0] == type) {
                            var_arr = new string[]{type, value[1], "False"};
                        } else throw new Exception($"{value[1]} is not of type {type}."); 
                    } else var_arr = new string[]{type, "", "False"};
                    symbol_table.Add(new_ident_var, var_arr);
                    break;
                case "ident":
                    string var_ident_assign = ((Node)children[0]).getLexeme();
                    if (symbol_table.ContainsKey(var_ident_assign)) {
                        if (symbol_table[var_ident_assign][0] == "True") {
                            throw new Exception($"Can't change control variable {var_ident_assign} of for loop.");
                        }
                        string[] ident_expr = visitExpression((Node)children[2]);
                        string[] new_value = new string[]{ident_expr[0], ident_expr[1], "False"};
                        symbol_table[var_ident_assign] = new_value;
                    } else throw new Exception($"Variable {var_ident_assign} has not been declared.");
                    break;
                case "for":
                    string var_ident_for = ((Node)children[1]).getLexeme();
                    if (symbol_table.ContainsKey(var_ident_for)) {
                        string[] var_for = symbol_table[var_ident_for];
                        if (var_for[0] == "int") {
                            string[] lower_bound = visitExpression((Node)children[3]);
                            if (lower_bound[0] == "int") {
                                string[] for_new_value = {"int", lower_bound[1], "True"};
                                symbol_table[var_ident_for] = for_new_value;
                                string[] upper_bound = visitExpression((Node)children[5]);
                                if (upper_bound[0] == "int") {
                                    int lower = Int32.Parse(lower_bound[1]);
                                    int upper = Int32.Parse(upper_bound[1]);
                                    if (lower <= upper) {
                                        string[] new_ident;
                                        for (var i = lower; i <= upper; i++ ) {
                                            var_for = symbol_table[var_ident_for];
                                            new_ident = new string[]{var_for[0], i.ToString(), "True"};
                                            symbol_table[var_ident_for] = new_ident;
                                            visitStatemnts((Node)children[7]);
                                        }
                                        var_for = symbol_table[var_ident_for];
                                        new_ident = new string[]{var_for[0], var_for[1], "False"};
                                        symbol_table[var_ident_for] = new_ident;
                                    } else throw new Exception($"Lower bound {lower} has to be smaller or equal than upper bound {upper} in for loop.");
                                } else throw new Exception($"Upper bound {upper_bound[1]} in a for loop has a to be an integer.");
                            } else throw new Exception($"Lower bound {lower_bound[1]} in a for loop has a to be an integer.");
                        } else throw new Exception($"For control variable {var_for[1]} has to be an integer.");
                    }
                    break;
                case "read":
                    string var_ident_read = ((Node)children[1]).getLexeme();
                    if (symbol_table.ContainsKey(var_ident_read)) {
                        if (symbol_table[var_ident_read][2] == "True") {
                            throw new Exception($"Can't change control variable {var_ident_read} of for loop.");
                        }
                        string text = Console.ReadLine();
                        int number;
                        if (Int32.TryParse(text, out number)) {
                            string[] read_arr = {"int", number.ToString(), "False"};
                            this.symbol_table[var_ident_read] = read_arr;
                        } else {
                            string[] arr = {"string", text, "False"};
                            this.symbol_table[var_ident_read] = arr;
                        }
                    } else throw new Exception($"Variable {var_ident_read} has not been declared.");
                    break;
                case "print":
                    string[] to_print = visitExpression((Node)children[1]);
                    Console.WriteLine(to_print[1]);
                    break;
                case "assert":
                    var expr = visitExpression((Node)children[2]);
                    if (expr[0] == "bool") {
                        if (expr[1] == "False") {
                            Console.WriteLine($"Assertion failed.");
                        }
                    } else throw new Exception($"Unexpected type {expr[1]} used in assert.");
                    break;
                default: throw new Exception($"Unexpected statement type {((Node)children[0]).getType()}.");
            }
        }

        private string[] visitExpression(Node expression) {
            //Console.WriteLine("visiting expression.");
            ArrayList children = expression.getChildren();
            if (((Node)children[0]).getType() == "unary_op") {
                var opnd = visitOperand((Node)children[1]);
                if (opnd[0] == "bool") {
                    string flipped = "False";
                    if (opnd[1] == "False") {
                        flipped = "True";
                    }
                    return new string[]{"bool", flipped};
                } else throw new Exception("Expected a Boolean after unary_op \"!\".");
            } else {
                if (children.Count > 1) {
                    var first = visitOperand((Node)children[0]);
                    var second = visitOperand((Node)children[2]);
                    switch (((Node)children[1]).getLexeme()) {
                        case "+":
                            if (first[0] == second[0]) {
                                    if (first[0] == "int") {
                                        int first_add = Int32.Parse(first[1]);
                                        int second_add = Int32.Parse(second[1]);
                                        return new[]{"int", (first_add + second_add).ToString()};
                                    } else if (first[0] == "string"){
                                        string first_add = first[1];
                                        string second_add = second[1];
                                        return new[]{"string", (first_add + second_add).ToString()};
                                    } else throw new Exception($"Types of {first[1]} and {second[1]} need to be integer or string.");
                            } else throw new Exception($"Types of {first[1]} and {second[1]} don't match.");
                        case "-":
                            if (first[0] == "int") {
                                if (second[0] == "int") {
                                    int first_substract = Int32.Parse(first[1]);
                                    int second_substract = Int32.Parse(second[1]);
                                    return new[]{"int", (first_substract - second_substract).ToString()};
                                } else throw new Exception($"{second[1]} is not an integer.");
                            } else throw new Exception($"{first[1]} is not an integer.");
                        case "*":
                            if (first[0] == "int") {
                                if (second[0] == "int") {
                                    int first_multiply = Int32.Parse(first[1]);
                                    int second_multiply = Int32.Parse(second[1]);
                                    return new[]{"int", (first_multiply * second_multiply).ToString()};
                                } else throw new Exception($"{second[1]} is not an integer.");
                            } else throw new Exception($"{first[1]} is not an integer.");
                        case "/": 
                            if (first[0] == "int") {
                                if (second[0] == "int") {
                                    int first_division = Int32.Parse(first[1]);
                                    int second_division = Int32.Parse(second[1]);
                                    return new []{"int", (first_division / second_division).ToString()};
                                } else throw new Exception($"{second[1]} is not an integer.");
                            } else throw new Exception($"{first[1]} is not an integer.");
                        case "<":
                            if (first[0] == second[0]) {
                                if (first[0] == "int" || first[0] == "string") {
                                    if (first[0] == "int") {
                                        int first_compare = Int32.Parse(first[1]);
                                        int second_compare = Int32.Parse(second[1]);
                                        return new[]{"bool", (first_compare < second_compare).ToString()};
                                    } else {
                                        string first_compare = first[1];
                                        string second_compare = second[1];
                                        return new[]{"bool" ,(first_compare.CompareTo(second_compare) == -1).ToString()};
                                    }
                                } else throw new Exception($"Types of {first[1]} and {second[1]} need to be integer or string.");
                            } else throw new Exception($"Types of {first[1]} and {second[1]} don't match.");
                        case "=":
                            if (first[0] == second[0]) {
                                return new string[]{"bool", (first[1] == second[1]).ToString()};
                            } else throw new Exception($"Types of {first[1]} and {second[1]} don't match.");
                        case "&":
                            if (first[0] == "bool") {
                                if (second[0] == "bool") {
                                    if (first[1] == "True" && second[1] == "True") {
                                        return new[]{"bool", "True"};
                                    } else return new[]{"bool", "False"};
                                } else throw new Exception($"{second[1]} is not a boolean. Boolean required for & operator.");
                            } else throw new Exception($"{first[1]} is not a boolean. Boolean required for & operator.");
                        default: throw new Exception($"Unexpected type of operator {((Node)children[1]).getLexeme()}.");
                    }
                // if no operators.
                } else return visitOperand((Node)children[0]);
            }
        }

        private dynamic visitOperand(Node operand) {
            ArrayList children = operand.getChildren();
            if (((Node)children[0]).getLexeme() == "(") {
                return visitExpression((Node)children[1]);
            } else {
                return visitLeaf((Node)operand.getChildren()[0]);
            }
        }

        private string visitType(Node type) {
            return ((Node)type.getChildren()[0]).getLexeme();
        }

        private dynamic visitLeaf(Node leaf) {
            if (leaf.getType() == "ident") {
                if (symbol_table.ContainsKey(leaf.getLexeme())) {
                    string[] ident = symbol_table[leaf.getLexeme()];
                    return new string[]{ident[0], ident[1]};
                } else throw new Exception($"Identifier {leaf.getLexeme()} has not been declared.");
            }
            return new string[]{leaf.getType(), leaf.getLexeme()};
        }
    }
}