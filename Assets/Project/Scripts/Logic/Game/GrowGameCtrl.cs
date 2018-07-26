using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GrowGameCtrl : MonoBehaviour {

    private static GrowGameCtrl instance = null;

    public static GrowGameCtrl GetInstance() {
        if (instance == null) {
            instance = GameObject.Find("GameCtrl").GetComponent<GrowGameCtrl>();
        }
        return instance;
    }

    public List<MonCtrl> monList;
    public List<GameObject> monPrefabList;

    // Use this for initialization
    void Start () {
        CreatMonByCount(new int[] { 0, 1 }, 10);

    }
	
	// Update is called once per frame
	void Update () {
        TouchAndCreat();

    }

    public MonCtrl GetCloseMon(MonCtrl self) {
        MonCtrl result = null;
        for (int i = 0;i<monList.Count;i++)
        {
            if (self != monList[i]) {
                if (result == null)
                {
                    result = monList[i];
                }
                else if(Vector3.Distance(self.transform.position,result.transform.position) > 
                    Vector3.Distance(self.transform.position, monList[i].transform.position)) {
                    result = monList[i];
                }
            }
        }

        return result;         
    }

    public void RemoveMon(MonCtrl monCtrl) {
        monList.Remove(monCtrl);
    }



    public void CreatMon(int id, Vector3 pos) {
        MonCtrl temp = (Instantiate(monPrefabList[id], pos, Quaternion.identity) as GameObject).GetComponent<MonCtrl>();
        monList.Add(temp);
    }

    public void CreatMonByCount(int[] ids, int count) {
        for (int i = 0; i < count; i++) {
            CreatMon(ids[Random.Range(0, ids.Length - 1)], new Vector3(Random.Range(
                -LocalData.SCENE_HALF_WEIGHT, LocalData.SCENE_HALF_WEIGHT),
                Random.Range(-LocalData.SCENE_HALF_HEIGHT,
            LocalData.SCENE_HALF_HEIGHT), 0));
        }
    }

    public void TouchAndCreat() {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dianV = Input.mousePosition;
            dianV.z = 0;
            Vector3 wv = Camera.main.ScreenToWorldPoint(dianV);
            CreatMon(0, wv);
        }
    }
}
