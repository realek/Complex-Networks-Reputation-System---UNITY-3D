import networkx as nx
import numpy as np
import matplotlib.pyplot as plt


# good reading
# http://stackoverflow.com/questions/20133479/how-to-draw-directed-graphs-using-networkx-in-python

G = nx.Graph()
# edges = [
#     ('A', 'B'), ('D', 'B'), ('E', 'C'), ('A', 'C'),
#     ('E', 'F'), ('B', 'H'), ('B', 'G'), ('B', 'F'),
#     ('C', 'G'), ('C', 'H')
# ]
# G.add_edges_from(edges)

# rule A->B => ("A1", "A2"), ("A1", "B"), ("A1", "A2") 

G.add_edge(0, 1, label="A->B")
for itr in G.edges_iter(None, True, True):
    print(itr)

# actualy build it
for edge in G.edges_iter(None, True, True):
    label = edge[2]["label"]
    if(label != "A->B"): continue

    G.remove_edge(*edge[:2])



# Need to create a layout when doing
# separate calls to draw nodes and edges
pos = nx.spring_layout(G)
nx.draw_networkx_nodes(G, pos, cmap=plt.get_cmap('jet'), node_color = "black")
nx.draw_networkx_edges(G, pos, arrows=False)
plt.axis('off')
plt.show()


# nx.draw_networkx_edges(G, pos, edgelist=edges, arrows=False)
"""
__all__ = ['circular_layout',
           'random_layout',
           'shell_layout',
           'spring_layout',
           'spectral_layout',
           'fruchterman_reingold_layout']
"""
