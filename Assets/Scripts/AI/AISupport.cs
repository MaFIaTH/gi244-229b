using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISupport : MonoBehaviour
{
    [SerializeField] private List<GameObject> fighters = new List<GameObject>(); //fighter
    [SerializeField] private List<GameObject> builders = new List<GameObject>(); //builder
    [SerializeField] private List<GameObject> workers = new List<GameObject>(); //worker
    [SerializeField] private List<GameObject> hq = new List<GameObject>(); //headquarters
    [SerializeField] private List<GameObject> barracks = new List<GameObject>(); //barrack
    [SerializeField] private List<GameObject> houses = new List<GameObject>(); //hunter lodge
    [SerializeField] private Faction faction;

    public List<GameObject> Workers => workers;
    public List<GameObject> Builders => builders;
    public List<GameObject> Fighters => fighters;
    public List<GameObject> HQ => hq;
    public List<GameObject> Barracks => barracks;
    public List<GameObject> Houses => houses;
    public Faction Faction => faction;

    // Start is called before the first frame update
    void Awake()
    {
        faction = GetComponent<Faction>();
    }

    public void Refresh()
    {
        fighters.Clear();
        workers.Clear();
        builders.Clear();

        foreach (Unit u in faction.AliveUnits)
        {
            if (!u) continue;
            if (u.IsBuilder) //if it is a builder
                builders.Add(u.gameObject);
            
            if (u.IsWorker) //if it is a worker
                workers.Add(u.gameObject);

            if (!u.IsBuilder && !u.IsWorker) //if it is a fighter
                fighters.Add(u.gameObject);
        }
        
        hq.Clear();
        barracks.Clear();
        houses.Clear();

        foreach (Building b in faction.AliveBuildings)
        {
            if (!b) continue;
            if (b.IsHQ) //if it is a headquarters
                hq.Add(b.gameObject);
            if (b.IsBarrack) //if it is a barrack
                barracks.Add(b.gameObject);
            if (b.IsHousing) //if it is a hunter lodge
                houses.Add(b.gameObject);
        }
    }

}
