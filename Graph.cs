using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using TMPro;

public class Graph : MonoBehaviour
{
    // Object to which we can include objects representing graph's nodes via Unity editor
    public GameObject[] waypoints;

    public GameObject[] allNPCs;

    string[] approachingStatus = new string[2];

    public GameObject npcCharacter;

    // Instance of Dijkstra class that contains implementation of Dijkstra algorithm
    public Dijkstra dijkstra = new Dijkstra();

    //public Behavior behavior = new Behavior();

    // Array of strings – names of graph's nodes
    public List<string> nodesArray = new List<string>();

    // Array of waypoints/nodes that work like starting and finishing nodes
    List<string> startEndNodesArray = new List<string>();

    public List<string> navigationSequence;

    public int endingNode;
    public int approachingNode;
    public int approachingNodeIndex;
    string changeBehaviorNode;
    int randomChoiseOfBehavior;
    public bool movingStatus;
    public float approachingToNodeSpeed;
    public bool isPlayerHiden = false;
    //int countDown;
    int thisNPC;
    int previouslyApproachedNode;

    public TMP_Text hitStudentsText;

    // Multidimensional array representing the vertices between graph's nodes
    public float[,] verticesArray = new float[29, 29] {
    //   A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  R  S  T  U  V  W  X  Y  Z  AA  AB  AC  AD
        {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeA
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeB
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeC
        {1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,  0,  0,  0}, // NodeD
        {0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeE
        {0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeF
        {0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  1,  0}, // NodeG
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeH
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeI
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeJ
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeK
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeL
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeM
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeN
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeO
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeP
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeR
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeS
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeT
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeU
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0,  0,  0,  0}, // NodeV
        {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1,  0,  0,  0}, // NodeW
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0,  1,  0,  0}, // NodeX
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,  0,  1,  0}, // NodeY
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,  0,  0,  0}, // NodeZ
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,  0,  0,  0}, // NodeAA
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,  0,  0,  0}, // NodeAB
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,  0,  0,  1}, // NodeAC
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,  0,  1,  0}  // NodeAD
    };



    // Start is called before the first frame update
    void Start()
    {
        // Add waypoints from the list of game objects to array
        for (int x = 0; x < waypoints.Length; x++)
        {
            nodesArray.Add(waypoints[x].name);
        }

        /* 
        Add nodes from nodesArray whose name contains StartEnd string. These nodes defines starting
        nodes from and to which the agent can go. The agent will choose from this list its starting
        and ending nodes later so Dijkstra's algorithm can be executed.
        */
        foreach (string item in nodesArray)
        {
            if (item.Contains("StartEnd"))
            {
                startEndNodesArray.Add(item);
            }
        }


        // Set distance between nodes with vertices in adjacency matrix
        for (int i = 0; i < nodesArray.Count; i++)
        {
            var originNodeCoordinates = GameObject.Find(nodesArray[i]).transform.position;
            for (int j = 0; j < nodesArray.Count; j++)
            {
                if (verticesArray[i, j] != 0)
                {
                    var nodeCoordinates = GameObject.Find(nodesArray[j]).transform.position;
                    // Measure the distance between these two nodes
                    float distance = Mathf.Sqrt(
                        Mathf.Pow(originNodeCoordinates.x - nodeCoordinates.x, 2) + Mathf.Pow(originNodeCoordinates.z - nodeCoordinates.z, 2));
                    //Debug.Log("Distance between " + nodesArray[i] + " and " + nodesArray[j] + " is " + distance);
                    // Replace 1 by the distance
                    verticesArray[i, j] = distance;
                }
                else
                {
                    // If there is no vertice / path between two nodes, set the value to infinity
                    verticesArray[i, j] = 1000;
                }
            }
        }

        for (int i = 0; i < allNPCs.Length; i++)
        {
            if (allNPCs[i] == npcCharacter)
            {
                thisNPC = i;
            }
        }

        Debug.Log("This NPC: " + thisNPC);

        //countDown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!npcCharacter.activeSelf)
        {
            // Finding starting node and ending point that is different from the starting one
            // I have to edit it here because the index number has to be from the array of all nodes, not just from the limited array of building nodes
            int startingNode = Random.Range(0, startEndNodesArray.Count);
            bool endingNodeFound = false;
            while (endingNodeFound == false)
            {
                endingNode = Random.Range(0, startEndNodesArray.Count);
                if (endingNode != startingNode)
                {
                    endingNodeFound = true;
                }
            }

            /*
            Till this moment, the index of starting and ending point corespond only to startEndNodesArray
            which contains only start and end nodes, but we need the indexes of our starting and ending nodes to
            correspond to the indexes of array with all nodes including the waypoints ones
            */
            string startingNodeName = startEndNodesArray[startingNode];
            startingNode = nodesArray.FindIndex(i => i == startingNodeName);
            //Debug.Log("Starting node index: " + startingNode + " and name: " + nodesArray[startingNode]);

            // Define starting node as the first approachingFrom node
            npcSharedData.approachingFrom[thisNPC] = startingNode;

            string endingNodeName = startEndNodesArray[endingNode];
            endingNode = nodesArray.FindIndex(i => i == endingNodeName);
            //Debug.Log("Ending node index: " + endingNode + " and name: " + nodesArray[endingNode]);
            //Debug.Log("Waypoints " + waypoints[endingNode]);

            //navigationSequence = Dijkstra(startingNode, endingNode);
            navigationSequence = dijkstra.DijkstraAlgorithm(startingNode, endingNode, nodesArray, verticesArray);

            /*
            The length of navigationSequence array has to be greater than two because the changeBehaviorNode
            cannot equal starting or ending Node. If the length of navigationSequence was equal certainly two,
            there would be only starting and ending node and the loop would never end
            */
            if (navigationSequence.Count > 2)
            {
                bool changeBehaviorNodeFound = false;
                int changeBehaviorNodeIndex;
                while (changeBehaviorNodeFound == false)
                {
                    changeBehaviorNodeIndex = Random.Range(0, navigationSequence.Count);
                    changeBehaviorNode = navigationSequence[changeBehaviorNodeIndex];
                    if (changeBehaviorNode != startingNodeName && changeBehaviorNode != endingNodeName)
                    {
                        changeBehaviorNodeFound = true;
                    }
                }
            }

            // Get coordinates of starting node
            var startingNodeCoordinates = GameObject.Find(navigationSequence[0]).transform.position;

            // Move NPC character to this node from which it will go to its ending node
            npcCharacter.transform.position = startingNodeCoordinates;

            // Approaching node index is number from zero to number represents the length of navigationSequence array
            approachingNodeIndex = 0;

            /*
            The speed of NPC character movement has to be set here because it can be changed
            while the behavior of NPC changes, namely in StartRunning function
            */
            approachingToNodeSpeed = 2.5f;

            /*
            In games mechanic, npcRunningState helps to recognize pumpkin script whether the npc is running or not
            so the higher amount of points can be given to players if they hit npcCharacter which is in this state
            */
            States.npcRunningState = false;

            movingStatus = true;

            // Wait two seconds before one character disappears and the new one appears
            //if (States.countDown == 100) {}

            // Display new NPC character (it is the same one in game hierarchy but new one for the player) by giving him selfActive status true
            npcCharacter.SetActive(true);

            //States.countDown += 1;
        }


        if (npcCharacter.activeSelf)
        {
            approachingNode = nodesArray.FindIndex(i => i == navigationSequence[approachingNodeIndex]);

            // Define this as approaching node if it's different from the current one stored in the array
            npcSharedData.approachingTo[thisNPC] = approachingNode;

            /*
            If NPC comes to node on which the change of behaviour is defined,
            the option is chosen based on random choosing considering probability
            and certain method is executed
            */
            if (nodesArray[approachingNode] == changeBehaviorNode)
            {
                if (Vector3.Distance(npcCharacter.transform.position, waypoints[approachingNode].transform.position) < 0.5)
                {
                    // Choose from options for changing behavior
                    randomChoiseOfBehavior = Random.Range(0, 100);
                    ChangeBehaviorChose(randomChoiseOfBehavior);
                    // Just before the approaching node changes, we can make the current approaching node the previous one
                    npcSharedData.approachingFrom[thisNPC] = approachingNode;
                    // After that, the approaching node can be changed
                    approachingNodeIndex++;
                }
            }
            else
            {
                if (Vector3.Distance(npcCharacter.transform.position, waypoints[approachingNode].transform.position) < 0.5)
                {
                    // Just before the approaching node changes, we can make the current approaching node the previous one
                    npcSharedData.approachingFrom[thisNPC] = approachingNode;
                    // After that, the approaching node can be changed
                    approachingNodeIndex++;
                }
            }

            /*
            After hitting the final node, the activity state of NPC character changes from active (true) to non-active (false).
            The NPC character disappears and the first if-statement that checks whether the character is active is executed.
            */
            if (waypoints[approachingNode] == waypoints[endingNode] && waypoints[endingNode].name == "NodeC_PrincipalOffice" && Score.hitStudents >= 2)
            {
                if (Vector3.Distance(npcCharacter.transform.position, waypoints[approachingNode].transform.position) < 0.5)
                {
                    npcCharacter.SetActive(false);
                    States.countDown = 0;
                    Score.hitStudents = Score.hitStudents - 2;
                    hitStudentsText.text = Score.hitStudents.ToString();
                }
            }
            else if (waypoints[approachingNode] == waypoints[endingNode])
            {
                if (Vector3.Distance(npcCharacter.transform.position, waypoints[approachingNode].transform.position) < 0.5)
                {
                    npcCharacter.SetActive(false);
                    States.countDown = 0;
                }
            }

            /*
            The first command heads the NPC towards the approaching node.
            The second command moves the NPC towards the approaching node.
            */
            if (movingStatus == true)
            {
                npcCharacter.transform.LookAt(waypoints[approachingNode].transform);
                npcCharacter.transform.Translate(0, 0, approachingToNodeSpeed * Time.deltaTime);
            }


            /*
            Loop through nodes other NPCs (active ones) are approaching to to find out whether there are other NPCs approaching
            the same node.

            If there are other NPCs approaching the same node, we need to know whether this NPC and other NPCs
            are approaching it from the same node or from different nodes.

            If this NPC and other NPCs are approaching the node from the same node, set status to APPROACHING_FROM_SAME_NODE
            If this NPC and other NPCs are approaching the node from different nodes, set status to APPROACHING_FROM_DIFFERENT_NODES

            If this NPC and other NPCs are approaching the node from different nodes, what is the angle in the node they are approaching to?
            The size of the angle defines whether we consider these two NPCs to approaching F2F or crossroad
            */
            previouslyApproachedNode = nodesArray.FindIndex(i => i == navigationSequence[approachingNodeIndex - 1]);
            for (int i = 0; i < allNPCs.Length; i++)
            {
                if (allNPCs[i].activeSelf == true && allNPCs[i] != npcCharacter)
                {
                    if (npcSharedData.approachingTo[i] == approachingNode && npcSharedData.approachingFrom[i] != previouslyApproachedNode)
                    {
                        approachingStatus[i] = "APPROACHING_FROM_DIFFERENT_NODES";
                    }
                    else if (npcSharedData.approachingTo[i] == approachingNode && npcSharedData.approachingFrom[i] == previouslyApproachedNode)
                    {
                        approachingStatus[i] = "APPROACHING_FROM_SAME_NODE";
                    }
                    else if (approachingNode == npcSharedData.approachingFrom[i] && previouslyApproachedNode == npcSharedData.approachingTo[i])
                    {
                        approachingStatus[i] = "APPROACHING_F2F";
                    }
                    else
                    {
                        approachingStatus[i] = "";
                    }
                }

                if (allNPCs[i].activeSelf == false && allNPCs[i] != npcCharacter)
                {
                    approachingStatus[i] = "";
                }
            }

            string approachingString = "";
            foreach (string item in approachingStatus)
            {
                approachingString = approachingString + item + ", ";
            }
            Debug.Log(approachingString);

            /*
            Change behaviour according to approachingStatus

            Loop through approachingStatuses to see statuses of active NPCs. 
            */
            /*for (int i = 0; i < allNPCs.Length; i++)
            {
                // If status is APPROACHING_F2F and the distance between this NPC and item NPC is < X
                if (approachingStatus[i] == "APPROACHING_F2F" && Vector3.Distance(allNPCs[i].transform.position, allNPCs[thisNPC].transform.position) < 10)
                {}
            }*/

        }
    }

    void ChangeBehaviorChose(int probability)
    {
        if (60 < probability && probability <= 70)
        {
            StartRunning();
        }
        else if (70 < probability && probability <= 80)
        {
            GoBack();
        }
        else if (80 < probability)
        {
            StopAndLookUp();
        }
    }

    void StartRunning()
    {
        approachingToNodeSpeed = 4.5f;
        States.npcRunningState = true;
    }

    void GoBack()
    {
        int startingNodeIndex = nodesArray.FindIndex(i => i == navigationSequence[0]);
        navigationSequence = dijkstra.DijkstraAlgorithm(approachingNode, startingNodeIndex, nodesArray, verticesArray);
        endingNode = startingNodeIndex;
        approachingNodeIndex = 0;
    }

    void StopAndLookUp()
    {
        movingStatus = false;
        //System.Threading.Thread.Sleep(2000);
        approachingToNodeSpeed = 0f;
        //int countDown = 200;

        approachingToNodeSpeed = 2.5f;
        int principalOfficeNodeIndex = 0;
        if (isPlayerHiden == false)
        {
            // Find out what node is principal office
            foreach (string item in nodesArray)
            {
                if (item.Contains("PrincipalOffice"))
                {
                    principalOfficeNodeIndex = nodesArray.FindIndex(i => i == item);
                }
            }
            navigationSequence = dijkstra.DijkstraAlgorithm(approachingNode, principalOfficeNodeIndex, nodesArray, verticesArray);
            endingNode = principalOfficeNodeIndex;
            approachingNodeIndex = 0;
        }
        movingStatus = true;
    }
}






