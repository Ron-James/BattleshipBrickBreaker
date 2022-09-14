using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleSliderControl : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] bool isStopped = false;
    [SerializeField] float maxZ = 19.5f;
    [SerializeField] float speed = 5;
    [SerializeField] Transform backBoard;
    float backboardOffset;
    PaddleController paddleController;
    float currentSliderValue;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        paddleController = GetComponent<PaddleController>();
        backboardOffset = backBoard.position.z;
        currentSliderValue = slider.value;
        if(paddleController.controlScheme == PaddleController.ControlScheme.slider){
            //rb.isKinematic = true;
        }
        else{
            //rb.isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(paddleController.controlScheme == PaddleController.ControlScheme.slider){
            if(!isStopped){
                Vector3 position = transform.position;
                position.z = (slider.value * maxZ) + backboardOffset;
                transform.position = Vector3.MoveTowards(transform.position, position, speed);
                currentSliderValue = slider.value;
            }
        }
        
    }
}
