﻿using UnityEngine;

public class SSTriggerScore : MonoBehaviour
{
    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    /// <summary>
    /// 是否为空心球得分.
    /// </summary>
    [HideInInspector]
    public bool IsKongXiBallScore = false;

    public void Init()
    {
        IsKongXiBallScore = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        SSBallMoveCtrl ballMove = other.GetComponent<SSBallMoveCtrl>();
        if (ballMove != null && !ballMove.IsDeFenQiu)
        {
            ballMove.IsDeFenQiu = true;
            bool isKongXiQiu = false;
            if (ballMove.CountOnHit == 0)
            {
                isKongXiQiu = true;
                Debug.Log("SSTriggerScore -> KongXinQiu, m_PlayerIndex == " + m_PlayerIndex);
            }
            IsKongXiBallScore = isKongXiQiu;

            int ballScore = 0;
            switch (ballMove.m_BallMoveData.m_LanQiuType)
            {
                case SSGameDataCtrl.LanQiuType.PuTong:
                    {
                        if (isKongXiQiu)
                        {
                            ballScore = SSGameDataCtrl.GetInstance().m_PuTongBallScoreDt.KongXinQiu;
                        }
                        else
                        {
                            ballScore = SSGameDataCtrl.GetInstance().m_PuTongBallScoreDt.PuTongQiu;
                        }
                        break;
                    }
                case SSGameDataCtrl.LanQiuType.HuaShi:
                    {
                        if (isKongXiQiu)
                        {
                            ballScore = SSGameDataCtrl.GetInstance().m_HuaShiBallScoreDt.KongXinQiu;
                        }
                        else
                        {
                            ballScore = SSGameDataCtrl.GetInstance().m_HuaShiBallScoreDt.PuTongQiu;
                        }
                        break;
                    }
            }

            if (ballMove.m_BallMoveData.IsYanWuTXBall)
            {
                //烟雾特效篮球分数加倍.
                ballScore *= SSGameDataCtrl.GetInstance().m_YanWuTXBallScoreBL;
            }

            SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score += ballScore;
            Debug.Log("SSTriggerScore -> Score == " + SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score + ", m_PlayerIndex == " + m_PlayerIndex);
        }
    }
}