using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISupport : MonoBehaviour
{
    [SerializeField] private List<GameObject> fighters = new List<GameObject>(); //fighter
    [SerializeField] private List<GameObject> builders = new List<GameObject>(); //builder
    [SerializeField] private List<GameObject> workers = new List<GameObject>(); //worker
    [SerializeField] private Faction faction;

    public List<GameObject> Workers => workers;
    public List<GameObject> Builders => builders;
    public List<GameObject> Fighters => fighters;
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

        foreach (Unit u in faction.AliveUnits)
        {
            if (u.IsBuilder) //if it is a builder
                builders.Add(u.gameObject);
            
            if (u.IsWorker) //if it is a worker
                workers.Add(u.gameObject);

            if (!u.IsBuilder && !u.IsWorker) //if it is a fighter
                fighters.Add(u.gameObject);
        }
    }

}
