#pragma warning disable 0649

using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;
using UnityEngine.UI;
public class TextToSpeechGame : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/text-to-speech/api\"")]
    [SerializeField]
    private string _serviceUrl;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string _iamApikey;
    #endregion

    TextToSpeech _service;
    string _testString = "<speak version=\"1.0\"><say-as interpret-as=\"letters\">I'm sorry</say-as>. <prosody pitch=\"150Hz\">This is Text to Speech!</prosody><express-as type=\"GoodNews\">I'm sorry. This is Text to Speech!</express-as></speak>";

    private bool _synthesizeTested = false;

    //TextBox
    private string tts;
    private bool instance_created = false;

    //GameController
    public GameController gc;

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        //Runnable.Run(CreateService());
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new WatsonException("Plesae provide IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        Credentials credentials = null;

        //  Authenticate using iamApikey
        TokenOptions tokenOptions = new TokenOptions()
        {
            IamApiKey = _iamApikey
        };

        credentials = new Credentials(tokenOptions, _serviceUrl);

        //  Wait for tokendata
        while (!credentials.HasIamTokenData())
            yield return null;

        _service = new TextToSpeech(credentials);

        Runnable.Run(Examples());
    }

    private IEnumerator Examples()
    {
        //  Synthesize
        Log.Debug("TextToSpeechGame.Examples()", "Attempting synthesize.");
        _service.Voice = VoiceType.en_US_Allison;
        _service.ToSpeech(HandleToSpeechCallback, OnFail, tts, true);
        while (!_synthesizeTested)
            yield return null;

        Log.Debug("TextToSpeechGame.Examples()", "Text to Speech examples complete.");
    }

    void HandleToSpeechCallback(AudioClip clip, Dictionary<string, object> customData = null)
    {
        PlayClip(clip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            GameObject audioObject = new GameObject("AudioObject");
            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            gc.SetTTStime(source);

            Destroy(audioObject, clip.length);

            _synthesizeTested = true;

            if (gc.GetGameState() == GameController.GameState.InterviewIntroduction)
            {
                gc.SetGameState(GameController.GameState.InterviewBegin);
            }
            else
            {
                gc.SetGameState(GameController.GameState.InterviewAnswer);
            }
            instance_created = false;
        }
    }

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("TextToSpeechGame.OnFail()", "Error received: {0}", error.ToString());
    }

    public void SetTextToSpeech(string text)
    {
        if (instance_created == false)
        {
            instance_created = true;
            tts = text;
            Runnable.Run(CreateService());
        }
    }
}
