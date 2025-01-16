using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Firebase_Manager
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;

    public void Init()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                GuestLogin();
                Debug.Log("Firebase 초기화 성공");
            }
            else
            {
                Debug.Log("Firebase 초기화 실패");
            }
        });
    }
}
