using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Door theDoor;
    bool executingBehavior = false;
    Task myCurrentTask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!executingBehavior)
            {
                executingBehavior = true;
                myCurrentTask = BuildTask_BargeDoor();

                myCurrentTask.run();
            }
        }
    }

    Task BuildTask_BargeDoor()
    {
        // create our behavior tree based on Millington pg. 344
        // building from the bottom up
        List<Task> taskList = new List<Task>();

        // barge a closed door
        taskList = new List<Task>();
        Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(bargeDoor);
        Sequence bargeClosedDoor = new Sequence(taskList);

        return bargeClosedDoor;
    }
}
