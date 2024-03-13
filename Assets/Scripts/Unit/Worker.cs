using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField]
    private ResourceSource curResourceSource;
    [SerializeField]
    private float gatherRate = 0.5f;
    [SerializeField]
    private int gatherAmount = 1; // An amount unit can gather every "gatherRate" second(s)
    [SerializeField]
    private int amountCarry; //amount currently carried
    [SerializeField]
    private int maxCarry = 25; //max amount to carry
    [SerializeField]
    private ResourceType carryType;
    
    private float lastGatherTime;
    private Unit unit;

    public ResourceSource CurResourceSource
    {
        get => curResourceSource;
        set => curResourceSource = value;
    }

    public int AmountCarry
    {
        get => amountCarry;
        set => amountCarry = value;
    }

    public int MaxCarry
    {
        get => maxCarry;
        set => maxCarry = value;
    }

    public ResourceType CarryType
    {
        get => carryType;
        set => carryType = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<Unit>();
    }


    // Update is called once per frame
    void Update()
    {
        switch (unit.State)
        {
            case UnitState.MoveToResource:
                MoveToResourceUpdate();
                break;
            case UnitState.Gather:
                GatherUpdate();
                break;
            case UnitState.DeliverToHQ:
                DeliverToHQUpdate();
                break;
            case UnitState.StoreAtHQ:
                StoreAtHQUpdate();
                break;
        }
    }
    
    public void ToGatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;

        //if gather a new type of resource, reset amount to 0
        if (curResourceSource.RsrcType != carryType)
        {
            carryType = CurResourceSource.RsrcType;
            amountCarry = 0;
        }

        unit.SetState(UnitState.MoveToResource);

        unit.NavAgent.isStopped = false;
        unit.NavAgent.SetDestination(pos);
    }
    
    private void MoveToResourceUpdate()
    {
        CheckForResource();
        if (!(Vector3.Distance(transform.position, unit.NavAgent.destination) <= 2f)) return;
        if (!curResourceSource) return;
        unit.LookAt(curResourceSource.transform.position);
        unit.NavAgent.isStopped = true;
        unit.SetState(UnitState.Gather);
    }
    
    private void GatherUpdate()
    {
        if (!(Time.time - lastGatherTime > gatherRate)) return;
        lastGatherTime = Time.time;

        if (amountCarry < maxCarry)
        {
            if (curResourceSource)
            {
                curResourceSource.GatherResource(gatherAmount);

                carryType = curResourceSource.RsrcType;
                amountCarry += gatherAmount;
            }
            else
                CheckForResource();
        }
        else //amount is full, go back to deliver at HQ
            unit.SetState(UnitState.DeliverToHQ);
    }
    
    private void DeliverToHQUpdate()
    {
        if (Time.time - unit.LastPathUpdateTime > unit.PathUpdateRate)
        {
            unit.LastPathUpdateTime = Time.time;

            unit.NavAgent.SetDestination(unit.Faction.GetHQSpawnPos());
            unit.NavAgent.isStopped = false;
        }

        if (Vector3.Distance(transform.position, unit.Faction.GetHQSpawnPos()) <= 1f)
            unit.SetState(UnitState.StoreAtHQ);
    }
    
    private void StoreAtHQUpdate()
    {
        unit.LookAt(unit.Faction.GetHQSpawnPos());

        if (amountCarry > 0)
        {
            unit.Faction.GainResource(carryType, amountCarry);
            amountCarry = 0;
        }
        // Deliver the resource to Faction

        CheckForResource();
        //Debug.Log("Delivered");
    }
    
    private void CheckForResource()
    {
        if (curResourceSource) //that resource still exists
            ToGatherResource(curResourceSource, curResourceSource.transform.position);
        else
        {
            //try to find a new resource
            curResourceSource = unit.Faction.GetClosestResource(transform.position, carryType);

            //CheckAgain, if found a new one, go to it
            if (curResourceSource)
                ToGatherResource(curResourceSource, curResourceSource.transform.position);
            else //can't find a new one
            {
                Debug.Log($"{unit.name} can't find a new tree");
                unit.SetState(UnitState.Idle);
            }
        }
    }





}
