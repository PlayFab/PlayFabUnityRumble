using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Steamworks;

public sealed class SteamVoiceChatManager : VoiceChatManager
{
    private class DecompressedVoiceSubPack
    {
        public uint dateLength;
        public byte[] dataBuffer;

        public DecompressedVoiceSubPack(uint bufferCap)
        {
            dataBuffer = new byte[bufferCap];
        }
    }

    // voice record buffer max length for send per frame is 1k
    private const int BUFFER_SIZE = 1024;
    private const int RECORD_VOICE_PERIOD = 1;

    private class AudioResourceInfo
    {
        public AudioSource audioSource;
        public float curPlayTime;
    }

    private Dictionary<ulong, List<VoiceChatState>> userSubVoicePackageDict = new Dictionary<ulong, List<VoiceChatState>>();
    private Dictionary<ulong, AudioSource> playerAudioSourceDict = new Dictionary<ulong, AudioSource>();
    private Dictionary<ulong, AudioResourceInfo> PlayeAudioResourceInfoDict = new Dictionary<ulong, AudioResourceInfo>();

    private float curTime = 0;

    private SteamVoiceChatManager()
    { 
    }

    public static void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = new SteamVoiceChatManager();
            Instance.Initialize();
        }
    }

    public override void Initialize()
    {
        SessionNetwork.Instance.OnNetworkMessage_VoiceChat_Received += OnVoiceChatReceived;
    }

    public override void Tick(float deltaTime)
    {
        uint pcbCompressed;
        EVoiceResult result = SteamUser.GetAvailableVoice(out pcbCompressed);
        if (result == EVoiceResult.k_EVoiceResultOK)
        {
            // if voice avalaible then read voice and send to all users in lobby
            byte[] buffer = new byte[BUFFER_SIZE];
            uint bufferWritten;
            result = SteamUser.GetVoice(true, buffer, BUFFER_SIZE, out bufferWritten);
            if (result == EVoiceResult.k_EVoiceResultOK)
            {
                // if read voice buffer success, store buffer in a list
                SessionNetwork.Instance.SendMessageToAll(new VoiceChatState(buffer, (int)bufferWritten));
            }
        }

        PlayVoiceChat(deltaTime);
    }

    public override void StartVoiceRecording()
    {
        SteamUser.StartVoiceRecording();
    }

    public override void StopVoiceRecording()
    {
        SteamUser.StopVoiceRecording();
    }

    private void OnVoiceChatReceived(ulong steamId, VoiceChatState state)
    {
        if (!userSubVoicePackageDict.ContainsKey(steamId))
        {
            userSubVoicePackageDict[steamId] = new List<VoiceChatState>();
        }

        userSubVoicePackageDict[steamId].Add(state);
    }

    private void PlayVoiceChat(float deltaTime)
    {
        foreach (var item in userSubVoicePackageDict)
        {
            if (item.Value.Count > 0)
            {
                AudioResourceInfo audioSourceInfo;
                if (PlayeAudioResourceInfoDict.TryGetValue(item.Key, out audioSourceInfo) && 
                    audioSourceInfo.audioSource.isPlaying && audioSourceInfo.curPlayTime + deltaTime < audioSourceInfo.audioSource.clip.length)
                {
                    // increase current play time
                    audioSourceInfo.curPlayTime += deltaTime;
                    continue;
                }

                // set up AudioResource and play voice
                PlayNewVoiceChat(item.Key);
            }
        }
    }

    private void PlayNewVoiceChat(ulong steamId)
    {
        List<DecompressedVoiceSubPack> decompPackList = new List<DecompressedVoiceSubPack>();
        var sampleRate = SteamUser.GetVoiceOptimalSampleRate();
        byte[] allVoiceBytes;
        using (var voiceChatStream = new MemoryStream())
        {
            foreach (var item in userSubVoicePackageDict[steamId])
            {
                var dataBuffer = DecompressVoicePack(item, sampleRate);
                if (dataBuffer == null)
                {
                    continue;
                }

                voiceChatStream.Write(dataBuffer.dataBuffer, 0, (int)dataBuffer.dateLength);
            }

            allVoiceBytes = voiceChatStream.ToArray();
        }

        var clip = GenerateVoiceChatClip(allVoiceBytes, (int)sampleRate);

        if (!playerAudioSourceDict.ContainsKey(steamId))
        {

            var audioSource = AddAudioSourceToScene();
            playerAudioSourceDict.Add(steamId, audioSource);
        }

        playerAudioSourceDict[steamId].clip = clip;
        playerAudioSourceDict[steamId].Play();

        userSubVoicePackageDict[steamId].Clear();
    }

    private AudioSource AddAudioSourceToScene()
    {
        var go = GameObject.Find("AudioRecourcesObject");
        if (go == null)
        {
            go = new GameObject();
        }

        return go.AddComponent<AudioSource>();
    }

    private DecompressedVoiceSubPack DecompressVoicePack(VoiceChatState state, uint sampleRate)
    {
        var decompPack = new DecompressedVoiceSubPack(sampleRate * 2);
        uint audioLen;
        var result = SteamUser.DecompressVoice(state.buffer, (uint)state.bufferLength, decompPack.dataBuffer, (uint)decompPack.dataBuffer.Length, out audioLen, sampleRate);
        if (result == EVoiceResult.k_EVoiceResultOK)
        {
            decompPack.dateLength = audioLen;
            return decompPack;
        }

        return null;
    }

    private float ByteToFloat(byte firstByte, byte secondByte)
    {
        short s;
        if (BitConverter.IsLittleEndian)
        {
            s = (short)((secondByte << 8 | firstByte));
        }
        else
        {
            s = (short)((firstByte << 8 | secondByte));
        }

        return s / 32768.0f;
    }

    private float[] AudioByteToFloat(byte[] byteArray)
    {
        float[] soundData = new float[byteArray.Length / 2];
        for (int i = 0; i < soundData.Length; i++)
        {
            soundData[i] = ByteToFloat(byteArray[i * 2], byteArray[i * 2 + 1]);
        }

        return soundData;
    }

    private AudioClip GenerateVoiceChatClip(byte[] audioBytes, int sampleRate)
    {
        var clipData = AudioByteToFloat(audioBytes);
        var clip = AudioClip.Create("audioClip", clipData.Length, 1, sampleRate, false);

        clip.SetData(clipData, 0);

        return clip;
    }
}
