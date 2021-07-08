using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
public class Database : SerializedMonoBehaviour
{
    public TMP_InputField inputField;
    public List<VenueScriptableObject> venuesToInitiate;
    public Dictionary<string, VenueName> abbreviationToVenueName = new Dictionary<string, VenueName>();
    public Dictionary<VenueName, string> venueNameToAbbreviation = new Dictionary<VenueName, string>();
    public GameObject successPrefab;
    public GameObject failurePrefab;
    public Canvas canvas;

    private List<Person> persons = new List<Person>();
    private List<Person> removedPersons = new List<Person>();
    private List<Venue> venues = new List<Venue>();
    private void Awake()
    {
        foreach (VenueScriptableObject item in venuesToInitiate)
        {
            venues.Add(new Venue { name = item.venueName, sprite = item.image });
        }
    }
    internal List<Venue> GetVenues()
    {
        return venues;
    }
    internal List<Person> GetPeople()
    {
        return persons;
    }
    public void CopyPersonsToClipBoard()
    {
        string totalString = "";

        foreach (Person person in persons)
        {
            string personString = $"{person.name}," +
                $"{person.priority1.Aggregate("", (prev, name) => prev + venueNameToAbbreviation[name])}," +
                $"{(person.priority2 != VenueName.None ? venueNameToAbbreviation[person.priority2] : "")}," +
                $"{(person.priority3 != VenueName.None ? venueNameToAbbreviation[person.priority3] : "")}," +
                $"{(person.priority4 != VenueName.None ? venueNameToAbbreviation[person.priority4] : "")}," +
                $"{(person.priority5 != VenueName.None ? venueNameToAbbreviation[person.priority5] : "")};\n";

            totalString += personString;
        }
        foreach (Person person in removedPersons)
        {
            string personString = $"{person.name}," +
                $"{person.priority1.Aggregate("", (prev, name) => prev + venueNameToAbbreviation[name])}," +
                $"{(person.priority2 != VenueName.None ? venueNameToAbbreviation[person.priority2] : "")}," +
                $"{(person.priority3 != VenueName.None ? venueNameToAbbreviation[person.priority3] : "")}," +
                $"{(person.priority4 != VenueName.None ? venueNameToAbbreviation[person.priority4] : "")}," +
                $"{(person.priority5 != VenueName.None ? venueNameToAbbreviation[person.priority5] : "")};\n";

            totalString += personString;
        }

        GUIUtility.systemCopyBuffer = totalString;
    }
    public void LoadData()
    {
        try
        {
            string data = inputField.text;

            data = Regex.Replace(data, @"\s+", string.Empty);

            string[] dataEntries = data.Split(';').Where(e => e.Contains(",")).ToArray();

            List<Person> persons = new List<Person>();

            if (dataEntries.Length == 0)
            {
                throw new Exception($"Cannot load data with 0 people");
            }

            for (int i = 0; i < dataEntries.Length; i++)
            {
                Person person = new Person();
                string[] personData = dataEntries[i].Split(',');

                if (personData.Length < 6)
                {
                    throw new Exception($"Row nr {i} only had {personData.Length} comma-separated elements, needs to have exactly {6}");
                }
                if (personData.Length > 6)
                {
                    throw new Exception($"Row nr {i} had too many ({personData.Length}) comma-separated elements, needs to have exactly {6}");
                }

                person.name = personData[0];
                if (personData[1].Length > 4)
                {
                    throw new Exception($"Priority {1} for row nr {i} had more than {4} abbreviations");
                }
                else
                {
                    foreach (char abbreviation in personData[1])
                    {
                        string availableMatches = "";
                        abbreviationToVenueName.Keys.ToList().ForEach(c => availableMatches += $"{c},");
                        if (!abbreviationToVenueName.ContainsKey(abbreviation.ToString()))
                        {
                            throw new Exception($"Priority {1} for row nr {i} had an invalid abbreviation {abbreviation}, could not find match. These are all the supported abbreviations: {availableMatches}");
                        }
                    }
                }
                List<VenueName> priority1s = personData[1].Select(c => abbreviationToVenueName[c.ToString()]).ToList();
                person.priority1 = priority1s;
                if (personData[2] != "")
                {
                    person.priority2 = abbreviationToVenueName[personData[2]];
                }
                if (personData[3] != "")
                {
                    if (personData[3].Length != 1)
                    {
                        throw new Exception($"Priority {3} for row nr {i} had an abbreviation that was too long, it was {personData[3].Length} but should be {1}");
                    }
                    person.priority3 = abbreviationToVenueName[personData[3]];
                }
                if (personData[4] != "")
                {
                    if (personData[4].Length != 1)
                    {
                        throw new Exception($"Priority {4} for row nr {i} had an abbreviation that was too long, it was {personData[4].Length} but should be {1}");
                    }
                    person.priority4 = abbreviationToVenueName[personData[4]];
                }
                if (personData[5] != "")
                {
                    if (personData[5].Length != 1)
                    {
                        throw new Exception($"Priority {5} for row nr {i} had an abbreviation that was too long, it was {personData[5].Length} but should be {1}");
                    }
                    person.priority5 = abbreviationToVenueName[personData[5]];
                }

                persons.Add(person);
            }

            this.persons = persons;
            removedPersons.Clear();

            UpdateVenues();

            GameObject clone = Instantiate(successPrefab, canvas.transform);
            LoadingStatusScript script = clone.GetComponent<LoadingStatusScript>();
            script.SetMessage($"Found {persons.Count} {(persons.Count > 1 ? "people" : "person")}");
            script.SetFadeTime(2.0f);
            inputField.text = "";
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            GameObject clone = Instantiate(failurePrefab, canvas.transform);
            LoadingStatusScript script = clone.GetComponent<LoadingStatusScript>();
            script.SetMessage($"Failed to read data:\n {e.Message}");
            script.SetFadeTime(10.0f);
            inputField.text = "";
        }
    }
    internal void RemovePerson(Person person, VenueName venueName)
    {
        if (person.priority1.Contains(venueName))
        {
            person.priority1.Remove(venueName);
        }
        if (person.priority2 == venueName)
        {
            person.priority2 = VenueName.None;
        }
        if (person.priority3 == venueName)
        {
            person.priority3 = VenueName.None;
        }
        if (person.priority4 == venueName)
        {
            person.priority4 = VenueName.None;
        }
        if (person.priority5 == venueName)
        {
            person.priority5 = VenueName.None;
        }
        persons.Remove(person);
        removedPersons.Add(person);
        UpdateVenues();
    }
    private void UpdateVenues()
    {
        Dictionary<VenueName, List<Person>> priority1s = GetPriorityDict();
        Dictionary<VenueName, List<Person>> priority2s = GetPriorityDict();
        Dictionary<VenueName, List<Person>> priority3s = GetPriorityDict();
        Dictionary<VenueName, List<Person>> priority4s = GetPriorityDict();
        Dictionary<VenueName, List<Person>> priority5s = GetPriorityDict();
        foreach (Person person in persons)
        {
            foreach (VenueName venueName in person.priority1)
            {
                priority1s[venueName].Add(person);
            }
            priority2s[person.priority2].Add(person);
            priority3s[person.priority3].Add(person);
            priority4s[person.priority4].Add(person);
            priority5s[person.priority5].Add(person);
        }
        foreach (Venue venue in venues)
        {
            venue.priority1s = priority1s[venue.name];
            venue.priority2s = priority2s[venue.name];
            venue.priority3s = priority3s[venue.name];
            venue.priority4s = priority4s[venue.name];
            venue.priority5s = priority5s[venue.name];
        }
    }
    private Dictionary<VenueName, List<Person>> GetPriorityDict()
    {
        Dictionary<VenueName, List<Person>> prioritys = new Dictionary<VenueName, List<Person>>();
        foreach (VenueName e in Enum.GetValues(typeof(VenueName)))
        {
            prioritys[e] = new List<Person>();
        }
        return prioritys;
    }
}
