using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Msagl.DebugHelpers;
using Microsoft.Msagl.Drawing;

namespace AoC23.Day25;

public class Day25
{
    public static long Part1(string input, List<(string, string)> cutWires)
    {
        Graph graph = new();
        foreach (string line in StringUtilities.SplitLines(input))
        {
            // Manually selected nodes to skip
            if (line[0] == '-') continue;
            AddNode(ref graph, line, cutWires);
        }

        // Found the cut edges manually using Win Forms with the function below
        // ShowGraph(graph);

        int totalEdges = graph.EdgeCount;
        var firstNode = graph.Nodes.ElementAt(0);

        HashSet<string> visited = new();
        Queue<Node> toCheck = new();
        toCheck.Enqueue(firstNode);

        while (toCheck.Count > 0)
        {
            var nextNode = toCheck.Dequeue();
            var connections = nextNode.Edges;
            foreach (var edge in connections)
            {
                Node from = edge.SourceNode;
                if (!visited.Contains(from.Id))
                {
                    visited.Add(from.Id);
                    toCheck.Enqueue(from);
                }
                Node to = edge.TargetNode;
                if (!visited.Contains(to.Id))
                {
                    visited.Add(to.Id);
                    toCheck.Enqueue(to);
                }
            }
        }

        return visited.Count * (graph.NodeCount - visited.Count);
    }

    private static void AddNode(ref Graph graph, string line, List<(string, string)> cutNodes)
    {
        string[] parts = StringUtilities.SplitWithTrim(line, ":");
        string from = parts[0];
        string[] allTo = StringUtilities.SplitWithTrim(parts[1], " ");

        foreach (string to in allTo)
        {
            // If we want this node cut, skip it
            if (cutNodes.Contains((from, to)) || cutNodes.Contains((to, from))) continue;
            graph.AddEdge(from, to);
        }
    }

    public static void ShowGraph(Graph graph)
    {
        //create a form 
        System.Windows.Forms.Form form = new System.Windows.Forms.Form();
        //create a viewer object 
        Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //bind the graph to the viewer 
        viewer.Graph = graph;
        //associate the viewer with the form 
        form.SuspendLayout();
        viewer.Dock = System.Windows.Forms.DockStyle.Fill;
        form.Controls.Add(viewer);
        form.ResumeLayout();
        //show the form 
        form.ShowDialog();
    }

    public static long Part2(string input)
    {
        return -1;
    }
}