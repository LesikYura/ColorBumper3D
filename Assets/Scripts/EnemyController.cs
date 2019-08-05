using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private List<Renderer> _objRenderList;
    [SerializeField] private List<GameObject> _objList;

    public void SetMaterials(List<Material> materialsList, int playerIndex)
    {
        var enemyCount = 0;
        for (var i = 0; i < _objList.Count; i++)
        {
            var currentMaterialIndex = Random.Range(0, materialsList.Count);

            _objRenderList[i].material = materialsList[currentMaterialIndex];
            if (currentMaterialIndex != playerIndex)
            {
                _objList[i].tag = "Enemy";
                enemyCount++;
            }
        }

        if (enemyCount == _objList.Count)
        {
            var count = _objList.Count - 1;
            _objRenderList[count].material = materialsList[playerIndex];
            _objList[count].tag = "Untagged";
        }
    }
    
}
