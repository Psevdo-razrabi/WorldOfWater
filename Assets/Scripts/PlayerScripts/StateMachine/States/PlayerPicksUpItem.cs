using System;
using Cysharp.Threading.Tasks;
using Inventory;
using StateMachine;
using UnityEngine;

namespace State
{
    public class PlayerPicksUpItem : PlayerBehaviour, IDisposable
    {
        public PlayerPicksUpItem(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
            playerStateMachine.AddDispose(this);
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Вход в поднятие предмета");
            await UniTask.WaitForSeconds(1f); //todo animation
            StateMachineData._isPickUpItem = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Выход из подьема предмета");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            StateMachine.TrySwapState<PlayerIdle>();
            StateMachine.TrySwapState<PlayerMovement>();
            StateMachine.TrySwapState<PlayerJumping>();
        }

        public override bool TrySwapState()
        {
            return StateMachineData.IsPickUpItem();
        }

        public void Dispose()
        {
            
        }
    }
}