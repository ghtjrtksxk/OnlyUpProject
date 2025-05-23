using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
    public void ItemEffect();
}
public class ItemObject : MonoBehaviour
{
    public ItemData data;
    public Player _player;

    private Coroutine coroutine;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }

    public void ItemEffect()
    {
        _player.controller.ItemEffect(data);
    }
}
