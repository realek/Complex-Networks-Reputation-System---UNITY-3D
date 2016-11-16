import networkx as nx
import numpy as np
import matplotlib.pyplot as plt

nodes = 100
edge_increment = 1
G = nx.barabasi_albert_graph(nodes, edge_increment, seed=None)
G2 = nx.barabasi_albert_graph(nodes, edge_increment, seed=None)

edge_counts = [len(G[node]) for node in G.nodes()]

node_labels = {}
for node in G.nodes():
    node_labels[node] = [len(G[node])]

for node in G2.nodes():
    node_labels[node].append(len(G2[node]))

pos = nx.spring_layout(G)
# nx.draw_networkx_nodes(G, pos, node_size=1500, node_color=edge_counts, edge_cmap=plt.cm.Reds) # if we want coloring
nx.draw_networkx_nodes(G, pos, node_size=2000, node_color="white")
nx.draw_networkx_edges(G, pos, edgelist=G.edges(), edge_color = "black")
nx.draw_networkx_edges(G, pos, edgelist=G2.edges(), edge_color = "red")
# nx.draw(G, pos, node_color = values, edge_color = edge_colors, edge_cmap=plt.cm.Reds)
nx.draw_networkx_labels(G,pos,node_labels,font_size=20)


plt.axis('off')
plt.show()


"""
layouts
__all__ = ['circular_layout',
           'random_layout',
           'shell_layout',
           'spring_layout',
           'spectral_layout',
           'fruchterman_reingold_layout']
"""
