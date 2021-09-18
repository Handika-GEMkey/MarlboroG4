using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomeCollider : MonoBehaviour 
{
    public Vector3 direction;
    public float MaxDistance = 10;
    public LayerMask LayerMask;

    public Animator CarAnimator;
    public AudioHandler AudioHandler;
    public Animator CameraAnimator;

    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, direction * MaxDistance, Color.green);
        if (Physics.Raycast(this.transform.position, direction, out hit, MaxDistance, LayerMask))
        {
           // Debug.Log(hit.transform.tag);
            if (hit.transform.CompareTag("Objective"))
            {
                hit.transform.gameObject.SetActive(false);
                ManagerRacing.Instance.TotalScore += 10;
                ManagerRacing.Instance.TokenScore += 1;
                AudioHandler.PlaySFXCollecting();
            }
            else if (hit.transform.CompareTag("Obstacle"))
            {
                hit.transform.gameObject.SetActive(false);
                CarAnimator.Play("Damaged", 0, 0);
                CameraAnimator.Play("HittedAnimation", 0, 0);
                ManagerRacing.Instance.TotalLife -= 1;
                AudioHandler.PlaySFXCollision();
            }
            else if (hit.transform.CompareTag("Fuel"))
            {
                hit.transform.gameObject.SetActive(false);
                ManagerRacing.Instance.AddingTime(10);
                AudioHandler.PlaySFXCollecting();
            }
        }
    }

    /*private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Objective"))
        {
            col.gameObject.SetActive(false);
            ManagerRacing.Instance.TotalScore += 100;
        }
        else if (col.CompareTag("Obstacle"))
        {
            col.gameObject.SetActive(false);
            CarAnimator.Play("Damaged", 0, 0);
            ManagerRacing.Instance.TotalLife -= 1;
        }
    }*/


}
