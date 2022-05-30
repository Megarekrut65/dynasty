using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that control card covering
/// </summary>
public class CoverCard : MonoBehaviour {
    [SerializeField]
    private GameObject coverImage;
    [SerializeField]
    public GameObject coverContainer;
    
    public void Cover() {
        coverImage.SetActive(true);
    }
    public void Uncover() {
        coverImage.SetActive(false);
    }
    public void AddToContainer(GameObject obj) {
        obj.transform.SetParent(
            coverContainer.transform.childCount == 0 ? coverContainer.transform : coverContainer.transform.GetChild(0),
            false);
        StartCoroutine(Alignment());
    }
    private IEnumerator Alignment() {
        yield return new WaitForSeconds(0.1f);
        var group = coverContainer.GetComponent<HorizontalLayoutGroup>();
        group.childAlignment = TextAnchor.MiddleCenter;
    }
}