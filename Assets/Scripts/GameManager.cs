using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTask
{
    public class GameManager : Singleton<GameManager>
    {
        bool hasGameStarted;

        void Start()
        {
            hasGameStarted = false;
            PlayerDataManager.Instance.Create(
                () => {
                    hasGameStarted = true;

                    InitManagers();
                }
            );
        }

        void InitManagers()
        {
            GridManager.Instance.InitManager();
            LevelManager.Instance.InitManager();
        }

        void Update()
        {
            if (hasGameStarted)
            {
                GridManager.Instance.UpdateManager();
                LevelManager.Instance.UpdateManager();
            }
        }
    }
}