using UnityEngine;

public class SimpleGameAnalytics : MonoBehaviour
{
    public static SimpleGameAnalytics I { get; private set; }

    public int totalRounds;
    public int winRounds;
    public int loseRounds;

    public int unrankedCount;
    public int bronzeCount;
    public int silverCount;
    public int goldCount;
    public int platinumCount;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RecordGameResult(string rank)
    {
        totalRounds++;

        switch (rank)
        {
            case "Bronze":
                winRounds++;
                bronzeCount++;
                break;

            case "Silver":
                winRounds++;
                silverCount++;
                break;

            case "Gold":
                winRounds++;
                goldCount++;
                break;

            case "Platinum":
                winRounds++;
                platinumCount++;
                break;

            default:
                loseRounds++;
                unrankedCount++;
                break;
        }

        Debug.Log("=== Analytics Result ===");
        Debug.Log("Total Rounds: " + totalRounds);
        Debug.Log("Win Rounds: " + winRounds);
        Debug.Log("Lose Rounds: " + loseRounds);
        Debug.Log("Unranked: " + unrankedCount);
        Debug.Log("Bronze: " + bronzeCount);
        Debug.Log("Silver: " + silverCount);
        Debug.Log("Gold: " + goldCount);
        Debug.Log("Platinum: " + platinumCount);
    }

    public float GetWinRate()
    {
        if (totalRounds == 0) return 0f;
        return (float)winRounds / totalRounds * 100f;
    }

    public float GetLoseRate()
    {
        if (totalRounds == 0) return 0f;
        return (float)loseRounds / totalRounds * 100f;
    }
}