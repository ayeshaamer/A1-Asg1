using System;
using System.Collections.Generic;
using System.Collections;

namespace BCSF16M001_AI_1
{
    class Node
    {
        public Node parent;
        public int state;
        public int cost = 0;
        public int action;
    }
    class Program
    {
        static public void getMNTValue(ref int states, ref int rules, ref int test)
        {
            Console.WriteLine("Enter M N T Values: ");
            var str = Console.ReadLine();
            var numbers = str.Split(' ');
            states = Convert.ToInt32(numbers[0]);
            rules = Convert.ToInt32(numbers[1]);
            test = Convert.ToInt32(numbers[2]);
        }
        static void getStates(string[] statesArr, int length) {
            Console.WriteLine("\nEnter {0} States: ", length);
            for (int i = 0; i < length; i++) {
                statesArr[i] = Console.ReadLine();
            }
        }
        static void getRules(string[] rulesArr, int length)
        {
            Console.WriteLine("\nEnter {0} Rules: ", length);
            for (int i = 0; i < length; i++)
            {
                rulesArr[i] = Console.ReadLine();
            }
        }
        static void getTransModel(int[,] transModel, int states, int rules) {
            Console.WriteLine("\nEnter {0}x{1} Transistion Model: ", states, rules);
            for (int i = 0; i < states; i++) {
                var str = Console.ReadLine();
                var numbers = str.Split(' ');
                for (int j = 0; j < rules; j++) {
                    transModel[i, j] = Convert.ToInt32(numbers[j]);
                }
            }
        }
        static void printTransModel(int[,] transModel, int states, int rules) {
            for (int i = 0; i < states; i++) {
                for (int j = 0; j < rules; j++) {
                    Console.Write(transModel[i, j]);
                }
                Console.Write("\n");
            }
        }
        static void getTestArr(string[,] testArr, int test)
        {
            Console.WriteLine("\nEnter {0} Test Cases: ", test);
            for (int i = 0; i < test; i++)
            {
                var str = Console.ReadLine();
                var substr = str.Split(' ');
                for (int j = 0; j < 2; j++)
                {
                    testArr[i, j] = substr[j];
                }
            }
        }
        static bool GraphSearch(Node initial,Node final, int[,]transModel,int rules,Queue actionPerform)
        {
            //Breadth Search Algo
            Node node = new Node();
            Stack frontier = new Stack();
            HashSet<int> explored = new HashSet<int>();
            frontier.Push(initial);
            while (frontier.Count != 0) {
                node = (Node)frontier.Pop();
                if (node.state == final.state) {
                    actionPerform.Enqueue(node.action);
                    return true;
                }
                explored.Add(node.state);
                actionPerform.Enqueue(node.action);
                for (int i = 0; i < rules; i++) {
                    Node temp = new Node();
                    temp.state = transModel[node.state, i];
                    temp.parent = node;
                    temp.cost = temp.cost++;
                    temp.action = i;
                    if (explored.Contains(temp.state)== false && ifExistInFrontier(frontier,temp.state)==false) {
                        frontier.Push(temp);
                    }
                }
            }
            return false;
        }
        static int getStateNum(string[] statesArr, int length, string str) {
            for (int i = 0; i < length; i++) {
                if (statesArr[i] == str)
                {
                    return i;
                }
            }
            return -1;
        }
        static bool ifExistInFrontier(Stack frontier, int state) {
            foreach (Node node in frontier)
            {
                if (node.state == state) {
                    return true;
                }
            }
                return false;
        }
        static void Main(string[] args)
        {
            int states = 0 , rules = 0, test = 0;
            getMNTValue(ref states, ref rules, ref test); //get MNT values from user

            string[] statesArr = new string[states];
            getStates(statesArr,states); //get States from user
            
            string[] rulesArr = new string[rules];
            getRules(rulesArr, rules); //get Actions from user

            int[,] transModel = new int[states,rules];
            getTransModel(transModel, states, rules); //get transistion models from user assuming that user will use integers
            //printTransModel(transModel, states, rules);

            string[,] testArr = new string[test, 2]; 
            getTestArr(testArr, test); //get initial and final states from user

            for (int i = 0; i < test; i++) {
                Node initial = new Node();
                initial.state = getStateNum(statesArr, states, testArr[i,0]);
                initial.cost = 0;
                initial.parent = null;
                initial.action = -1;

                Node final = new Node();
                final.state = getStateNum(statesArr, states, testArr[i,1]);
                final.cost = 0;
                final.action = -1;
                final.parent = null;
                Queue actionPerformed = new Queue();
                bool isGoal = GraphSearch(initial, final, transModel, rules, actionPerformed);
                if (isGoal)
                {
                    foreach (int act in actionPerformed)
                    {
                        if (act != -1)
                        {
                            Console.Write("{0} -> ", rulesArr[act]);
                        }
                    }
                    Console.Write("Goal Achieved\n");
                }
                else
                {
                    Console.Write("Goal Can't be Achieved\n");
                }
            }
        }
    }
    
}
