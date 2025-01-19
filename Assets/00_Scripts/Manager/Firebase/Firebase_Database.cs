using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
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
        string defaultJson = JsonUtility.ToJson(Data_Manager.m_Data);

        reference.Child("Users").Child(currentUser.UserId).SetRawJsonValueAsync(defaultJson).ContinueWithOnMainThread(task =>
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

            /*  DB 저장 테스트
                // json 형태로 데이터를 저장
                User user = new User();

                user.userName = currentUser.UserId;
                user.Stage = Data_Manager.m_data.Stage;

                // Class를 Json String으로 변환
                string json = JsonUtility.ToJson(user);


            });*/
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
                Data_Manager.m_Data.Stage = user.Stage;

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
