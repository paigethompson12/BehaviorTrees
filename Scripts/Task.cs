using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;  
using System.Threading; 
using UnityEngine;

public abstract class Task
{
    public abstract void run();
    protected static Mutex mutex = new Mutex();  
    public bool succeeded;
    protected int eventId;
    private static int nextEventID = 1; // non-zero seems best
    public static int GetEventID() { return nextEventID++;}
    const string EVENT_NAME_PREFIX = "FinishedTask";
    public Task()
    {
        eventId = GetEventID();
    }
}

public class OpenDoor : Task
{
    Door mDoor;

    public OpenDoor(Door someDoor)
    {
        mDoor = someDoor;
    }

    public override void run()
    {
        mutex.WaitOne();
        succeeded = mDoor.Open();
        mutex.ReleaseMutex();
    }
}

public class BargeDoor : Task
{
    Rigidbody mDoor;

    public BargeDoor(Rigidbody someDoor)
    {
        mDoor = someDoor;
    }

    public override void run()
    {
        mutex.WaitOne();
        mDoor.AddForce(-10f, 0, 0, ForceMode.VelocityChange);
        succeeded = true;
        mutex.ReleaseMutex();
    }
}


public class Wait : Task
{
    float mTimeToWait;

    public Wait(float time)
    {
        mTimeToWait = time;
    }

    public override void run()
    {
        mutex.WaitOne();
        succeeded = true;
        mutex.ReleaseMutex();
    }
}

public class MoveKinematicToObject : Task
{
    Arriver mMover;
    GameObject mTarget;

    public MoveKinematicToObject(Kinematic mover, GameObject target)
    {
        mMover = mover as Arriver;
        mTarget = target;
    }

    public override void run()
    {
        mutex.WaitOne();
        //Debug.Log("Moving to target position: " + mTarget);
        mMover.OnArrived += MoverArrived;
        mMover.myTarget = mTarget;
        mutex.ReleaseMutex();
    }

    public void MoverArrived()
    {
        //Debug.Log("arrived at " + mTarget);
        mMover.OnArrived -= MoverArrived;
        succeeded = true;
    }
}

