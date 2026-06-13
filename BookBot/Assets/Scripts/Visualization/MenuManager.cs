using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuManager : MonoBehaviour
{
    public Slider bookCountSlider;
    public TextMeshProUGUI bookCountText;

    public Slider speedSlider;
    public TextMeshProUGUI speedText;

    public TMP_Dropdown algorithmDropdown;
    public Button startButton;


    public GameManager gameManager;
    public GameObject menuPanel;

    void Start()
    {
     
        bookCountSlider.value = SimulationSettings.booksToGenerate;
        speedSlider.value = SimulationSettings.robotSpeed;
        algorithmDropdown.value = SimulationSettings.useDijkstra ? 1 : 0;

        UpdateTextDisplays();

        bookCountSlider.onValueChanged.AddListener(delegate { UpdateSettings(); });
        speedSlider.onValueChanged.AddListener(delegate { UpdateSettings(); });
        algorithmDropdown.onValueChanged.AddListener(delegate { UpdateSettings(); });


        startButton.onClick.AddListener(StartSimulation);
    }

    public void UpdateSettings()
    {
        SimulationSettings.booksToGenerate = (int)bookCountSlider.value;
        SimulationSettings.robotSpeed = speedSlider.value;
        
        SimulationSettings.useDijkstra = (algorithmDropdown.value == 1);
        UpdateTextDisplays();
    }

    private void UpdateTextDisplays()
    {
        
        bookCountText.text = $"Books to Sort: {SimulationSettings.booksToGenerate}";
        speedText.text = $"Robot Speed: {SimulationSettings.robotSpeed:F1}";
    }

    public void StartSimulation()
    {
        
        menuPanel.SetActive(false);

        gameManager.StartSimulation();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Debug.Log("Simulation Complete! Opening menu for the next run.");
    }

}
