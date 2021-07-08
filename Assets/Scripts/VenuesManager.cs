using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VenuesManager : MonoBehaviour
{
    private Database database;

    public GameObject venuePrefab;
    
    private Dictionary<VenueName, VenueManager> venueManagers = new Dictionary<VenueName, VenueManager>();

    private void Start()
    {
        database = FindObjectOfType<Database>();

        List<Venue> venues = database.GetVenues();

        foreach (Venue venue in venues)
        {
            GameObject clone = Instantiate(venuePrefab, transform);
            VenueManager manager = clone.GetComponent<VenueManager>();
            manager.SetVenue(venue);
            venueManagers[venue.name] = manager;
        }
    }
    public void Update()
    {
        List<Venue> venues = database.GetVenues();

        foreach (Venue venue in venues)
        {
            venueManagers[venue.name].SetVenue(venue);
        }

        List<VenueManager> managers = venueManagers.Values.ToList();

        // Get descending
        managers.Sort((m1, m2) => m2.CompareTo(m1));

        for (int i = 0; i < managers.Count; i++)
        {
            VenueManager manager = managers[i];

            manager.transform.SetSiblingIndex(i);
        }
    }
}
