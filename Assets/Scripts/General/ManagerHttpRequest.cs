using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ManagerHttpRequest : MonoBehaviour
{
    public static ManagerHttpRequest Instance;

    [Header("Rest API Components")]
    public string APIUrl;
    public string APIKey;
    public string ContentType;

    private event Action<bool, List<PlayerData>, int, string> onCallBackGetMultipleData;

    private event Action<PlayerData> onCallBackGetSingleData;
    
    private event Action<bool, string> onCallBackCreateGame;

    private event Action<bool, string> onCallBackUploadPhoto;

    private event Action<bool> onCallBackSendScore;

    public event Action<bool, string> OnCallBackCreateGame
    {
        add
        {
            this.onCallBackCreateGame -= value;
            this.onCallBackCreateGame += value;
        }
        remove
        {
            this.onCallBackCreateGame += value;
        }
    }

    public event Action<bool, string> OnCallBackUploadPhoto
    {
        add
        {
            this.onCallBackUploadPhoto -= value;
            this.onCallBackUploadPhoto += value;
        }
        remove
        {
            this.onCallBackUploadPhoto += value;
        }
    }

    public event Action<bool> OnCallBackSendScore
    {
        add
        {
            this.onCallBackSendScore -= value;
            this.onCallBackSendScore += value;
        }
        remove
        {
            this.onCallBackSendScore += value;
        }
    }

    public event Action<PlayerData> OnCallBackGetSingleData
    {
        add
        {
            this.onCallBackGetSingleData -= value;
            this.onCallBackGetSingleData += value;
        }
        remove
        {
            this.onCallBackGetSingleData += value;
        }
    }

    public event Action<bool, List<PlayerData>, int, string> OnCallBackGetMultipleData
    {
        add
        {
            this.onCallBackGetMultipleData -= value;
            this.onCallBackGetMultipleData += value;
        }
        remove
        {
            this.onCallBackGetMultipleData += value;
        }
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Manager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void CreateNewGame()
    {
        StartCoroutine(CreateNewGame(ManagerPlayer.Instance));
    }

    public void UploadOriginalPhoto()
    {
        StartCoroutine(UploadOriginalPhoto(ManagerPlayer.Instance));
    }

    public void GetAllTimeLeaderboard(int page, LeaderboardType type)
    {
        ManagerApplication.Instance.LoadingScreen.SetActive(true);
        StartCoroutine(GetLeaderboardData(page, type));
    }

    public void GetUserPoster(string url, UnityAction<bool, Sprite, string> action)
    {
        StartCoroutine(GetPoster(url, action));
    }

    public void GetUserPoster(string url, UnityAction<bool, Sprite, string, PlayerData> action, PlayerData data)
    {
        StartCoroutine(GetPoster(url, action, data));
    }

    public void GetUserByName(string searchingKey, UnityAction<bool, List<PlayerData>, string> action, LeaderboardType type)
    {
        StartCoroutine(GetUserBy(searchingKey, action, type));
    }

    public void GetDownloadLink(PlayerData uData, UnityAction<bool, string> action)
    {
        StartCoroutine(GetUserDownloadLink(uData, action));
    }

    private IEnumerator CreateNewGame(ManagerPlayer managerPlayer)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onCallBackCreateGame != null)
            {
                onCallBackCreateGame.Invoke(false, null);
            }
            yield return null;
        }
        else
        {
            var timeout = 3;
            var curTime = 0;
            var status = false;

            while (curTime < timeout)
            {
                WWWForm form = new WWWForm();
                form.AddField("name", managerPlayer.UserName);
                form.AddField("email", managerPlayer.Email);
                form.AddField("phone", managerPlayer.TelphoneNumber);
                //form.AddBinaryData("photo", managerPlayer.NormalPhotoBytes, "OriginalPhoto.jpg");
                form.AddField("gender", managerPlayer.GenderChoosen);
                form.AddField("mobil", managerPlayer.CarChoice);

                UnityWebRequest www = UnityWebRequest.Post(APIUrl, form);
                www.SetRequestHeader("x-api-key", APIKey);

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    ManagerApplication.Instance.CustomeToast.CallToast(www.downloadHandler.text);
                    status = false;
                }
                else
                {
                    CreateGameMessage myStruct = new CreateGameMessage();
                    object boxedStruct = myStruct;
                    JsonUtility.FromJsonOverwrite(www.downloadHandler.text, boxedStruct);
                    myStruct = (CreateGameMessage) boxedStruct;

                    if (onCallBackCreateGame != null)
                    {
                        onCallBackCreateGame.Invoke(true, myStruct.data.id);
                    }
                    status = true;
                    break;
                }
                curTime++;
            }
            if (!status)
            {
                if (onCallBackCreateGame != null)
                {
                    onCallBackCreateGame.Invoke(false, null);
                }
            }
        }
    }

    private IEnumerator UploadOriginalPhoto(ManagerPlayer managerPlayer)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onCallBackUploadPhoto != null)
            {
                onCallBackUploadPhoto.Invoke(false, "No Internet Connection!");
            }
            yield return null;
        }
        else
        {
            var timeout = 3;
            var curTime = 0;
            var status = false;

            while (curTime < timeout)
            {
                WWWForm form = new WWWForm();
                form.AddBinaryData("photo", managerPlayer.NormalPhotoBytes, "OriginalPhoto.jpg");

                UnityWebRequest www = UnityWebRequest.Post(APIUrl + "/" + managerPlayer.UserID + "/upload", form);
                www.SetRequestHeader("x-api-key", APIKey);

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    ManagerApplication.Instance.CustomeToast.CallToast(www.downloadHandler.text);
                    status = false;
                }
                else
                {
                    CreateGameMessage myStruct = new CreateGameMessage();
                    object boxedStruct = myStruct;
                    JsonUtility.FromJsonOverwrite(www.downloadHandler.text, boxedStruct);
                    myStruct = (CreateGameMessage)boxedStruct;

                    if (onCallBackCreateGame != null)
                    {
                        onCallBackUploadPhoto.Invoke(true, "Success!");
                    }
                    status = true;
                    break;
                }
                curTime++;
            }

            if (!status)
            {
                if (onCallBackUploadPhoto != null)
                {
                    onCallBackUploadPhoto.Invoke(false, "Request Timeout!");
                }
            }
        }
    }

    public IEnumerator UploadScore(string userId, float score, byte[] editedPhoto)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onCallBackSendScore != null)
            {
                onCallBackSendScore.Invoke(false);
            }
            yield return null;
        }
        else
        {
            var timeOut = 3;
            var curTime = 0;
            bool uploadStatus = false;

            while (curTime < timeOut)
            {
                WWWForm form = new WWWForm();
                form.AddField("score", score.ToString());
                form.AddBinaryData("photo", editedPhoto, userId + ".jpg");

                UnityWebRequest www = UnityWebRequest.Post(APIUrl + "/" + userId, form);
                www.SetRequestHeader("x-api-key", APIKey);

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    curTime += 1;
                    uploadStatus = false;
                }
                else
                {
                    Debug.Log("Score Uploaded!");
                    uploadStatus = true;
                    break;
                }
            }

            if (uploadStatus)
            {
                if (onCallBackSendScore != null)
                {
                    onCallBackSendScore.Invoke(false);
                }
            }
        }
    }

    private IEnumerator GetLeaderboardData(int page, LeaderboardType type)
    {
        var timeout = 3;
        var curTime = 0;
        var status = false;

        List<PlayerData> playerDataList = new List<PlayerData>();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (onCallBackGetMultipleData != null)
            {
                onCallBackGetMultipleData.Invoke(false, null, 0, "No Internet Connection");
            }
            status = false;
            yield return null;
        }
        else
        {
            while (curTime < timeout)
            {
                var urlGenerator = type == LeaderboardType.ALL_TIME ? APIUrl + "/leaderboard/alltime?page=" : APIUrl + "/leaderboard?page=";
                UnityWebRequest www = UnityWebRequest.Get(urlGenerator + page);
                www.SetRequestHeader("x-api-key", APIKey);

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                    if (onCallBackGetMultipleData != null)
                    {
                        onCallBackGetMultipleData.Invoke(false, null, 0, "Request Error!");
                    }
                    status = false;
                }
                else
                {
                    var JSONdata = www.downloadHandler.text;
                    MultipleData myStruct = new MultipleData();
                    object boxedStruct = myStruct;
                    JsonUtility.FromJsonOverwrite(JSONdata, boxedStruct);
                    myStruct = (MultipleData)boxedStruct;

                    foreach (var userData in myStruct.data)
                    {
                        playerDataList.Add((new PlayerData(userData)));
                    }

                    if (onCallBackGetMultipleData != null)
                    {
                        this.onCallBackGetMultipleData.Invoke(true, playerDataList, page, "Data Loaded!");
                    }

                    status = true;
                    yield break;
                }
                curTime++;
            }

            if (!status) 
            {
                if (onCallBackGetMultipleData != null)
                {
                    onCallBackGetMultipleData.Invoke(false, null, 0, "Plaase check your internet connection!");
                }
            }
        }
    }

    private IEnumerator GetPoster(string url, UnityAction<bool, Sprite, string> action)
    {
        if (!String.IsNullOrEmpty(url))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            www.SetRequestHeader("x-api-key", APIKey);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                if (action != null)
                {
                    action.Invoke(false, null, url);
                }
            }
            else
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite newPict = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
                action.Invoke(true, newPict, "Success!");
                yield return null;
            }
        }
    }

    private IEnumerator GetPoster(string url, UnityAction<bool, Sprite, string, PlayerData> action, PlayerData data)
    {
        if (!String.IsNullOrEmpty(url))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            www.SetRequestHeader("x-api-key", APIKey);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                if (action != null)
                {
                    action.Invoke(false, null, url, data);
                }
            }
            else
            {
                var texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite newPict = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);
                action.Invoke(true, newPict, "Success!", data);
                yield return null;
            }
        }
    }

    private IEnumerator GetUserBy(string searchingKey, UnityAction<bool, List<PlayerData>, string> action, LeaderboardType type)
    {
        List<PlayerData> playerDataList = new List<PlayerData>();

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (action != null) { action.Invoke(false, null, "No internet connection!"); }
            yield return null;
        }
        else
        {
            var urlGenerator = "";
            if (type == LeaderboardType.ALL_TIME)
            {
                urlGenerator = APIUrl + "/leaderboard/alltime?q=" + searchingKey;
            }
            else
            {
                urlGenerator = APIUrl + "/leaderboard?q=" + searchingKey;
            }
            UnityWebRequest www = UnityWebRequest.Get(urlGenerator);
            www.SetRequestHeader("x-api-key", APIKey);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                if (action != null) { action.Invoke(false, null, "Request error!"); }
            }
            else
            {
                var JSONdata = www.downloadHandler.text;
                MultipleData myStruct = new MultipleData();
                object boxedStruct = myStruct;
                JsonUtility.FromJsonOverwrite(JSONdata, boxedStruct);
                myStruct = (MultipleData) boxedStruct;

                foreach (var userData in myStruct.data)
                {
                    playerDataList.Add((new PlayerData(userData)));
                }

                if (action != null) { action.Invoke(true, playerDataList, "Success!"); }
                yield break;
            }
        }
    }
    
    private IEnumerator GetUserDownloadLink(PlayerData userData, UnityAction<bool, string> action)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (action != null) { action.Invoke(false, "No internet connection!"); }
            yield return null;
        }
        else
        {
            var timeout = 3;
            var curTime = 0;
            var status = false;

            while (curTime < timeout)
            {
                WWWForm form = new WWWForm();
                form.AddField("id", userData.UID);
                form.AddField("phone", userData.Phone);

                UnityWebRequest www = UnityWebRequest.Post(APIUrl + "/download", form);
                www.SetRequestHeader("x-api-key", APIKey);

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    ManagerApplication.Instance.CustomeToast.CallToast(www.downloadHandler.text);
                    if (action != null) { action.Invoke(false, "Network Error"); }
                    status = false;
                }
                else
                {
                    RequestLink myStruct = new RequestLink();
                    object boxedStruct = myStruct;
                    JsonUtility.FromJsonOverwrite(www.downloadHandler.text, boxedStruct);
                    myStruct = (RequestLink) boxedStruct;
                    if (action != null) { action.Invoke(true, myStruct.data.download_link.ToString()); }
                    status = true;
                    break;
                }
                curTime++;
            }
            if (!status)
            {
                if (onCallBackCreateGame != null)
                {
                    onCallBackCreateGame.Invoke(false, null);
                }
            }
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string UID;
    public string Name;
    public string Email;
    public string Phone;
    public string Gender;
    public string Mobil;
    public string Score;
    public string ScoreAt;
    public string PhotoEditedURL;
    public string Rank;

    public PlayerData() { }
    public PlayerData(UserData userData) 
    {
        this.UID = userData.id;
        this.Name = userData.name;
        this.Email = userData.email;
        this.Phone = userData.phone;
        this.Gender = userData.gender;
        this.Mobil = userData.mobil;
        this.Score = userData.score;
        this.ScoreAt = userData.score_at;
        this.PhotoEditedURL = userData.photo_edited;
        this.Rank = userData.rank;
    }
}

[System.Serializable]
struct SingleData
{
    public string status;
    public string message;
    public UserData data;
}

[System.Serializable]
struct CreateGameMessage
{
    public string status;
    public string message;
    public UserId data;
}

[System.Serializable]
struct RequestLink
{
    public string status;
    public string message;
    public DownloadLink data;
    public string meta;
}

[System.Serializable]
struct MultipleData
{
    public string status;
    public string message;
    public UserData[] data;
    public MetaData meta;
}

[System.Serializable]
public class UserData
{
    public string id;
    public string name;
    public string email;
    public string phone;
    public string gender;
    public string mobil;
    public string score;
    public string score_at;
    public string photo_edited;
    public string rank;
}

[System.Serializable]
public class UserId
{
    public string id;
}

[System.Serializable]
public class DownloadLink
{
    public string download_link;
}

[System.Serializable]
public class MetaData
{
    public int totalRecords;
    public int per_page;
    public int current_page;
    public int last_page;
}

[System.Serializable]
public enum LeaderboardType
{
    ALL_TIME,
    DAILY
}

