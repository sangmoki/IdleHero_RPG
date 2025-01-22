using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Newtonsoft.Json;

public class User
{
    public string userName;
    public int Stage;
}

public partial class Firebase_Manager
{
    public void WriteData()
    {
        #region DEFAULT_DATA
        Data data = new Data();

        if (Data_Manager.m_Data != null)
        {
            data = Data_Manager.m_Data;
        }

        string Default_Json = JsonUtility.ToJson(data);

        Debug.Log("Default_Json : " + Default_Json);
        // 유저 기본 데이터 저장
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(Default_Json).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("기본 데이터 쓰기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

        #region CHARACTER_DATA
        // Newtonsoft.Json을 사용하여 클래스, 구조체를 Json으로 변환
        // JsonConvert.SerializeObject(data);를 통하여 Dictionary를 Json으로 변환
        string Character_Json = JsonConvert.SerializeObject(Base_Manager.Data.Character_Holder);

        Debug.Log("Character_Json : " + Character_Json);
        // 캐릭터 정보 저장
        reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_Json).ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
            {
                Debug.LogError("영웅 정보 작성 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

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
        #region DEFAULT_DATA
        reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                var defaultData = JsonUtility.FromJson<Data>(snapshot.GetRawJsonValue());
                Data data = new Data();

                if (defaultData != null)
                {
                    data = defaultData;
                }

                Data_Manager.m_Data = data;
                // 데이터를 가져온 후 메인 로딩
                LoadingScene.instance.LoadingMain();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

        #region CHARACTER_DATA
        reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());
                Base_Manager.Data.Character_Holder = data;
                
                Base_Manager.Data.Init();
            }
            else
            {
                Debug.LogError("데이터 읽기 실패 : " + task.Exception.ToString());
            }
        });
        #endregion

    }
}
