using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotSpawnController : MonoBehaviour
{
    private int _gamemode;
    private int _cameraMode;
    private int _blueRobotIndex;
    private int _redRobotIndex;

    public static bool isMultiplayer;
    public static bool sameAlliance;

    [SerializeField] private GameObject[] RobotPrefabs;

    [SerializeField] private GameObject[] blueCameras;
    [SerializeField] private GameObject[] redCameras;
    [SerializeField] private GameObject[] secondaryBlueCameras;
    [SerializeField] private GameObject[] secondaryRedCameras;

    [SerializeField] private GameObject cameraBorder;

    [SerializeField] private Transform blueSpawn;
    [SerializeField] private Transform secondaryBlueSpawn;
    [SerializeField] private Transform redSpawn;
    [SerializeField] private Transform secondaryRedSpawn;

    private ZoneControl _zoneCtrl;

    private void Start()
    {
        _zoneCtrl = FindFirstObjectByType<ZoneControl>();

        cameraBorder.SetActive(false);

        _gamemode = PlayerPrefs.GetInt("gamemode");
        _cameraMode = PlayerPrefs.GetInt("cameraMode");
        _redRobotIndex = PlayerPrefs.GetInt("redRobotSettings");
        _blueRobotIndex = PlayerPrefs.GetInt("blueRobotSettings");

        switch (_gamemode)
        {
            case 1:
                isMultiplayer = true;
                sameAlliance = false;
                break;
            case 2:
                sameAlliance = true;
                isMultiplayer = false;
                break;
            default:
                sameAlliance = false;
                isMultiplayer = false;
                break;
        }

        HideAll();

        if (isMultiplayer)
        {
            cameraBorder.SetActive(true);

            
            RobotPrefabs[_redRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 2";
            RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isRedRobot = true;
            RobotPrefabs[_redRobotIndex].tag = "RedPlayer";

            

            switch (_cameraMode)
            {
                case 0:
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = true;
                    break;
                case 1:
                    {
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;

                        break;
                    }
                case 2:
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = true;
                    break;
            }

            Instantiate(RobotPrefabs[_redRobotIndex], redSpawn.position, redSpawn.rotation);
            redCameras[_cameraMode + 3].SetActive(true);


            RobotPrefabs[_blueRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 1";
            RobotPrefabs[_blueRobotIndex].tag = "Player";
            RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isRedRobot = false;

            switch (_cameraMode)
            {
                case 0:
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = true;
                    break;
                case 1:
                    {
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;

                        break;
                    }
                case 2:
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = false;
                    break;
            }

            Instantiate(RobotPrefabs[_blueRobotIndex], blueSpawn.position, blueSpawn.rotation);
            blueCameras[_cameraMode + 3].SetActive(true);
        }
        else if (sameAlliance)
        {
            cameraBorder.SetActive(true);

            if (PlayerPrefs.GetString("alliance") == "red")
            {
                RobotPrefabs[_redRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 1";
                RobotPrefabs[_redRobotIndex].tag = "RedPlayer";
                RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isRedRobot = true;


                switch (_cameraMode)
                {
                    case 0:
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = !RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed;                    
                        break;
                    case 1:
                        {
                            RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                            RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = false;
                            break;
                        }
                    case 2:
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = true;
                        break;
                }

                Instantiate(RobotPrefabs[_redRobotIndex], redSpawn.position, redSpawn.rotation);
                redCameras[_cameraMode + 3].SetActive(true);


                RobotPrefabs[_blueRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 2";
                RobotPrefabs[_blueRobotIndex].tag = "RedPlayer2";
                RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isRedRobot = true;


                switch (_cameraMode)
                {
                    case 0:
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = !RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed;
                        break;
                    case 1:
                        {
                            RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                            RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = false;
                            break;
                        }
                    case 2:
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = true;
                        break;
                }


                Instantiate(RobotPrefabs[_blueRobotIndex], secondaryRedSpawn.position,
                    secondaryRedSpawn.rotation);
                secondaryRedCameras[_cameraMode].SetActive(true);

            }
            else
            {
                RobotPrefabs[_blueRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 1";
                RobotPrefabs[_blueRobotIndex].tag = "Player";
                RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isRedRobot = false;

                switch (_cameraMode)
                {
                    case 0:
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = !RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed;
                        break;
                    case 1:
                        {
                            RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                            RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = false;
                            break;
                        }
                    case 2:
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                        RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed = true;
                        break;
                }

                Instantiate(RobotPrefabs[_blueRobotIndex], blueSpawn.position, blueSpawn.rotation);
                blueCameras[_cameraMode + 3].SetActive(true);


                RobotPrefabs[_redRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 2";
                RobotPrefabs[_redRobotIndex].tag = "Player2";
                RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isRedRobot = false;

                switch (_cameraMode)
                {
                    case 0:
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = !RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed;
                        break;
                    case 1:
                        {
                            RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                            RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = false;
                            break;
                        }
                    case 2:
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                        RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = true;
                        break;
                }

                Instantiate(RobotPrefabs[_redRobotIndex], secondaryBlueSpawn.position,
                    secondaryBlueSpawn.rotation);
                secondaryBlueCameras[_cameraMode].SetActive(true);
            }
        }
        else
        {
            //Set correct robots & cameras active
            if (PlayerPrefs.GetString("alliance") == "red")
            {
                RobotPrefabs[_redRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 1";
                RobotPrefabs[_redRobotIndex].tag = "RedPlayer";
                RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isRedRobot = true;



                if (_cameraMode == 0)
                {
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed = !RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().startingReversed;
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                }
                else if (_cameraMode == 1)
                {
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                }
                else if (_cameraMode == 2)
                {
                    RobotPrefabs[_redRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                }

                Instantiate(RobotPrefabs[_redRobotIndex], redSpawn.position, redSpawn.rotation);
                redCameras[_cameraMode].SetActive(true);
            }
            else
            {
                RobotPrefabs[_blueRobotIndex].GetComponent<PlayerInput>().defaultControlScheme = "Controls 1";
                RobotPrefabs[_blueRobotIndex].tag = "Player";
                RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isRedRobot = false;


                if (_cameraMode == 0)
                {
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed =
                        !RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().startingReversed;
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                }
                else if (_cameraMode == 1)
                {
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = true;
                }
                else if (_cameraMode == 2)
                {
                    RobotPrefabs[_blueRobotIndex].GetComponent<DriveController>().isFieldCentric = false;
                }

                Instantiate(RobotPrefabs[_blueRobotIndex], blueSpawn.position, blueSpawn.rotation);
                blueCameras[_cameraMode].SetActive(true);
            }
        }
    }

    private void HideAll()
    {
        foreach (var blueCamera in blueCameras)
        {
            blueCamera.SetActive(false);
        }

        foreach (var redCamera in redCameras)
        {
            redCamera.SetActive(false);
        }

        foreach (var blueCamera in secondaryBlueCameras)
        {
            blueCamera.SetActive(false);
        }

        foreach (var redCamera in secondaryRedCameras)
        {
            redCamera.SetActive(false);
        }
    }
}