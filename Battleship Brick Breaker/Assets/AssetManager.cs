using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{

    [SerializeField] FloatingText [] floatingTexts; 

    
    // Start is called before the first frame update
    void Start()
    {
        floatingTexts = GetComponentsInChildren<FloatingText>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public FloatingText GetFloatingText(){
        foreach(FloatingText item in floatingTexts){
            if(!item.IsActive){
                return item;
            }
        }
        return null;
    }
}
