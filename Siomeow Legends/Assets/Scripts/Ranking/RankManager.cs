using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    public List<PlayerData> playerDatas = new List<PlayerData>();
    public Group[] groups;
    private void Start(){
        CalKillScore();
        Debug.Log(CalKillScore());
    }


    private void Update(){
        playerDatas.Sort(SortByKills);
        UpdateRank();
    }

    //Calculate Kill Score
    private string CalKillScore(){
        int killScore = 0;
        string topName  = "";

        for(int i = 0; i < playerDatas.Count; i++){

            //a player has greater score than the other
            if(playerDatas[i].playerKills > killScore){
                killScore = playerDatas[i].playerKills;
                topName = playerDatas[i].playerName;
            } 
        }

        return topName;
    }

    private int SortByKills(PlayerData _playerA, PlayerData _playerB){
        return _playerB.playerKills.CompareTo(_playerA.playerKills);
    }

    private void UpdateRank(){
        for(int i = 0; i < playerDatas.Count; i ++){
            groups[i].playerData = playerDatas[i];
            groups[i].UpdateGroup();
        }
    }
}
