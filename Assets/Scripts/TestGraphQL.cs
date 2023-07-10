using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestGraphQL : MonoBehaviour {
    private string POST_URL = "https://vbc2qnto3za4nare6aa2eygsfu.appsync-api.us-east-1.amazonaws.com/graphql";

    void Start() {
        StartCoroutine(PostDatas());
    }

    IEnumerator PostDatas() {        
        // string query = @"query { listTodos { items { name id } } }";
        string query = @"query {
            listTodos {
                items {
                    name 
                    id
                }
            }
        }".Replace("\n", "");
        string jsonData = "{\"query\":\"" + query + "\"}";

        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        WWWForm form = new WWWForm();
        using(UnityWebRequest request = UnityWebRequest.Post(POST_URL, form)) {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-api-key", "da2-q6xouvrjf5dr7aqpnwh4kmorqa");
            request.uploadHandler = new UploadHandlerRaw(byteData);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError) {
                Debug.LogError(request.error);
            } else {
                string response = request.downloadHandler.text;
                Debug.Log("Response:" + response);
            }
        }
    }
}
