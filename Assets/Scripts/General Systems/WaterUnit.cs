using System;
using System.Collections;
using UnityEngine;

public class WaterInteractArgs : EventArgs
{
    public WaterUnit.WaterType WaterType;
    public bool Fishable;
    public FishPool FishPool;

    public WaterInteractArgs(WaterUnit.WaterType wt, bool fishable, FishPool fp)
    {
        WaterType = wt;
        Fishable = fishable;
        FishPool = fp;
    }
}

public class WaterUnit : Interactable
{
    public enum WaterType { SWAMP, SALT, FRESH };

    public event EventHandler<WaterInteractArgs> OnInteractEvent;

    [SerializeField]
    private FishPool fishPool;

    private WaterType _waterType;
    private bool _fishable = false;
    

    private void Start()
    {
        StartCoroutine(LazyLoad());
    }

    IEnumerator LazyLoad()
    {
        yield return new WaitForSeconds(1);
        if (GameManager.instance != null)
            GameManager.instance.Player.GetComponent<PlayerBehaviors>().SubscribeToWaterEvent(this);
    }

    public override void OnInteract()
    {
        if (OnInteractEvent != null)
        {
            OnInteractEvent.Invoke(this, new WaterInteractArgs(_waterType, _fishable, fishPool));
        }
    }
}
