using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Firebase_Manager
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    private DatabaseReference reference;

    public void Init()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // 익명 사용자 로그인
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;

                // 데이터베이스 초기화
                reference = FirebaseDatabase.DefaultInstance.RootReference;

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
