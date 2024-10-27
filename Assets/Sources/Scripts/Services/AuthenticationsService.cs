using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Services
{
    public class AuthenticationsService
    {
        public async UniTask InitServices()
        {
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Services initialized!");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public async UniTask SignInAnonymously()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 

            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
        
        public async UniTask SignOut()
        {
            try
            {
                AuthenticationService.Instance.SignOut();
                Debug.Log("Unsigned successful!");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 

            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}