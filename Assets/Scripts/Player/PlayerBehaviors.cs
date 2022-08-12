using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviors : MonoBehaviour
{
    private float _minCatch, _maxCatch, _currentCatchX;
    private float _meterMin, _meterMax;
    private FishPool currentFishPool;

    private bool fishing = false;
    private bool _interact = false;
    private List<RaycastHit2D> _interactCollisions = new List<RaycastHit2D>();
    private PlayerAnimation _playerAnimation;
    private PlayerInventory _playerInventory;
    private PlayerMovement _playerMovement;
    private Rigidbody2D rb;

    private void Start()
    {
        StartCoroutine(LazyLoad());
    }

    IEnumerator LazyLoad()
    {
        yield return new WaitForSeconds(1);
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerMovement = GetComponent<PlayerMovement>();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_playerMovement.Busy) _interact = true;

        if (fishing)
        {
            SlideCatchValue();
            if(Input.GetKeyDown(KeyCode.Space))
            {
                fishing = false;
                //print($"Min:{_minCatch}, Stopped:{_currentCatchX}, Max:{_maxCatch}");
                if (ValidateCatch())
                    CatchFish();
                StopFishing();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_interact)
        {
            _interact = false;
            TryInteract(_playerMovement.LastDir);
        }
    }

    public void TryInteract(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            _interactCollisions,
            1);

        foreach (RaycastHit2D hit in _interactCollisions)
        {
            if (hit.collider.gameObject.GetComponent<IInteractable>() != null)
            {
                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

                interactable.OnInteract();
            }
        }
    }

    public void SubscribeToWaterEvent(WaterUnit waterUnit)
    {
        waterUnit.OnInteractEvent += WaterInteractEvent;
    }

    public void WaterInteractEvent(object sender, WaterInteractArgs e)
    {
        _playerMovement.LockMovement();
        fishing = true;
        Fishing(e);
    }

    public void Fishing(WaterInteractArgs args)
    {
        currentFishPool = args.FishPool;

        GameManager.instance.UIManager.EnableFishingUI();

        var meterRect = GameManager.instance.UIManager.GetMeterInfo();
        var catchRect = GameManager.instance.UIManager.GetCatchInfo();

        _minCatch = catchRect.transform.localPosition.x - catchRect.rect.width / 2;
        _maxCatch = catchRect.transform.localPosition.x + catchRect.rect.width / 2;
        _meterMin = meterRect.rect.xMin;
        _meterMax = meterRect.rect.xMax;
    }

    public void StopFishing()
    {
        currentFishPool = null;
        _meterMin = 0;
        _meterMax = 0;
        _minCatch = 0;
        _maxCatch = 0;
        _currentCatchX = 0;

        GameManager.instance.UIManager.DisableFishingUI();

        _playerMovement.UnlockMovement();
    }

    public void SlideCatchValue()
    {
        var value = Mathf.PingPong(Time.time, 1);
        //Debug.Log(value);

        var width = _meterMax - _meterMin;

        _currentCatchX = _meterMin + (width * value);
       // print(_currentCatchX);

        GameManager.instance.UIManager.FishingSlider(value);
    }

    public bool ValidateCatch()
    {
        if(_currentCatchX >= _minCatch && _currentCatchX <= _maxCatch)
        {
            return true;
        }
        print("No fish");
        return false;
    }

    public void CatchFish()
    {
        print("Caught fish");

        var randomFish = currentFishPool.fish[Random.Range(0, currentFishPool.fish.Count)];

        _playerInventory.AddItem(randomFish);
    }
}
