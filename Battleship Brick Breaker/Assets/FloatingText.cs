using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public enum FloatAxis{
        x,
        y,
        z
    }
    [SerializeField] GameObject textGameObject;
    [SerializeField] TextMeshPro text;
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] Sound sound;

    [Header("Floating Parameters")]
    [SerializeField] float floatHeight = 1f;
    [SerializeField] float initialSpeed = 1f;
    [SerializeField] float maximumSpeed = 1f;
    [SerializeField] float lifeSpan = 1f;
    [SerializeField] string floatText = "+1 Ammo";
    [SerializeField] FloatAxis floatAxis = FloatAxis.z;

    [SerializeField] bool isActive = false;

    public bool IsActive { get => isActive; set => isActive = value; }

    // Start is called before the first frame update
    void Start()
    {
        sound.src = GetComponent<AudioSource>();
        isActive = false;
        textGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)){
            DeployFloatingText(new Vector3(5, 10, 0));
        }
    }

    public void DeployFloatingText(Vector3 position, float duration, float height, float iniSpeed, float maxSpeed ,string Text = "+1 Ammo"){
        transform.position = position;
        if(text != null){
            text.text = Text;
        }
        if(textUI != null){
            textUI.text = Text;
        }
        StartCoroutine(StartFloatingText(duration, height, iniSpeed, maxSpeed));
    }
    public void DeployFloatingText(Vector3 position){
        transform.position = position;
        if(text != null){
            text.text = floatText;
        }
        if(textUI != null){
            textUI.text = floatText;
        }
        StartCoroutine(StartFloatingText(lifeSpan, floatHeight, initialSpeed, maximumSpeed));
    }
    public void DeployFloatingText(Vector3 position, string message){
        transform.position = position;
        if(text != null){
            text.text = message;
        }
        if(textUI != null){
            textUI.text = message;
        }
        StartCoroutine(StartFloatingText(lifeSpan, floatHeight, initialSpeed, maximumSpeed));
    }
    IEnumerator StartFloatingText(float duration, float height, float iniSpeed, float maxSpeed){
        float time = 0;
        Vector3 defaultPos = textGameObject.transform.localPosition;
        textGameObject.SetActive(true);
        Vector3 target = textGameObject.transform.localPosition;

        switch((int) floatAxis){
            case 0:
            target.x = height;
            break;

            case 1:
            target.y = height;
            break;
            case 2:
            target.z = height;
            break;
        }

        float diff = maxSpeed - iniSpeed;
        isActive = true;
        sound.PlayOnce();
        while(true){
            time += Time.deltaTime;
            float ratio = time / duration;
            float speed = iniSpeed + animationCurve.Evaluate(ratio) * diff;
            if(time >= duration){
                textGameObject.SetActive(false);
                textGameObject.transform.localPosition = Vector3.zero;
                isActive = false;
                break;
            }
            else{
                textGameObject.transform.localPosition = Vector3.MoveTowards(textGameObject.transform.localPosition, target, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
