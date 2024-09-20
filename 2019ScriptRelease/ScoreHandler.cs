using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public HatchScoring[] BlueHatchScorings;

    public HatchScoring[] RedHatchScorings;

    public CargoScoring[] BlueCargoScorings;
    public CargoScoring[] RedCargoScorings;
    private int HatchesScoredBlue;
    private int HatchesScoredRed; 
    private int CargoScoredBlue;  
    private int CargoScoredRed;
    private int totalBluePoints;
    private int totalRedPoints;
    private int climbPointsBlue;
    private int climbPointRed;
    public ClimbScoring[] blueClimbBots;
    public ClimbScoring[] redClimbBots;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

            if (GameManager.GameState != GameState.End){

        HatchesScoredBlue = 0;

        for(var i = 0; i < BlueHatchScorings.Length; i++){
            if (BlueHatchScorings[i].hasHatch) {
                HatchesScoredBlue ++;
            }
        }

        CargoScoredBlue = 0;

        for(var i = 0; i < BlueCargoScorings.Length; i++){
            if (BlueCargoScorings[i].hasCargoScored) {
                CargoScoredBlue ++;
            }
        }

            climbPointsBlue = 0;

        for (var i = 0; i < blueClimbBots.Length; i++)
            {
                if (blueClimbBots[i] != null)
                {
                    climbPointsBlue += blueClimbBots[i].scoreContribution;
                }
            }

        if (!(GameManager.GameState == GameState.Endgame))
            {
                climbPointsBlue = 0;
            }

        GameScoreTracker.BlueTeleopAmpPoints = HatchesScoredBlue * 2;
        GameScoreTracker.BlueTeleopSpeakerPoints = CargoScoredBlue * 3;
        GameScoreTracker.BlueStagePoints = climbPointsBlue;

        totalBluePoints = (HatchesScoredBlue*2) + (CargoScoredBlue*3) + climbPointsBlue;

        Score.blueScore = totalBluePoints;

        HatchesScoredRed = 0;

        for (var i = 0; i < RedHatchScorings.Length; i++) {
            if (RedHatchScorings[i].hasHatch) {
                HatchesScoredRed ++;
            }
        }

        CargoScoredRed = 0;

        for (var i = 0; i < RedCargoScorings.Length; i++) {
            if (RedCargoScorings[i].hasCargoScored) {
                CargoScoredRed ++;
            }
        }

        climbPointRed = 0;

        for (var i = 0; i < redClimbBots.Length; i++)
        {
                if (redClimbBots[i] != null)
                {
                    climbPointRed += redClimbBots[i].scoreContribution;
                }
            
        }

        if (!(GameManager.GameState == GameState.Endgame))
        {
            climbPointRed = 0;
        }


        GameScoreTracker.RedTeleopAmpPoints = HatchesScoredRed * 2;
        GameScoreTracker.RedTeleopSpeakerPoints = CargoScoredRed * 3;
        GameScoreTracker.RedStagePoints = climbPointRed;

        totalRedPoints = (HatchesScoredRed*2) + (CargoScoredRed *3) + climbPointRed;

        Score.redScore = totalRedPoints;
    }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player2") || other.gameObject.CompareTag("ClimbScore"))
        {
            if (blueClimbBots.Length > 0)
            {
                for (var i = 0; i < blueClimbBots.Length; i++)
                {
                    if (blueClimbBots[i] != other.GetComponent<ClimbScoring>() && blueClimbBots[i] == null)
                    {
                        blueClimbBots[i] = other.GetComponent<ClimbScoring>();
                        return;
                    }
                    else if (blueClimbBots[i] == other.GetComponent<ClimbScoring>())
                    {
                        return;
                    }
                }
            }
            else
            {
                {
                    blueClimbBots[0] = other.GetComponent<ClimbScoring>();
                }
            }
        }

        if (other.gameObject.CompareTag("RedPlayer") || other.gameObject.CompareTag("RedPlayer2") || other.gameObject.CompareTag("ClimbScoreRed"))
        {
            if (redClimbBots.Length > 0)
            {
                for (var i = 0; i < redClimbBots.Length; i++)
                {
                    if (redClimbBots[i] != other.GetComponent<ClimbScoring>() && redClimbBots[i] == null)
                    {
                        redClimbBots[i] = other.GetComponent<ClimbScoring>();
                        return;
                    }
                    else if (redClimbBots[i] == other.GetComponent<ClimbScoring>())
                    {
                        return;
                    }
                }
            }
            else
            {
                {
                    redClimbBots[0] = other.GetComponent<ClimbScoring>();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player2") || other.gameObject.CompareTag("ClimbScore"))
        {
            for (var i = 0; i < blueClimbBots.Length; i++)
            {
                if (blueClimbBots[i] != other.GetComponent<ClimbScoring>() && blueClimbBots[i] == null)
                {
                    blueClimbBots[i] = other.GetComponent<ClimbScoring>();
                    return;
                }
                else if (blueClimbBots[i] == other.GetComponent<ClimbScoring>())
                {
                    return;
                }
            }
        }

        if (other.gameObject.CompareTag("RedPlayer") || other.gameObject.CompareTag("RedPlayer2") || other.gameObject.CompareTag("ClimbScoreRed"))
        {
            for (var i = 0; i < redClimbBots.Length; i++)
            {
                if (redClimbBots[i] != other.GetComponent<ClimbScoring>() && redClimbBots[i] == null)
                {
                    redClimbBots[i] = other.GetComponent<ClimbScoring>();
                    return;
                }
                else if (redClimbBots[i] == other.GetComponent<ClimbScoring>())
                {
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player2") || other.gameObject.CompareTag("ClimbScore"))
        {
            for (var i = 0; i < blueClimbBots.Length; i++)
            {
                if (blueClimbBots[i] == other.GetComponent<ClimbScoring>())
                {
                    blueClimbBots[i] = null;

                }
            }
        } else if (other.gameObject.CompareTag("RedPlayer") || other.gameObject.CompareTag("RedPlayer2") || other.gameObject.CompareTag("ClimbScoreRed"))
        {
            for (var i = 0; i < redClimbBots.Length; i++)
            {
                if (redClimbBots[i] == other.GetComponent<ClimbScoring>())
                {
                    redClimbBots[i] = null;

                }
            }
        }
    }
}
