using System;
using UnityEngine;

public class PlayerSystem
{
    private GameObject characterPrefab, character;
    private PlayerController playerController;

    public void Init() {
        characterPrefab = ResourceManager.Instance.LoadAsset(AssetUtility.GetPrefab("PlayerCharacter")) as GameObject;

        SpawnPlayerCharacter();
    }

    private void SpawnPlayerCharacter() {
        character = GameObject.Instantiate(characterPrefab, new Vector2(-1, -3), Quaternion.identity);
        playerController = character.GetComponent<PlayerController>();
    }

    public void Shutdown() {
        if(character)GameObject.Destroy(character);

        character = null;
        playerController = null;
    }

    public void Update() {
        if (!playerController) return;

        playerController.UpdateMovement();
    }

    public void LateUpdate() {
        if (!playerController) return;

        playerController.UpdateAnimation();
    }
}