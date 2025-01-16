using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Firebase_Manager
{
    public void GuestLogin()
    {
        if (auth.CurrentUser != null)
        {
            Debug.Log("기기에 로그인 된 상태입니다. : " + auth.CurrentUser.UserId);
            return;
        }

        // 게스트로 로그인 시도
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("로그인 실패");
                return;
            }
            FirebaseUser user = task.Result.User;
            Debug.Log("게스트 로그인 성공 ID : " + user.UserId);
        });

    }
}
