using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : MonoBehaviour
{

    protected float checkRate = 1.0f;

    protected LayerMask unitLayerMask;
    protected LayerMask buildingLayerMask;

    protected Unit unit;
    
    void Start()
    {
        InvokeRepeating("Check", 0.0f, checkRate);
        unitLayerMask = LayerMask.GetMask("Unit");
        buildingLayerMask = LayerMask.GetMask("Building");

        if (!unit)
            unit = GetComponent<Unit>();
    }

    // checks for nearby enemies with a sphere cast
    protected Unit CheckForNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, unit.DefendRange, Vector3.up, unitLayerMask);

        GameObject closest = null;
        float closestDist = 0.0f;

        for (int x = 0; x < hits.Length; x++)
        {
            //Debug.Log("Test - " + hits[x].collider.gameObject.ToString());
            Unit target = hits[x].collider.GetComponent<Unit>();

            // skip if this is not a unit or if it is a corpse
            if (!target || target.CurHP <= 0)
                continue;

            // skip if this is myself
            if (hits[x].collider.gameObject == gameObject)
                continue;

            // skip if it is a natural unit
            if (!target.Faction)
                continue;

            // is this a team mate?
            Debug.Log(unit.Faction + gameObject.name);
            if (unit.Faction.IsMyUnit(target))
                continue;

            // if it is not the closest enemy or the distance is less than the closest distance it currently has
            if (!closest || (Vector3.Distance(transform.position, hits[x].transform.position) < closestDist))
            {
                closest = hits[x].collider.gameObject;
                closestDist = Vector3.Distance(transform.position, hits[x].transform.position);
            }
        }

        return closest ? closest.GetComponent<Unit>() : null;
    }
    
    protected void Check()
    {
        if (unit.CurHP <= 0 || unit.State == UnitState.Die) return;
        if (unit.IsWorker || unit.IsBuilder) return;
        // check if we have nearby enemies - if so, attack them
        if (unit.State == UnitState.AttackUnit || unit.State == UnitState.MoveToEnemy) return;
        Unit enemy = CheckForNearbyEnemies(); //check if we have nearby enemies 

        if (enemy) //if so, attack them
        {
            unit.ToAttackUnit(enemy);
        }
        else
        {
            Building potentialEnemyBuilding = CheckForNearbyEnemyBuildings();
            if (potentialEnemyBuilding)
            {
                unit.ToAttackBuilding(potentialEnemyBuilding);
            }
        }
    }

    // checks for nearby enemy buildings with a sphere cast
    protected Building CheckForNearbyEnemyBuildings()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, unit.DefendRange, Vector3.up, buildingLayerMask);

        GameObject closest = null;
        float closestDist = 0.0f;

        for (int x = 0; x < hits.Length; x++)
        {
            //Debug.Log("Test - " + hits[x].collider.gameObject.ToString());
            Building target = hits[x].collider.GetComponent<Building>();

            // skip if this is not a building or destroyed
            if ((!target) || (target.CurHP <= 0))
                continue;

            // skip if it is a natural/neutral Building
            if (!target.Faction)
                continue;

            // is this my building?
            if (unit.Faction.IsMyBuilding(target))
                continue;

            // if it is not the closest enemy building or the distance is less than the closest distance it currently has
            if (!closest || (Vector3.Distance(transform.position, hits[x].transform.position) < closestDist))
            {
                closest = hits[x].collider.gameObject;
                closestDist = Vector3.Distance(transform.position, hits[x].transform.position);
            }
        }

        return closest ? closest.GetComponent<Building>() : null;
    }

}

