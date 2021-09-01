using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MatchesManager : MonoBehaviour
{
    private Database database;

    public GameObject matchPrefab;
    
    private List<MatchManager> matchManagers = new List<MatchManager>();

    private void Start()
    {
        database = FindObjectOfType<Database>();

        List<Match> matches = database.GetMatches();

        foreach (Match match in matches)
        {
            GameObject clone = Instantiate(matchPrefab, transform);
            MatchManager manager = clone.GetComponent<MatchManager>();
            manager.SetMatch(match);
            matchManagers.Add(manager);
        }
    }
    public void Update()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        List<Match> matches = database.GetMatches();

        foreach (Match match in matches)
        {
            GameObject clone = Instantiate(matchPrefab, transform);
            MatchManager manager = clone.GetComponent<MatchManager>();
            manager.SetMatch(match);
            matchManagers.Add(manager);
        }
    }
}
