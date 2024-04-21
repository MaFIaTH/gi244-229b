using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum ResourceType
{
    Food,
    Wood,
    Gold,
    Stone
}

public class ResourceSource : MonoBehaviour
{
    [SerializeField] private string rsrcName;
    [SerializeField] private Sprite rsrcPic;
    [SerializeField] private ResourceType rsrcType;
    [SerializeField] private int quantity;
    [SerializeField] private int maxQuantity;
    [SerializeField] private GameObject selectionVisual;
    [SerializeField] private UnityEvent onRsrcQuantityChange;
    [SerializeField] private UnityEvent onInfoQuantityChange;

    public string RsrcName => rsrcName;
    public Sprite RsrcPic => rsrcPic;
    public ResourceType RsrcType => rsrcType;

    public int Quantity
    {
        get => quantity;
        set => quantity = value;
    }

    public GameObject SelectionVisual => selectionVisual; //Selection Ring
    public int MaxQuantity => maxQuantity;

    public void Start()
    {
        onRsrcQuantityChange.Invoke();
    }

    //called when a unit gathers the resource
    public void GatherResource(int amountRequest)
    {
        // make sure we don't give more than we have
        int amountToGive = amountRequest > quantity ? quantity : amountRequest;

        quantity -= amountToGive;
        onRsrcQuantityChange.Invoke();

        // if we're depleted, delete the resource
        if (quantity <= 0)
        {
            Destroy(gameObject);
        }
    }

    // toggles green selection ring around resource
    public void ToggleSelectionVisual(bool selected)
    {
        if (SelectionVisual != null)
            SelectionVisual.SetActive(selected);
    }

    void Update()
    {
        if (quantity > 0) return;
        InfoManager.Instance.ClearAllInfo();
        Destroy(gameObject);
    }
}
