using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject[] Player;
    public GameObject[] Object;
    public GameObject ObjectToSpawn;
    //public GameObject[] AllObjectInScene;
    public float delayPerSpawn = 2.0f;
    private int maxSpawn = 30;
    public int currentNumSpawn = 0;
    private bool spawning = true;
    private bool changeSize = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(spawning == true && currentNumSpawn < maxSpawn)
        {
            StartCoroutine(SpawnObject());
        }
    }

    IEnumerator SpawnObject()
    {
        spawning = false;
        yield return new WaitForSeconds(delayPerSpawn);
        GameObject clone = Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
        currentNumSpawn++;
        //AllObjectInScene.SetValue(go, AllObjectInScene.Length);
        spawning = true;
    }
}


/*ifstream infile(“Level_1.txt”);
string line;
std::vector<float> values;
while (getline(infile, line)) {
   values.push_back(atof(line.c_str()));
}
for (size_t i =0; i<values.size(); i++){
if(current_time == values[i]){
#generate a new object;
}*/