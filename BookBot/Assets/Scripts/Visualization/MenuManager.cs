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
   


    public GameManager gameManager;
    public GameObject menuPanel;

    void Start()
    {
     
        bookCountSlider.value = SimulationSettings.booksToGenerate;
        speedSlider.value = SimulationSettings.robotSpeed;
        if (SimulationSettings.useDijkstra == true)
        {
            algorithmDropdown.value = 1;
        }
        else
        {
            algorithmDropdown.value = 0;
        }   
        UpdateTextDisplays();

    }

    public void UpdateSettings()
    {
        SimulationSettings.booksToGenerate = (int)bookCountSlider.value;
        SimulationSettings.robotSpeed = speedSlider.value;
        
        SimulationSettings.useDijkstra = algorithmDropdown.value == 1;
        UpdateTextDisplays();
    }

    private void UpdateTextDisplays()
    {
        
        bookCountText.text = "Books to Sort" + SimulationSettings.booksToGenerate;
        speedText.text = "Robot Speed:" + SimulationSettings.robotSpeed.ToString("F1");
    }

    public void StartSimulation()
    {
        
        menuPanel.SetActive(false);

        gameManager.StartSimulation();
    }


    //if all books are sorted
    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Debug.Log("Simulation Complete! Opening menu for the next run.");
    }

}
