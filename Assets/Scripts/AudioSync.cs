﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioSync : NetworkBehaviour
{
    public AudioSource tempAudioSource;

    public AudioClip[] clipArray;

    public void PlaySound(GameObject objectSource, int audioID)
    {
        if (audioID >= 0 || audioID < clipArray.Length)
        {
            CmdSyncAudioServer(objectSource, audioID); //Make a request to play a sound via the server from a particular client
        }
    }

    public void ResetSound()
    {
        tempAudioSource = null;
    }
	
    [Command]
    public void CmdSyncAudioServer(GameObject objectSource, int audioID)
    {
        RpcSyncAudioClient(objectSource, audioID); //Make a call from the server to all connected clients
    }

    [ClientRpc]
    public void RpcSyncAudioClient(GameObject objectSource, int audioID)
    {
        if (objectSource != null)
        {
            tempAudioSource = objectSource.GetComponent<AudioSource>(); //Set the audio source for all clients to the object being passed in

            if (!tempAudioSource.isPlaying)
            {
                tempAudioSource.PlayOneShot(clipArray[audioID], 1.0f); //Play our sound from the array of clips available
            }
        }

        ResetSound();
    }
}