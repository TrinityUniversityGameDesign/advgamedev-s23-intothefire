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
    
    GameManager.Instance.LabyrinthExploreBegin.AddListener(playLabyrinth);

    GameManager.Instance.SideEventBegin.AddListener(playSideEvent);

    GameManager.Instance.SideEventEnd.AddListener(playLabyrinth);

    GameManager.Instance.ShowdownBegin.AddListener(playShowdown);

    GameManager.Instance.PausedBegin.AddListener(audioSource.Pause);

    GameManager.Instance.PausedEnd.AddListener(audioSource.UnPause);

    GameManager.Instance.EndScreenBegin.AddListener(playVictory);

    GameManager.Instance.EndScreenEnd.AddListener(audioSource.Stop);
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

  void playSideEvent()
	{
    playAudio(sideEvent);
	}

  void playShowdown()
  {
    playAudio(showdown);
  }

  void playVictory()
  {
    playAudio(victory);
  }



}
