using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : Graph
{
    public void ChangeBehaviorChose(int probability)
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

    public void StartRunning()
    {
        approachingToNodeSpeed = 4.5f;
        States.npcRunningState = true;
    }

    public void GoBack()
    {
        int startingNodeIndex = nodesArray.FindIndex(i => i == navigationSequence[0]);
        navigationSequence = dijkstra.DijkstraAlgorithm(approachingNode, startingNodeIndex, nodesArray, verticesArray);
        endingNode = startingNodeIndex;
        approachingNodeIndex = 0;
    }

    public void StopAndLookUp()
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

