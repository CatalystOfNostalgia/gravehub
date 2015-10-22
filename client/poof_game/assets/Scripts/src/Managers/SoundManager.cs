using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public Dictionary<string, AudioSource> playDict{ get; set;}
	public AudioSource[] playlist { get; set;}
	public bool[] preferredPlaylist { get; set; }
	public AudioSource currentSong { get; set;}
	bool currentSongPlayed;
	bool isPlayingSpecialRequestSong;
	bool allSongsDisabled;
	//this volume field gets changed by a slider in the scene
	//should we create an explicit script to do so or is it fine to let generic unity slider to change it?
	public float musicVolume { get; set; } 

	int index;

	public AudioSource[] getAvailableMusic(){
		return this.GetComponentsInChildren<AudioSource> ();
	}

	public void playSong(string songName){
		isPlayingSpecialRequestSong = true;
		AudioSource song;
		if (playDict.TryGetValue (songName, out song)) {
			Debug.Log("SoundManager: now playing " + songName);
			stopSong ();
			currentSong = song;
			currentSong.Play ();
			currentSongPlayed = true;
		} else {
			Debug.Log ("SoundManager: The key " + songName + " was not found in the dictionary");
		}
	}

	public void stopSong(){
		if (currentSong == null) {
			return;
		}
		currentSong.Stop ();
	}

	public void nextSong(){
		stopSong (); //might be redundant
		for (int i = 0; i<playlist.Length; i++) {
			currentSongPlayed = false;
			index++;
			if (preferredPlaylist[index % playlist.Length]){
				currentSong = playlist[index % playlist.Length];
				return;
			}
		}
		//this means all of the songs are disabled
		allSongsDisabled = true;
		Debug.Log ("SoundManager: All music are disabled");
	}

	public void previousSong(){
		stopSong (); //might be redundant
		for (int i = 0; i<playlist.Length; i++) {
			currentSongPlayed = false;
			index--;
			if (preferredPlaylist[index % playlist.Length]){
				currentSong = playlist[index % playlist.Length];
				return;
			}
		}
		//this means all of the songs are disabled
		allSongsDisabled = true;
		Debug.Log ("SoundManager: All music are disabled");
	}

	public void setPreferredPlaylist (string song){
		//maybe I could have referenced the toggle to here and avoid the search
		int i = findIndex (song);
		if (i == -1) {
			return;
		}
		if (allSongsDisabled && !preferredPlaylist [i]) {
			allSongsDisabled = false;
		}
		preferredPlaylist [i] = !preferredPlaylist [i];
	}

	private int findIndex (string songName){
		for (int i = 0; i<playlist.Length; i++){
			if (playlist[i].name.Equals(songName)){
				return i;
			}
		}
		return -1;
	}

	// Use this for initialization
	void Start () {
		index = 0;

		//in the future, load user's music preference
		playlist = getAvailableMusic();
		preferredPlaylist = new bool[playlist.Length];
		for (int i = 0; i<preferredPlaylist.Length; i++) {
			preferredPlaylist[i] = true;
		}
		playDict = new Dictionary<string, AudioSource> ();
		foreach (AudioSource music in playlist) {
			playDict.Add(music.name, music);
			Debug.Log("SoundManager: Added " + music.name + " to the dictionary");
		}
		musicVolume = 100f;

		currentSong = playlist [index];
		currentSongPlayed = false;
		isPlayingSpecialRequestSong = false;
		allSongsDisabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (allSongsDisabled) {
			currentSong = null;
			return;
		}
		if (currentSong == null) {
			return;
		}
		currentSong.volume = musicVolume;
		if (!currentSong.isPlaying) {
			//didn't start the song yet
			if (!currentSongPlayed){
				currentSong.Play();
				currentSongPlayed = true;
			}
			else {//finished the song
				nextSong();
			}
		}
	}
}
