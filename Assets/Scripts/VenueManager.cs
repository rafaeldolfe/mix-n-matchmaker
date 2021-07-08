using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VenueManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IComparable
{
    private LotteryManager rmm;
    private AreYouSureManager ays;


    public Venue venue;
    public Image image;
    public PriorityScript priority1;
    public PriorityScript priority2;
    public PriorityScript priority3;
    public PriorityScript priority4;
    public PriorityScript priority5;
    public GameObject hoverBlocker;

    private void Awake()
    {
        rmm = FindObjectOfType<LotteryManager>();
        ays = FindObjectOfType<AreYouSureManager>();
    }
    internal void SetVenue(Venue venue)
    {
        this.venue = venue;
        image.sprite = venue.sprite;
        priority1.SetNumber(venue.priority1s.Count);
        priority2.SetNumber(venue.priority2s.Count);
        priority3.SetNumber(venue.priority3s.Count);
        priority4.SetNumber(venue.priority4s.Count);
        priority5.SetNumber(venue.priority5s.Count);
    }
    public void AreYouSure()
    {
        ays.Show(this);
    }
    public void Confirm()
    {
        rmm.StartRandomChoosing(GetBestPossibleMatch(), venue.name, venue.sprite);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ays.Show(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverBlocker.SetActive(false);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverBlocker.SetActive(true);
    }
    public (Person, Person) GetBestPossibleMatch()
    {
        Person firstPerson = GetHighestPriorityPerson();
        Person secondPerson = GetHighestPriorityPerson(firstPerson);

        return (firstPerson, secondPerson);
    }

    public Person GetHighestPriorityPerson(Person whoIsNotThisPerson = null)
    {
        Person person = null;

        if (venue.priority1s.Count != 0)
        {
            venue.priority1s = venue.priority1s.Shuffle().ToList();
            person = venue.priority1s.Find(p => p != whoIsNotThisPerson);
            if (person != null)
            {
                return person;
            }
        }
        if (venue.priority2s.Count != 0 && person == null)
        {
            venue.priority2s = venue.priority2s.Shuffle().ToList();
            person = venue.priority2s.Find(p => p != whoIsNotThisPerson);
            if (person != null)
            {
                return person;
            }
        }
        if (venue.priority3s.Count != 0 && person == null)
        {
            venue.priority3s = venue.priority3s.Shuffle().ToList();
            person = venue.priority3s.Find(p => p != whoIsNotThisPerson);
            if (person != null)
            {
                return person;
            }
        }
        if (venue.priority4s.Count != 0 && person == null)
        {
            venue.priority4s = venue.priority4s.Shuffle().ToList();
            person = venue.priority4s.Find(p => p != whoIsNotThisPerson);
            if (person != null)
            {
                return person;
            }
        }
        if (venue.priority5s.Count != 0 && person == null)
        {
            venue.priority5s = venue.priority5s.Shuffle().ToList();
            person = venue.priority5s.Find(p => p != whoIsNotThisPerson);
            if (person != null)
            {
                return person;
            }
        }

        return person;
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return -1;
        }
        if (obj.GetType() != typeof(VenueManager))
        {
            return -1;
        }

        VenueManager manager = obj as VenueManager;

        long self = venue.priority1s.Count * 100000000L + 
            venue.priority2s.Count * 1000000L + 
            venue.priority3s.Count * 10000L + 
            venue.priority4s.Count * 100L + 
            venue.priority5s.Count * 1L;

        long other = manager.venue.priority1s.Count * 100000000L +
            manager.venue.priority2s.Count * 1000000L +
            manager.venue.priority3s.Count * 10000L +
            manager.venue.priority4s.Count * 100L +
            manager.venue.priority5s.Count * 1L;

        return self.CompareTo(other);
    }

    public override string ToString()
    {
        return $"P1s: {venue.priority1s.Count}, " +
            $"P2s: {venue.priority2s.Count}, " +
            $"P3s: {venue.priority3s.Count}, " +
            $"P4s: {venue.priority4s.Count}, " +
            $"P5s: {venue.priority5s.Count}";
    }
}
