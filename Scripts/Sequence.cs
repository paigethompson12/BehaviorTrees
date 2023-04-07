using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
    List<Task> children;
    Task task;
    int taskIndex = 0;

    public Sequence(List<Task> taskList)
    {
        children = taskList;
    }

    // Sequence wants all tasks to succeed
    // try all tasks in order
    // stop and return false on the first task that fails
    // return true if all tasks succeed
    public override void run()
    {
        //Debug.Log("sequence running child task #" + taskIndex);
        task = children[taskIndex];
        //mutex lock
        task.run();
        //mutex unlock
    }

}
