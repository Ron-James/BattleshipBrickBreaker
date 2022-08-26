using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMotion : MonoBehaviour
{
    [SerializeField] float deltaRotationX = 5;
    [SerializeField] float deltaRotationZ = 5;
    [SerializeField] float rockPeriodX = 5f;
    [SerializeField] float rockPeriodZ = 5f;
    [SerializeField] bool isStopped;
    [SerializeField] float restoreZSpeed = 1f;
    [SerializeField] float sinkSpeed = 0.1f;
    [SerializeField] PaddleSoundBox soundBox;
    Vector3 defaultRotation;
    Vector3 defaulPosition;
    Coroutine rock;
    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.eulerAngles;
        defaulPosition = transform.position;
        isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && rock == null)
        {
            rock = StartCoroutine(RockBoat(rockPeriodX, rockPeriodZ));
        }
    }

    IEnumerator DriveUp()
    {
        while (true)
        {
            if (transform.eulerAngles.z != 0)
            {
                Quaternion target = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target, restoreZSpeed * Time.deltaTime);
            }
        }
    }

    public void SinkShip()
    {
        StartCoroutine(Sink(sinkSpeed));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator Sink(float speed)
    {
        Vector3 startPos = transform.localPosition;
        soundBox.boatSink.PlayOnce();

        Vector3 target = transform.localPosition;
        target.y -= 20;
        while (true)
        {
            if (!GetComponentInParent<HandicapController>().isHandicapped)
            {
                transform.localPosition = startPos;
                break;
            }
            else
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    IEnumerator RockBoat(float periodX, float periodZ)
    {
        float time = 0;
        float wX = (1 / periodX) * 2 * Mathf.PI;
        float wZ = (1 / periodZ) * 2 * Mathf.PI;
        while (true)
        {
            if (isStopped)
            {
                rock = null;
                break;
            }
            else
            {
                time += Time.deltaTime;
                float rotationX = defaultRotation.x + (deltaRotationX * Mathf.Sin(wX * time));
                float rotationZ = defaultRotation.z + (deltaRotationZ * Mathf.Sin(wZ * time));
                Vector3 newRotation = new Vector3(rotationX, defaultRotation.y, rotationZ);
                transform.eulerAngles = newRotation;
                yield return null;
            }
        }
    }
}
