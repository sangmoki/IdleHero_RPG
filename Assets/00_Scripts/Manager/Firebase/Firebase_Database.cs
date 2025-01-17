using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class User
{
    public string userName;
    public int Stage;
}

public partial class Firebase_Manager
{
    public void WriteData()
    {
        // json 형태로 데이터를 저장
        User user = new User();

        user.userName = currentUser.UserId;
        user.Stage = Base_Manager.Data.Stage;

        // Class를 Json String으로 변환
        string json = JsonUtility.ToJson(user);

        reference.Child("Users").Child(currentUser.UserId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("데이터 작성 성공");
            }
            else
            {
                Debug.LogError("데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
    }

    public void ReadData()
    {
        reference.Child("Users").Child(currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Json 문자열을 클래스로 변환
                User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                Base_Manager.Data.Stage = user.Stage;

                // 데이터를 가져온 후 메인 로딩
                LoadingScene.instance.LoadingMain();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });
    }
}
