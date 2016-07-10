using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Structure_for_Graphs
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern Boolean AllocConsole();
    }

    class Program
    {
        static void Main(string[] args)
        {
            PromptInput.mainLoop();
            // Console.ReadKey();
        }
    }

    public static class PromptInput
    {
        private static string line = "";
        private static bool exit = false;

        // the main loop
        public static void mainLoop()
        {
            do
            {
                // get input from the user
                Console.WriteLine("Please issue a command for execution.  Type commands to see the commands.");
                line = Console.ReadLine();

                // execute the request
                Controller.executeRequests(line);
                // check if exit or quit were chosen.
                updateExit(line);
            } while (!exit);
        }

        public static void updateExit(string line)
        {
            if (line == "exit" || line == "quit")
                exit = true;
            else
                exit = false;
        }
    }

    public static class Model
    {
        // PROPERTIES

        // Dictionary: For a node (key), what edges are attached to it (value)?
        public static Dictionary<Node, List<Edge>> dictionary = new Dictionary<Node, List<Edge>>();

        // Keeps the memory location of each node in a list
        public static List<Node> nodeList
        {
            get;
            set;
        } = new List<Node>();

        // Keeps the memory location of each edge in a list
        public static List<Edge> edgeList
        {
            get;
            set;
        } = new List<Edge>();

        // stores the location of the letters of the alphabet on a keyboard
        // top left corners of each key
        public static Dictionary<char, Tuple<int, int>> dictionaryKeyboardXYLocation = new Dictionary<char, Tuple<int, int>>
        {
            {'q', Tuple.Create(2, 3) },
            {'w', Tuple.Create(4, 3) },
            {'e', Tuple.Create(6, 3) },
            {'r', Tuple.Create(8, 3) },
            {'t', Tuple.Create(10, 3) },
            {'y', Tuple.Create(12, 3) },
            {'u', Tuple.Create(14, 3) },
            {'i', Tuple.Create(16, 3) },
            {'o', Tuple.Create(18, 3) },
            {'p', Tuple.Create(20, 3) },
            {'a', Tuple.Create(3, 5) },
            {'s', Tuple.Create(5, 5) },
            {'d', Tuple.Create(7, 5) },
            {'f', Tuple.Create(9, 5) },
            {'g', Tuple.Create(11, 5) },
            {'h', Tuple.Create(13, 5) },
            {'j', Tuple.Create(15, 5) },
            {'k', Tuple.Create(17, 5) },
            {'l', Tuple.Create(19, 5) },
            {'z', Tuple.Create(4, 7) },
            {'x', Tuple.Create(6, 7) },
            {'c', Tuple.Create(8, 7) },
            {'v', Tuple.Create(10, 7) },
            {'b', Tuple.Create(12, 7) },
            {'n', Tuple.Create(14, 7) },
            {'m', Tuple.Create(16, 7) }
        };

        public static Dictionary<char, string> adjacentDictionary = new Dictionary<char, string>
        {
            {'q', "aw" },
            {'w', "qase"},
            {'e', "wsdr"},
            {'r', "edft"},
            {'t', "rfgy"},
            {'y',  "tghu"},
            {'u',  "yhji"},
            {'i',  "ujko"},
            {'o',  "iklp"},
            {'p',  "ol"},
            {'a', "qwsz"},
            {'s', "weadzx"},
            {'d', "ersfxc"},
            {'f', "rtdgcv"},
            {'g', "tyfhvb"},
            {'h',  "yugjbn"},
            {'j',  "uihknm"},
            {'k',  "iojlm"},
            {'l',  "opk"},
            {'z', "asx"},
            {'x', "sdzc"},
            {'c', "dfxv"},
            {'v', "fgcb"},
            {'b',  "ghvn"},
            {'n',  "hjbm"},
            {'m',  "jkn"}
        };
    }


    // unimplemented 
    public static class View
    {

    }

    public static class Controller
    {
        //// METHODS

        // CRUD Node
        public static void createNode(string key, int xLocation, int yLocation)
        {
            Node.Create(key, xLocation, yLocation);
        }

        public static Node selectNode(string searchKey)
        {
            foreach (var element in Model.nodeList)
            {
                if (element.key == searchKey)
                    return element;
            }
            Console.WriteLine("Tried to select a node but no such node exists");
            return null;
        }

        public static void modifyNode(Node node, string key, int xLocation, int yLocation)
        {
            // We don't just delete and recreate the node because then the edges
            // that depend on the node would be affected.

            // if this is a unique node key then there is no problem changing the values
            if (Controller.isUniqueNodeKey(key))
            {
                node.key = key;
                node.xLocation = xLocation;
                node.yLocation = yLocation;
            }
            // If it is not unique but has the same key value then we allow the user to update the location only
            else if (key == node.key)
            {
                node.xLocation = xLocation;
                node.yLocation = yLocation;
            }
            else
            {
                Console.WriteLine("Failed to modify node.  Not a unique key.");
                return;
            }
        }

        public static void deleteNode(Node node)
        {
            // remove all edges attached to the node
            foreach (Edge edge in Model.dictionary[node])
            {
                deleteEdge(edge);
            }
            // remove the entry from the dictionary
            Model.dictionary.Remove(node);
            // remove the node from the node list
            Model.nodeList.Remove(node);
        }

        // CRUD Edge
        public static void createEdge(string key, string description, Node node1, Node node2)
        {
            Edge.Create(key, description, node1, node2);
        }

        public static Edge selectEdge(string searchKey)
        {
            foreach (var element in Model.edgeList)
            {
                if (element.key == searchKey)
                    return element;
            }
            Console.WriteLine("Tried to select an edge but no such edge exists");
            return null;
        }

        public static void modifyEdge(Edge edge, string key, string description, Node node1, Node node2)
        {
            // it is okay to delete the edge and recreate it here because
            // nodes aren't depending on the same edge instance existing.
            deleteEdge(edge);
            createEdge(key, description, node1, node2);
        }

        public static void deleteEdge(Edge edge)
        {
            Model.dictionary[edge.node1].Remove(edge);
            Model.dictionary[edge.node2].Remove(edge);
            Model.edgeList.Remove(edge);
        }

        // Validation methods
        public static bool isUniqueNodeKey(string checkKey)
        {
            int nodeListLength = Model.nodeList.Count();
            for (var i = 0; i < nodeListLength; i++)
            {
                if (checkKey == Model.nodeList[i].key)
                    return false;
            }
            return true;
        }
        public static bool isUniqueEdgeKey(string checkKey)
        {
            int edgeListLength = Model.edgeList.Count();
            for (var i = 0; i < edgeListLength; i++)
            {
                if (checkKey == Model.edgeList[i].key)
                    return false;
            }
            return true;
        }

        // Maybe this can be improved with a dictionary
        public static bool isNode(int x, int y)
        {
            foreach (var node in Model.nodeList)
            {
                if (node.xLocation == x && node.yLocation == y)
                    return true;
            }
            return false;
        }

        // Graph associations, what is attached to each other
        public static List<Edge> selectLinkedEdges(Node queryNode)
        {
            return Model.dictionary[queryNode];
        }

        public static List<Node> selectNearestLinkedNodes(Node queryNode)
        {
            List<Node> nodes = new List<Node>();
            foreach (var edge in selectLinkedEdges(queryNode))
            {
                if (edge.node1 != queryNode) // then it is a new node
                    nodes.Add(edge.node1);
                if (edge.node2 != queryNode) // then it is a new node
                    nodes.Add(edge.node2);
            }
            return nodes.Distinct().ToList(); // remove any duplication, then return the node list
        }

        // executes the request that comes from the transport
        public static void executeRequests(string input)
        {
            string line = "";
            // See if the user wants to create nodes.  Each letter on the keyboard has a name (ie. a, b, o) and an (X,Y) location.
            // Each letter the user types can create a node.
            if (System.Text.RegularExpressions.Regex.IsMatch(input, "create nodes", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                while (true)
                {
                    Console.WriteLine("Please type the letters a to z to create your nodes");
                    line = Console.ReadLine().ToLower();
                    if (isStringAllAlphabeticalChars(line))
                        break;
                }
                // create a node for each letter in the line
                foreach (char letter in line)
                {
                    createNode(letter.ToString(), Model.dictionaryKeyboardXYLocation[letter].Item1, Model.dictionaryKeyboardXYLocation[letter].Item2);
                }
            }

            // Check to see if the user wants to connect nodes with edges
            if (System.Text.RegularExpressions.Regex.IsMatch(input, "create edges", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                // if there are an odd number of edge choices or if they don't match a node they will be ignored
                while (true)
                {
                    Console.WriteLine("Please type the letters a to z to create your edges.");
                    Console.WriteLine("Edges must be adjacent to the letter (ie. for x choices are zasdc");
                    line = Console.ReadLine().ToLower();
                    if (isStringAllAlphabeticalChars(line))
                        break;
                }
                // create a node for each letter in the line
                int lineLength = line.Length;
                for (var i = 1; i < lineLength; i += 2)
                {
                    // Check if the nodes are created before connecting the edges
                    // Also check if the nodes are adjacent with the adjacent dictionary.
                    if (selectNode(line[i - 1].ToString()) != null && selectNode(line[i].ToString()) != null &&
                        Model.adjacentDictionary[line[i - 1]].Contains(line[i]))
                    {
                        createEdge(String.Concat(line[i - 1].ToString(), line[i].ToString()), "",
                            selectNode(line[i - 1].ToString()), selectNode(line[i].ToString()));
                    }
                }
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(input, "print graph", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                Application.EnableVisualStyles();
                Application.Run(new GraphWind());
            }

            //if (input == "comands")
            if (System.Text.RegularExpressions.Regex.IsMatch(input, "commands", System.Text.RegularExpressions.RegexOptions.IgnoreCase))            
            {
                Console.WriteLine("The commands are");
                Console.WriteLine("create nodes");
                Console.WriteLine("create edges");
                Console.WriteLine("print graph");
                Console.WriteLine("quit");
                Console.WriteLine("exit");
            }
        }

        public static bool isStringAllAlphabeticalChars(string str)
        {
            Regex rgx = new Regex("^[A-Za-z]+$");
            return rgx.IsMatch(str);
        }

    }   

    public class Node
    {
        // CONSTRUCTORS
        private Node(string key, int xLocation, int yLocation)
        {
            this.key = key;
            this.xLocation = xLocation;
            this.yLocation = yLocation;
        }

        public static void Create(string key, int xLocation, int yLocation)
        {
            if (Controller.isUniqueNodeKey(key))
            {
                Node newNode = new Node(key, xLocation, yLocation);
                // The graphing controller needs to update the node list
                Model.nodeList.Add(newNode);
                // The graphing controller needs to update its dictionary
                Model.dictionary.Add(newNode, new List<Edge>());
                return;
            }
            else
            {
                Console.WriteLine("Failed to create node.  Not a unique key.");
                return;
            }
        }
        // PROPERTIES
        public string key
        {
            get;
            set;
        }

        public int xLocation
        {
            get;
            set;
        }

        public int yLocation
        {
            get;
            set;
        }

        // METHODS
        public override string ToString()
        {
            return String.Concat("Node ", key, " - At location (", xLocation, ",", yLocation, ")");
        }
    }

    public class Edge
    {
        // CONSTRUCTORS
        private Edge(string key, string description, Node node1, Node node2)
        {
            this.description = description;
            this.key = key;
            this.node1 = node1;
            this.node2 = node2;
        }

        public static void Create(string key, string description, Node node1, Node node2)
        {
            if (Controller.isUniqueEdgeKey(key))
            {
                Edge newEdge = new Edge(key, description, node1, node2);
                // The graphing controller needs to update the edge list
                Model.edgeList.Add(newEdge);
                // The graphing controller needs to update its dictionary twice
                Model.dictionary[node1].Add(newEdge);
                Model.dictionary[node2].Add(newEdge);

                return;
            }
            else
            {
                Console.WriteLine("Failed to create edge.  Not a unique key.");
                return;
            }
        }

        // PROPERTIES
        public string description
        {
            get;
            set;
        }
        public string key
        {
            get;
            set;
        }
        public Node node1
        {
            get;
            set;
        }
        public Node node2
        {
            get;
            set;
        }

        // METHODS
        public override string ToString()
        {
            return String.Concat("Edge ", key, " - ", description);
        }

    }
}