using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
  public AudioSource audioSource;
  
  
  [Header ("Music Clips")]
  [SerializeField]
  private AudioClip mainMenu;
  [SerializeField]
  private AudioClip labyrinth;
  [SerializeField]
  private AudioClip sideEvent;
  [SerializeField]
  private AudioClip showdown;
  [SerializeField]
  private AudioClip victory;

  [HideInInspector]
  public AudioClip currentSong;



	private void Start()
	{

    GameManager.Instance.StartupNewGameBegin.AddListener(playMainMenu);



    //public UnityEvent LabyrinthExploreBegin; //play labyrinth

    //public UnityEvent LabyrinthExploreEnd;

    //public UnityEvent SideEventBegin; //play side event

    //public UnityEvent SideEventEnd; //resume labyrinth




    //public UnityEvent ShowdownBegin; //play showdown

    //public UnityEvent ShowdownEnd;



    //public UnityEvent PausedBegin; //pause current song

    //public UnityEvent PausedEnd; //unpause current song




    //public UnityEvent EndScreenBegin; //play victory

    //public UnityEvent EndScreenEnd;  //play main menu???



  }

  void playAudio(AudioClip audio)
	{
    currentSong = audio;
    audioSource.clip = currentSong;
    audioSource.Play();
  }

  void playMainMenu()
	{
    playAudio(mainMenu);
	}

  void playLabyrinth()
  {
    playAudio(labyrinth);
  }




}
