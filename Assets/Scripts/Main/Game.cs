using UnityEngine;

public class Game : MonoBehaviour
{
    private ResourceManager resourceManager;
    private DataTableManager dataTableManager;
    private BagpackSystem bagpackSystem;
    private EventManager eventManager;

    private SceneObjSystem sceneObjectSystem;

    private PlayerSystem playerSystem;
    private PlayerCameraSystem playerCameraSystem;

    private UIManager uiManager;

    private bool pauseGame;

    private void Awake() {
        Init();
    }

    private void Init() {
        InitSystems();

        eventManager.Subscribe(TriggerEndGameEventArgs.EventId, OnTriggerEndGame);
    }

    private void InitSystems() {
        resourceManager = new ResourceManager();
        dataTableManager = new DataTableManager();
        bagpackSystem = new BagpackSystem();
        eventManager = new EventManager();
        playerSystem = new PlayerSystem();
        playerCameraSystem = new PlayerCameraSystem();
        sceneObjectSystem = new SceneObjSystem();

        uiManager = FindObjectOfType<UIManager>();

        eventManager.Init();
        dataTableManager.Init();
        dataTableManager.SetDataTableHelper(new DefaultDataTableHelper());

        resourceManager.Init();
        bagpackSystem.Init();
        playerSystem.Init();
        playerCameraSystem.Init();
        sceneObjectSystem.Init();

        uiManager.Init();
    }

    private void Update() {
        if (pauseGame) return;

        playerSystem.Update();
    }

    private void LateUpdate() {
        playerSystem.LateUpdate();
        playerCameraSystem.Update();
    }

    private void OnTriggerEndGame(object sender, GameEventArgs e) {
        pauseGame = true;

        SystemUI.Instance.OpenDialog("重新開始遊戲?", () => { pauseGame = false; RestartGame(); },
            () => { pauseGame = false; });
    }

    private void ShutdownSystems() {
        playerCameraSystem.Shutdown();
        playerSystem.Shutdown();
        sceneObjectSystem.Shutdown();
    }

    private void RestartGame() {
        ShutdownSystems();

        Init();
    }
}