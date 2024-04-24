using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI Instance;

    [SerializeField] private GameObject selectionMarker;

    [SerializeField] private TextMeshProUGUI unitCountText;
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI stoneText;
    
    [SerializeField] private RectTransform selectionBox;
    
    [SerializeField] private TextMeshProUGUI gameTimerText;
    private Canvas canvas;
    public Canvas Canvas => canvas;
  

    public GameObject SelectionMarker => selectionMarker;
    public RectTransform SelectionBox => selectionBox;

    private void Awake()
    {
        Instance = this;
        canvas = GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateAllResource(Faction faction)
    {
        unitCountText.text = $"{faction.AliveUnits.Count}/{faction.UnitLimit}";
        foodText.text = faction.Food.ToString();
        woodText.text = faction.Wood.ToString();
        goldText.text = faction.Gold.ToString();
        stoneText.text = faction.Stone.ToString();
    }
    
    public Vector3 ScalePosition(Vector3 pos)
    {
        Vector3 newPos;

        newPos = new Vector3(pos.x * canvas.transform.localScale.x
            , pos.y * canvas.transform.localScale.y
            , pos.z * canvas.transform.localScale.z);

        return newPos;
    }
    
    public void UpdateGameTimer(float time)
    {
        gameTimerText.text = TimeSpan.FromSeconds(time).ToString("mm':'ss");
    }

}
