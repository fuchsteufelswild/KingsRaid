using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CompanionItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Companion companion;
    public Image image;

    private void Awake()
    {
        if (companion == null)
            image.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && companion != null)
        {
            if (companion != null)
            {
                if (Character.instance.coin >= companion.coinToDrop)
                    Character.instance.coin -= companion.coinToDrop;
                else
                    return;

                CanvasScript.instance.inventoryPanel.GetComponent<InventoryPanel>().coin.text = "Coin:\n " + Character.instance.coin;
                Character.instance.audioSource.PlayOneShot(GameObjectContainers.instance.GetAudioClip(0));
                Character.instance.Subscribe(this.companion);
                this.companion = null;
                image.gameObject.SetActive(false);
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (companion == null)
        {
            CanvasScript.instance.companionInfo.SetActive(false);
            return;
        }

        
        Vector3 cameraPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 infoPos = Camera.main.ScreenToWorldPoint(this.transform.position);

        if (cameraPos.x + 6.4 > infoPos.x + 3.0f)
            CanvasScript.instance.companionInfo.transform.position = new Vector3(this.transform.position.x + 420.0f, this.transform.position.y, this.transform.position.z);
        else if (cameraPos.x < infoPos.x - 3.0f)
            CanvasScript.instance.companionInfo.transform.position = new Vector3(this.transform.position.x - 420.0f, this.transform.position.y, this.transform.position.z);
        
        CanvasScript.instance.companionInfo.SetActive(true);
        


        CompanionInfoScript companionInfo = CanvasScript.instance.companionInfo.GetComponent<CompanionInfoScript>();
        companionInfo.health.text = "Health: " + this.companion.health;
        companionInfo.companionName.text = this.companion.actorName;
        companionInfo.damage.text = "Damage: " + companion.damage.ToString();
        companionInfo.value.text = "Value:  " + companion.coinToDrop.ToString();
        companionInfo.companionIcon.sprite = companion.actorIcon;
        companionInfo.companionSprite.sprite = companion.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanvasScript.instance.companionInfo.SetActive(false);
    }

}
