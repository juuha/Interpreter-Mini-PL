using System;
using System.Collections;

namespace interpreter {
    class Node {
        private string type;
        private string lexeme;
        private ArrayList children = new ArrayList();

        public Node(string type, string lexeme) {
            this.type = type;
            this.lexeme = lexeme;
        }

        public Node() {}

        public void addNode(Node newNode) {
            this.children.Add(newNode);
        }

        public string getLexeme() {
            return this.lexeme;
        }

        public string getType() {
            return this.type;
        }        

        public ArrayList getChildren() {
            return this.children;
        }
    }
}