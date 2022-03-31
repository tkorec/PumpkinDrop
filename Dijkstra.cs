using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{

    float smallestDistance;

    private List<string> WaypointsSequence(int startingNode, int endingNode, string[] previousNodesArray, List<string> nodesArray)
    {
        List<string> waypointSequence = new List<string>();
        string reversedNodeName = "";
        int reversedNodeIndex = endingNode;
        waypointSequence.Add(nodesArray[reversedNodeIndex]);
        while (reversedNodeName != nodesArray[startingNode])
        {
            reversedNodeName = previousNodesArray[reversedNodeIndex];
            reversedNodeIndex = nodesArray.FindIndex(i => i == reversedNodeName);
            waypointSequence.Add(nodesArray[reversedNodeIndex]);
        }

        // Reverse the order of waypoints in the array
        waypointSequence.Reverse();

        return waypointSequence;
    }

    public List<string> DijkstraAlgorithm(int startingNode, int endingNode, List<string> nodesArray, float[,] verticesArray)
    {
        // Array of unvisited nodes
        List<string> unvisitedNodesArray = new List<string>();

        /*
        The array of unvisited nodes has to be same as the array of all nodes so we can remove visited nodes from this list
        but they stay in the original array of nodes
        */
        foreach (string item in nodesArray)
        {
            unvisitedNodesArray.Add(item);
        }

        // Array of visited nodes
        List<string> visitedNodesArray = new List<string>();

        // Set array that has the same length as the array with names of nodes
        float[] distancesFromStartingNode = new float[nodesArray.Count];

        // Set array that wild hold the sequence of waypoints that will represent the shortes way between two nodes
        string[] previousNodesArray = new string[nodesArray.Count];

        /*
        The currentNode is the same as the startingNode but only at the beginning. It will change
        while looping through the adjacency matrix.
        */
        int currentNode = startingNode;


        /*
        The distance to current node. It is 0 at the starting point but it will change
        while changing the currentNode
        */
        float distanceToCurrentNode;

        // Set the value of starting node to 0 and for all other nodes set value to infinity
        for (int i = 0; i < distancesFromStartingNode.Length; i++)
        {
            if (i == startingNode)
            {
                distancesFromStartingNode.SetValue(0, i);
            }
            else
            {
                distancesFromStartingNode.SetValue(1000, i);
            }
        }


        // Until there are some unvisited nodes in the array of unvisited nodes
        while (unvisitedNodesArray.Count > 0)
        {
            distanceToCurrentNode = distancesFromStartingNode[currentNode];
            for (int j = 0; j < nodesArray.Count; j++)
            {
                /*
                If the distance in adjacency array is specified/different from infinity, 
                add this distance to distanceToCurrentNode and insert it into array of distanceFromCurrentNodes
                to index that represents the node that has specified this distance from the currentNode
                in the adjacency matrix

                Insert into previousNodeArray the name of currentNode at the index that represents the node
                to which we can travel from the currentNode using specified vertice with specified length
                */
                if (verticesArray[currentNode, j] != 1000)
                {
                    if (distancesFromStartingNode[j] > distanceToCurrentNode + verticesArray[currentNode, j] && distancesFromStartingNode[j] != 0)
                    {
                        distancesFromStartingNode.SetValue(distanceToCurrentNode + verticesArray[currentNode, j], j);
                        previousNodesArray.SetValue(nodesArray[currentNode], j);
                    }
                }
            }


            /*
            Add the currentNode to the array of visited nodes and
            remove it from the array of unvisited nodes
            */
            visitedNodesArray.Add(nodesArray[currentNode]);
            int indexOfCurrentNodeInUnvisitedNodesArray = unvisitedNodesArray.FindIndex(i => i == nodesArray[currentNode]);
            unvisitedNodesArray.RemoveAt(indexOfCurrentNodeInUnvisitedNodesArray);


            /*
            Loop through the distancesFromStartingNode
            Find the node with the smallest distance
            If this node is in unvisited nodes array,
            set as the currentNode
            */
            smallestDistance = 1000;
            for (int m = 0; m < distancesFromStartingNode.Length; m++)
            {
                if (distancesFromStartingNode[m] < smallestDistance && distancesFromStartingNode[m] != 0 && unvisitedNodesArray.Contains(nodesArray[m]) == true)
                {
                    smallestDistance = distancesFromStartingNode[m];
                    currentNode = m;
                }
            }
        }


        // Find the sequence of waypoints for the shortest path
        List<string> reversedWaypointsSequence = WaypointsSequence(startingNode, endingNode, previousNodesArray, nodesArray);

        return reversedWaypointsSequence;

    }
}
