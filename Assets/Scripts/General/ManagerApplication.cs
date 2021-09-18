using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerApplication : MonoBehaviour {

	public static ManagerApplication Instance;

	public string BuildCode;

	public string ExtendedMenuKey;
	public CustomeToast CustomeToast;
	public GameObject LoadingScreen;

	public bool IsFullScreen;

	void Awake()
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

	void Start()
	{
		Screen.fullScreen = IsFullScreen;
#if !UNITY_EDITOR && UNITY_WEBGL
			WebGLInput.captureAllKeyboardInput = true;
#endif
	}

	public void SceneChange(string sceneName)
	{
		string[] keySplitter = sceneName.Split(new char[] {'/'});
		if (keySplitter.Length > 1)
		{
			ExtendedMenuKey = keySplitter[1];
		}

		StartCoroutine(LoadSceneAsyncronusly(keySplitter[0]));
	}

	IEnumerator LoadSceneAsyncronusly(string sceneName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		LoadingScreen.SetActive(true);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		yield return new WaitForSeconds(1);
		LoadingScreen.SetActive(false);
	}

	public void CallVirtualKeyboard()
	{
		TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false);
	}
}
