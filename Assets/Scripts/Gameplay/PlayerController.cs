using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float touchSensitivity;

    [SerializeField] private float mouseCurrentPosition;
    [SerializeField] private float mouseBoundaryRight;
    [SerializeField] private float mouseBoundaryLeft;

    [SerializeField] private Vector3[] movementPositions;
    [SerializeField] private Material[] carGraphics;
    [SerializeField] private Texture[] carTexture0;
    [SerializeField] private Texture[] carTexture1;

    private bool oneTimeMove;
    private bool oneTimeShoot;
    private bool onTheMove;
    private Vector3 targetPos;
    private bool carHasInitiated;
    private int indexPosition = 1;
    private int carCode = 0;

    void Start()
    {
        // movementPositions = movementPositions;

        this.transform.position = movementPositions[indexPosition];
    }

    void Update()
    {
        if (ManagerRacing.Instance.GameStarted)
        {
            ////see if car has been initiated
            //if (!carHasInitiated)
            //{
            //    InitiateCar();
            //}

            //navigation
            if ((Input.GetMouseButton(0)))
            {
                if (!oneTimeMove)
                {
                    mouseCurrentPosition = Input.mousePosition.x;
                    if (!oneTimeShoot)
                    {
                        mouseBoundaryLeft = (mouseCurrentPosition - touchSensitivity);
                        mouseBoundaryRight = (mouseCurrentPosition + touchSensitivity);
                        oneTimeShoot = true;
                    }
                    if (mouseCurrentPosition < mouseBoundaryLeft)
                    {
                        if (this.transform.position != movementPositions[0])
                        {
                            if (indexPosition > 0)
                            {
                                indexPosition -= 1;
                                targetPos = movementPositions[indexPosition];
                                onTheMove = true;
                            }
                        }
                        oneTimeMove = true;
                    }
                    else if (mouseCurrentPosition > mouseBoundaryRight)
                    {
                        if (this.transform.position != movementPositions[2])
                        {
                            if (indexPosition < 2)
                            {
                                indexPosition += 1;
                                targetPos = movementPositions[indexPosition];
                                onTheMove = true;
                            }
                        }
                        oneTimeMove = true;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseCurrentPosition = 0;
                oneTimeShoot = false;
                oneTimeMove = false;
            }


            if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown("left")))
            {
                if (indexPosition > 0)
                {
                    indexPosition -= 1;
                    targetPos = movementPositions[indexPosition];
                    onTheMove = true;
                   
                }
            }

            if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown("right")))
            {

                if (indexPosition < 2)
                {
                    indexPosition += 1;
                    targetPos = movementPositions[indexPosition];
                    onTheMove = true;

                }
            }
            OnPlayerMove(targetPos, indexPosition);
        }
    }

    public void InitiateCar(int code)
    {
        carCode = code;
        switch (carCode)
        {
            case 0:
                for (int i = 0; i < carGraphics.Length; i++)
                {
                    carGraphics[i].SetTexture("_MainTex", carTexture0[i]);
                }

                break;
            case 1:
                for (int i = 0; i < carGraphics.Length; i++)
                {
                    carGraphics[i].SetTexture("_MainTex", carTexture1[i]);
                }

                break;
        }

    }

    private void OnPlayerMove(Vector3 TargetPosition, int indexPos)
    {
        if (onTheMove)
        {
            Vector3 movement = Vector3.MoveTowards(this.transform.position, TargetPosition, (Time.deltaTime * (ManagerRacing.Instance.RacingSpeed * 15)));
            this.transform.position = movement;
            if (Vector3.Distance(this.transform.position, TargetPosition) == 0)
            {
                this.GetComponent<MeshRenderer>().material = carGraphics[indexPos];
                onTheMove = false;
            }
        }
    }
}
