using Lab5;

namespace UnitTests;

[TestClass]
public class UnitTests
{

    [TestClass]
    public class UndirectedWeightedGraphTests
    {
        [TestMethod]
        public void AddEdge_ConnectsNodesInBothDirections()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("X"));
            graph.Nodes.Add(new Node("Y"));

            graph.AddEdge("X", "Y", 4);

            var nodeX = graph.Nodes.Find(n => n.Name == "X");
            var nodeY = graph.Nodes.Find(n => n.Name == "Y");

            Assert.IsTrue(nodeX.Neighbors.Any(n => n.Node == nodeY && n.Weight == 4));
            Assert.IsTrue(nodeY.Neighbors.Any(n => n.Node == nodeX && n.Weight == 4));
        }
        [TestMethod]
        public void TestDijkstra()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("A"));
            graph.Nodes.Add(new Node("B"));
            graph.Nodes.Add(new Node("C"));
            graph.Nodes.Add(new Node("D"));


            graph.AddEdge("A", "B", 1);
            graph.AddEdge("B", "C", 2);
            graph.AddEdge("C", "D", 3);
            graph.AddEdge("A", "D", 10);

            var dijkstraPath = graph.DijkstraPathBetween("A", "D", out var dijkstraNodes);
            Assert.AreEqual(6, dijkstraPath);
            Assert.AreEqual(4, dijkstraNodes.Count);
            Assert.AreEqual("A", dijkstraNodes[0].Name);
            Assert.AreEqual("D", dijkstraNodes[3].Name);
        }


        [TestMethod]
        public void TestEmptyGraph()
        {
            var graph = new UndirectedWeightedGraph();
            Assert.AreEqual(0, graph.Nodes.Count);
            Assert.AreEqual(0, graph.ConnectedComponents);
            Assert.ThrowsException<Exception>(() => graph.IsReachable("A", "B"));
        }


        [TestMethod]
        public void AddEdge_IgnoresMissingNodes()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("A"));

            // Do not add node B
            Assert.ThrowsException<Exception>(() => graph.AddEdge("A", "B", 3));
        }

        [TestMethod]
        public void IsReachable_ReturnsTrueIfPathExists()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("A"));
            graph.Nodes.Add(new Node("B"));
            graph.Nodes.Add(new Node("C"));
            graph.Nodes.Add(new Node("D"));

            graph.AddEdge("A", "B", 5);
            graph.AddEdge("B", "C", 3);
            graph.AddEdge("C", "D", 2);

            bool reachable = graph.IsReachable("A", "D");

            Assert.IsTrue(reachable, "Expected A to be reachable to D.");
        }


        [TestMethod]
        public void ConnectedComponents_SeparateClusters_ReturnsCorrectCount()
        {
            var graph = new UndirectedWeightedGraph();

            graph.Nodes.Add(new Node("A"));
            graph.Nodes.Add(new Node("B"));
            graph.Nodes.Add(new Node("C"));
            graph.Nodes.Add(new Node("D"));
            graph.Nodes.Add(new Node("E"));
            graph.Nodes.Add(new Node("F"));

            graph.AddEdge("A", "B", 1);
            graph.AddEdge("B", "C", 1);
            graph.AddEdge("D", "E", 1);
            

            int components = graph.ConnectedComponents;
            Assert.AreEqual(3, components, "Expected 3 connected components (ABC, DE, F)");
        }

        [TestMethod]
        public void DFSPathBetween_ReturnsCorrectPathAndCost()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("A"));
            graph.Nodes.Add(new Node("B"));
            graph.Nodes.Add(new Node("C"));
            graph.Nodes.Add(new Node("D"));

            graph.AddEdge("A", "B", 2);
            graph.AddEdge("B", "C", 3);
            graph.AddEdge("C", "D", 4);

            int cost = graph.DFSPathBetween("A", "D", out var path);

            Assert.AreEqual(9, cost, "Expected total DFS cost from A to D to be 9");
            Assert.AreEqual(4, path.Count, "Expected path to include 4 nodes");
            Assert.AreEqual("A", path[0].Name);
            Assert.AreEqual("D", path[3].Name);
        }

        [TestMethod]
        public void DFSPathBetween_ReturnsEmptyPathAndZeroCostIfUnreachable()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("A"));
            graph.Nodes.Add(new Node("B"));
            graph.Nodes.Add(new Node("C")); 

            graph.AddEdge("A", "B", 1); 

            int cost = graph.DFSPathBetween("A", "C", out var path);

            Assert.AreEqual(0, cost, "Expected cost 0 when no path exists");
            Assert.AreEqual(0, path.Count, "Expected empty path when no path exists");
        }



        [TestMethod]
        public void BFSPathBetween_ReturnsCorrectPathAndCost()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("A"));
            graph.Nodes.Add(new Node("B"));
            graph.Nodes.Add(new Node("C"));
            graph.Nodes.Add(new Node("D"));

            graph.AddEdge("A", "B", 1);
            graph.AddEdge("B", "C", 2);
            graph.AddEdge("C", "D", 3);
            graph.AddEdge("A", "D", 10);

            int cost = graph.BFSPathBetween("A", "D", out var path);

            Assert.AreEqual(10, cost, "Expected BFS to return cost 10 for direct A->D path");
            Assert.AreEqual(2, path.Count, "Expected path to include A and D only");
            Assert.AreEqual("A", path[0].Name);
            Assert.AreEqual("D", path[1].Name);
        }


        [TestMethod]
        public void BFSPathBetween_ReturnsEmptyPathAndZeroCostIfUnreachable()
        {
            var graph = new UndirectedWeightedGraph();
            graph.Nodes.Add(new Node("X"));
            graph.Nodes.Add(new Node("Y")); 

            int cost = graph.BFSPathBetween("X", "Y", out var path);

            Assert.AreEqual(0, cost, "Expected cost 0 when no path exists");
            Assert.AreEqual(0, path.Count, "Expected empty path when no path exists");
        }

    }




    // [TestMethod]
    // public void Graph1IsReachable()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph1.txt");

    //     Assert.IsTrue(undirectedGraph.IsReachable("a", "c"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("e", "c"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("d", "e"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("d", "c"));
    // }



    // [TestMethod]
    // public void Graph1ConnectedComponents()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph1.txt");

    //     Assert.AreEqual(1, undirectedGraph.ConnectedComponents);
    // }


    // [TestMethod]
    // public void Graph2IsReachable()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph2.txt");

    //     Assert.IsFalse(undirectedGraph.IsReachable("a", "c"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("e", "c"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("d", "e"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("b", "a"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("d", "b"));

    // }

    // [TestMethod]
    // public void Graph2ConnectedComponents()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph2.txt");

    //     Assert.AreEqual(5, undirectedGraph.ConnectedComponents);
    // }


    // [TestMethod]
    // public void Graph3IsReachable()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph3.txt");

    //     Assert.IsTrue(undirectedGraph.IsReachable("a", "c"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("e", "d"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("h", "g"));

    //     Assert.IsFalse(undirectedGraph.IsReachable("a", "h"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("c", "i"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("g", "b"));

    // }

    // [TestMethod]
    // public void Graph3ConnectedComponents()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph3.txt");

    //     Assert.AreEqual(3, undirectedGraph.ConnectedComponents);
    // }

    // [TestMethod]
    // public void Graph4IsReachable()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph4.txt");

    //     Assert.IsTrue(undirectedGraph.IsReachable("a", "c"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("e", "i"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("g", "b"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("c", "f"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("a", "d"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("b", "i"));

    // }

    // [TestMethod]
    // public void Graph4ConnectedComponents()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/graph4.txt");

    //     Assert.AreEqual(1, undirectedGraph.ConnectedComponents);
    // }

    // [TestMethod]
    // public void SavannahIsReachable()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/Savannah.txt");

    //     Assert.IsTrue(undirectedGraph.IsReachable("a", "c"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("e", "i"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("g", "b"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("c", "f"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("a", "j"));
    //     Assert.IsTrue(undirectedGraph.IsReachable("b", "i"));


    //     Assert.IsFalse(undirectedGraph.IsReachable("a", "d"));
    //     Assert.IsFalse(undirectedGraph.IsReachable("d", "j"));

    // }

    // [TestMethod]
    // public void SavannahConnectedComponents()
    // {
    //     UndirectedUnweightedGraph undirectedGraph = new UndirectedUnweightedGraph("../../../graphs/Savannah.txt");

    //     Assert.AreEqual(2, undirectedGraph.ConnectedComponents);
    // }

}

