import networkx as nx
import numpy as np
import matplotlib.pyplot as plt

nodes = 100
edge_increment = 1

G = nx.erdos_renyi_graph(100,0.1)

pos = nx.spring_layout(G)
nx.draw_networkx_nodes(G, pos, cmap=plt.get_cmap('jet'), node_color = "black")
nx.draw_networkx_edges(G, pos, arrows=False, edge_color = "red")
plt.axis('off')
plt.show()


"""
__all__ = ['circular_layout',
           'random_layout',
           'shell_layout',
           'spring_layout',
           'spectral_layout',
           'fruchterman_reingold_layout']
"""
