## Unity 기능 구현

### 1. HTTP 통신을 위한 UnityWebRequest 활용
Unity의 `UnityWebRequest` 클래스를 사용하여 서버와의 HTTP 통신을 처리합니다. 이를 통해 GET, POST 등의 요청을 보낼 수 있으며, 비동기 방식으로 데이터를 처리하여 사용자 경험을 최적화할 수 있습니다.

```csharp
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

IEnumerator GetRequest(string uri)
{
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            Debug.Log("Received: " + webRequest.downloadHandler.text);
        }
    }
}
```

### 2. 사용자 입력에 따른 카메라 움직임 구현
사용자의 입력에 따라 카메라가 움직이도록 `UI_Canvas`와 연동하여 카메라 애니메이터 기능을 구현합니다. 이를 통해 직관적인 UI로 카메라의 시점을 조정할 수 있습니다.

```csharp
public class CameraController : MonoBehaviour
{
    public Animator cameraAnimator;

    public void MoveCamera(string direction)
    {
        cameraAnimator.SetTrigger(direction);
    }
}
```

### 3. JSON 형식의 API 데이터 처리
`Newtonsoft.Json` 라이브러리의 `JObject`와 `JToken`을 활용하여 JSON 형식의 데이터를 파싱합니다. 이를 통해 API로부터 받은 데이터를 효율적으로 처리할 수 있습니다.

```csharp
using Newtonsoft.Json.Linq;

public void ParseJsonData(string jsonString)
{
    JObject jsonObject = JObject.Parse(jsonString);
    string data = jsonObject["key"].ToString();
    Debug.Log("Parsed Data: " + data);
}
```

### 4. 주기적인 API 데이터 요청
코루틴(Coroutine)을 활용하여 주기적으로 API 데이터를 요청합니다. 일정 시간 간격으로 서버에 데이터를 요청하여 실시간 정보를 업데이트할 수 있습니다.

```csharp
IEnumerator FetchDataPeriodically(string uri, float delay)
{
    while (true)
    {
        StartCoroutine(GetRequest(uri));
        yield return new WaitForSeconds(delay);
    }
}
```

