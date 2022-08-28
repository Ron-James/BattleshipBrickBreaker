using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBrickIndicator : MonoBehaviour
{
    public bool filled = false;
    [SerializeField] GameObject empty;
    [SerializeField] GameObject full;
    // Start is called before the first frame update
    void Start()
    {
        TurnOff();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        if (!filled)
        {
            filled = true;
            empty.SetActive(false);
            full.SetActive(true);
        }
        else
        {
            filled = false;
            empty.SetActive(true);
            full.SetActive(false);
        }
    }

    public void UpdateState()
    {
        if (filled)
        {
            empty.SetActive(false);
            full.SetActive(true);
        }
        else
        {
            empty.SetActive(true);
            full.SetActive(false);
        }
    }

    public void TurnOn()
    {
        filled = true;
        empty.SetActive(false);
        full.SetActive(true);
    }

    public void TurnOff()
    {
        filled = false;
        empty.SetActive(true);
        full.SetActive(false);
    }
}
