# CRUD-Data-Structure-and-Visualization-of-Graphs

The "CRUD Data Structure and Visualization of Graphs" project allows a user to create example graphs using simple console commands.  The node and edge data structures can also be reused in another project.

Main Methods:
- create, read, update, and delete nodes and edges
- validate input
- prompt user input, read user input, execute commands

Main Classes:
- the graphics engine: draws nodes, edges, and labels to the graphics window
- graphics manager: manages the graphics engine and room
- node, edge
- model: holds a list of all nodes, edges, a dictionary for all edges for each node, and two other dictionaries
- controller: manages CRUD, data validation, reading user input, executing commands
- room: finds the location of the nodes on the window and stores the values into an array
- form