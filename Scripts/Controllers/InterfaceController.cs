using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InterfaceController : MonoBehaviour, IPlayerUI
{
    List<RawImage> UI_Elements;
    List<Text> UI_Text;

    

    [SerializeField] Text health;
    [SerializeField] Text armor;
    [SerializeField] Text ammoType;
    [SerializeField] Text ammoCount;
    [SerializeField] Text activeWeapons;
    [SerializeField] Text ammoListText;

    [SerializeField] RawImage Portrait;

    [SerializeField] RawImage UI_CROSSHAIR;
    [SerializeField] RawImage UI_AMMO_COUNT;
    [SerializeField] RawImage UI_BOT_LEFT;
    [SerializeField] RawImage UI_BOT_RIGHT;
    [SerializeField] RawImage UI_BOT_MID;

    [SerializeField] RawImage HP_Bar;
    [SerializeField] RawImage Armor_Bar;

    [SerializeField] RawImage Ammo_Bar;
    [SerializeField] RawImage Wep_Cooldown;

    [SerializeField] RawImage Clip_Bar;
    [SerializeField] RawImage Shell_Bar;
    [SerializeField] RawImage Cell_Bar;
    [SerializeField] RawImage Rock_Bar;

    [SerializeField] Text WeaponType;
    [SerializeField] Text AmmoCount;

    [SerializeField] RawImage blueKey;
    [SerializeField] RawImage yellowKey;
    [SerializeField] RawImage redKey;

    [SerializeField] TextMeshProUGUI GAME_OVER;
    [SerializeField] RawImage EndGameScreen;

    float pickup_display_time = 2f;

    [SerializeField] TextMeshProUGUI item_pickup;

    

    private void Start()
    {
        UI_Text = new List<Text>();
        UI_Elements = new List<RawImage>();

        UI_Text.Add(health);
        UI_Text.Add(armor);
        UI_Text.Add(ammoType);
        UI_Text.Add(ammoCount);
        UI_Text.Add(ammoListText);
        UI_Text.Add(activeWeapons);

        UI_Elements.Add(UI_AMMO_COUNT);
        UI_Elements.Add(UI_BOT_LEFT);
        UI_Elements.Add(UI_BOT_MID);
        UI_Elements.Add(UI_BOT_RIGHT);
        UI_Elements.Add(UI_CROSSHAIR);

        UI_Elements.Add(Portrait);
        UI_Elements.Add(HP_Bar);
        UI_Elements.Add(Armor_Bar);
        UI_Elements.Add(Ammo_Bar);
        UI_Elements.Add(Wep_Cooldown);
        UI_Elements.Add(Clip_Bar);
        UI_Elements.Add(Shell_Bar);
        UI_Elements.Add(Cell_Bar);
        UI_Elements.Add(Rock_Bar);

        UI_Elements.Add(blueKey);
        UI_Elements.Add(yellowKey);
        UI_Elements.Add(redKey);

        EndGameScreen.enabled = false;
        GAME_OVER.enabled = false;

        item_pickup.text = "";
        item_pickup.overrideColorTags = true;
        item_pickup.color = Color.green;

        Clip_Bar.material = Instantiate<Material>(Clip_Bar.material);
        Shell_Bar.material = Instantiate<Material>(Shell_Bar.material);
        Cell_Bar.material = Instantiate<Material>(Cell_Bar.material);
        Rock_Bar.material = Instantiate<Material>(Rock_Bar.material);
    }

    public void PlayPickUpText(Keys.KeyType type)
    {
        switch (type)
        {
            case Keys.KeyType.Blue      :   { item_pickup.text = "Picked up Blue key"; item_pickup.color = Color.blue;    } break;
            case Keys.KeyType.Yellow    :   { item_pickup.text = "Picked up Yellow key"; item_pickup.color = Color.yellow;  } break;
            case Keys.KeyType.Red       :   { item_pickup.text = "Picked up Red key"; item_pickup.color = Color.red;     } break;
        }

        StopCoroutine("ClearPickupMessage");
        StartCoroutine("ClearPickupMessage");
    }

    IEnumerator ClearPickupMessage()
    {
        yield return new WaitForSeconds(pickup_display_time);

        item_pickup.text = "";
    }
    public void UpdateHealth(float health)
    {
        HP_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(health, 0, 1));
    }
    public void UpdateArmor(float armor)
    {
        Armor_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(armor, 0, 1));
    }

    public void UpdateAmmo(float ammo, int count)
    {
        AmmoCount.text = count.ToString();
        Ammo_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(ammo, 0, 1));
    }
    public void UpdateReload(float reload)
    {
        Wep_Cooldown.material.SetFloat("_FillPercent", Mathf.Clamp(reload, 0, 1));
    }
    public void UpdateType(Weapons.WeaponType type)
    {
        WeaponType.text = type.ToString();
    }

    public void UpdateKeys(bool blue, bool yellow, bool red)
    {
        blueKey.enabled = blue;
        yellowKey.enabled = yellow;
        redKey.enabled = red;
    }

    public void ShowEndScreen()
    {
        foreach (RawImage element in UI_Elements)
            if (element != null)
                element.enabled = false;

        foreach (Text text in UI_Text)
            if (text != null)
                text.enabled = false;

        GAME_OVER.enabled = true;

        GameController.Instance.input.gameOver = true;
        EndGameScreen.enabled = true;
    }

    public void EnableInterfaceAtRestart()
    {
        foreach (RawImage element in UI_Elements)
            if (element != null)
                element.enabled = true;

        foreach (Text text in UI_Text)
            if (text != null)
                text.enabled = true;

        GAME_OVER.enabled = false;

        GameController.Instance.input.gameOver = false;
        EndGameScreen.enabled = false;
    }
    public void UpdateOverview(float clip, float shell, float cell, float rock)
    {
        Clip_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(clip, 0, 1));
        Shell_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(shell, 0, 1));
        Cell_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(cell, 0, 1));
        Rock_Bar.material.SetFloat("_FillPercent", Mathf.Clamp(rock, 0, 1));
    }
}
