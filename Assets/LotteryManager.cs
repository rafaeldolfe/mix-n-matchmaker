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
    public GameObject matchFixingPrefab;
    public Canvas canvas;
    public AudioClip chooseCandidateSound;
    public AudioClip highlightSound;
    public AudioClip showResultsSound;
    public AudioSource source;
    public AudioSource backgroundSource;

    private readonly List<PersonListEntryScript> listEntries = new List<PersonListEntryScript>();
    private List<Person> persons;
    private bool canEndResultsScreen;

    private void Start()
    {
        database = FindObjectOfType<Database>();
    }
    private void Update()
    {
        if (Input.anyKeyDown && canEndResultsScreen)
        {
            resultManager.Hide();
            container.SetActive(false);
            canEndResultsScreen = false;
        }
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
        //int firstIndex = persons.IndexOf(person1);
        //int secondIndex = persons.IndexOf(person2);
        //Person firstPerson = null;
        //Person secondPerson = null;
        //if (firstIndex > secondIndex)
        //{
        //    firstPerson = person2;
        //    secondPerson = person1;
        //}
        //else
        //{
        //    firstPerson = person1;
        //    secondPerson = person2;
        //}

        //int secondIndexStart = persons.IndexOf(firstPerson) + 1;

        backgroundSource.volume *= 0.5f;
        int randomTrollNumber = UnityEngine.Random.Range(0, 1000);
        if (randomTrollNumber == 0)
        {
            SpawnMatchFixingMessage(5.0f);
            yield return new WaitForSeconds(5);
        }
        yield return new WaitForSeconds(1.5f);
        yield return TravelToPersonAndHighlight(person1, 0);
        var person1Entry = listEntries.Find(e => e.person == person1);
        listEntries.Remove(person1Entry);
        yield return TravelToPersonAndHighlight(person2, 0);
        yield return new WaitForSeconds(0.5f);
        database.RemovePerson(person1, venueName);
        database.RemovePerson(person2, venueName);
        database.SetPair(person1, person2, venueName);
        ShowResult(person1, person2, venueName, venueSprite);
        source.PlayOneShot(showResultsSound);
        backgroundSource.volume *= 2;
        yield return new WaitForSeconds(9.0f);
        ActivateClickListening();
    }

    private void SpawnMatchFixingMessage(float fadeTime)
    {
        GameObject clone = Instantiate(matchFixingPrefab, canvas.transform);
        LoadingStatusScript script = clone.GetComponent<LoadingStatusScript>();
        script.SetMessage($"Predefined pairing retrieval failed. Will generate random pair instead.");
        script.SetFadeTime(fadeTime);
    }

    private void ShowResult(Person firstPerson, Person secondPerson, VenueName venueName, Sprite venueSprite)
    {
        resultManager.ShowResult(firstPerson, secondPerson, venueName, venueSprite);
    }
    private void ActivateClickListening()
    {
        canEndResultsScreen = true;
        resultManager.ShowPressAnyKey();
    }

    private IEnumerator TravelToPersonAndHighlight(Person target, int startIndex)
    {
        float baseDelay = 0.075f;
        float delay = baseDelay;
        int startSlowDownPosition = UnityEngine.Random.Range(5, 20);
        float delayMultiplier = UnityEngine.Random.Range(10f, 12.5f) / startSlowDownPosition;
        int indexOfTarget = listEntries.FindIndex(startIndex, e => e.person == target);
        for (int i = startIndex; i < listEntries.Count; i++)
        {
            PersonListEntryScript script = listEntries[i];
            if (!script.IsPicked())
            {
                script.HighlightAsCandidate();
            }
            source.PlayOneShot(highlightSound);
            yield return new WaitForSeconds(delay);
            if (script.person == target)
            {
                delay = baseDelay;
                script.ChooseCandidate();
                source.PlayOneShot(chooseCandidateSound);
                yield return new WaitForSeconds(0.5f);
                yield break;
            }
            yield return new WaitForSeconds(delay);
            if (!script.IsPicked())
            {
                script.Unhighlight();
            }

            if (indexOfTarget - i < startSlowDownPosition && indexOfTarget - i > 0)
            {
                delay *= 1.0f + UnityEngine.Random.Range(0.15f, 0.25f) * delayMultiplier;
            }
        }
    }
}
