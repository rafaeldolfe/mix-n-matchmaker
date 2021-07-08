using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryManager : MonoBehaviour
{
    private Database database;

    public GameObject container;
    public GameObject content;
    public GameObject personListEntryPrefab;
    public ResultManager resultManager;
    public GameObject matchFailedPrefab;
    public Canvas canvas;
    public AudioClip chooseCandidateSound;
    public AudioClip highlightSound;
    public AudioClip showResultsSound;
    public AudioSource source;
    public AudioSource backgroundSource;

    private readonly List<PersonListEntryScript> listEntries = new List<PersonListEntryScript>();
    private List<Person> persons;

    private void Start()
    {
        database = FindObjectOfType<Database>();
    }

    public void StartRandomChoosing((Person, Person) pair, VenueName venueName, Sprite venueSprite)
    {
        persons = database.GetPeople();

        List<Person> shuffledPeople = new List<Person>(persons);
        shuffledPeople.Remove(pair.Item1);
        shuffledPeople.Remove(pair.Item2);
        shuffledPeople.Shuffle();

        if (pair.Item1 == null)
        {
            if (shuffledPeople.Count != 0)
            {
                pair.Item1 = shuffledPeople[0];
                shuffledPeople.Remove(pair.Item1);
            }
        }
        if (pair.Item2 == null)
        {
            if (shuffledPeople.Count != 0)
            {
                pair.Item2 = shuffledPeople[0];
                shuffledPeople.Remove(pair.Item2);
            }
        }

        if (pair.Item1 == null || pair.Item2 == null)
        {
            GameObject clone = Instantiate(matchFailedPrefab, canvas.transform);
            LoadingStatusScript script = clone.GetComponent<LoadingStatusScript>();
            script.SetMessage($"Not enough people to match.");
            script.SetFadeTime(5.0f);
            return;
        }

        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }

        listEntries.Clear();


        if (persons.Count < 2)
        {
            return;
        }

        container.SetActive(true);



        foreach (var person in persons)
        {
            GameObject clone = Instantiate(personListEntryPrefab, content.transform);
            PersonListEntryScript script = clone.GetComponent<PersonListEntryScript>();
            script.SetPerson(person);
            listEntries.Add(script);
        }

        StartCoroutine(PickTwo(pair, venueName, venueSprite));
    }

    private IEnumerator PickTwo((Person, Person) pair, VenueName venueName, Sprite venueSprite)
    {
        Person person1 = pair.Item1;
        Person person2 = pair.Item2;
        int firstIndex = persons.IndexOf(person1);
        int secondIndex = persons.IndexOf(person2);
        Person firstPerson = null;
        Person secondPerson = null;
        if (firstIndex > secondIndex)
        {
            firstPerson = person2;
            secondPerson = person1;
        }
        else
        {
            firstPerson = person1;
            secondPerson = person2;
        }

        int secondIndexStart = persons.IndexOf(firstPerson) + 1;

        backgroundSource.volume *= 0.5f;
        yield return new WaitForSeconds(1.5f);
        yield return TravelToPersonAndHighlight(firstPerson, 0);
        yield return TravelToPersonAndHighlight(secondPerson, secondIndexStart);
        yield return new WaitForSeconds(0.5f);
        database.RemovePerson(firstPerson, venueName);
        database.RemovePerson(secondPerson, venueName);
        ShowResult(firstPerson, secondPerson, venueName, venueSprite);
        source.PlayOneShot(showResultsSound);
        backgroundSource.volume *= 2;
        yield return new WaitForSeconds(9.0f);
        HideEverything();
    }

    private void ShowResult(Person firstPerson, Person secondPerson, VenueName venueName, Sprite venueSprite)
    {
        resultManager.ShowResult(firstPerson, secondPerson, venueName, venueSprite);
    }
    private void HideEverything()
    {
        resultManager.Hide();
        container.SetActive(false);
    }

    private IEnumerator TravelToPersonAndHighlight(Person target, int startIndex)
    {
        for (int i = startIndex; i < listEntries.Count; i++)
        {
            PersonListEntryScript script = listEntries[i];
            script.HighlightAsCandidate();
            source.PlayOneShot(highlightSound);
            yield return new WaitForSeconds(0.075f);
            if (script.person == target)
            {
                script.ChooseCandidate();
                source.PlayOneShot(chooseCandidateSound);
                yield return new WaitForSeconds(0.5f);
                yield break;
            }
            yield return new WaitForSeconds(0.075f);
            script.Unhighlight();
        }
    }
}
